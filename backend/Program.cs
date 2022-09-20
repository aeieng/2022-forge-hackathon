using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using Autodesk.Forge;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using Autodesk.Forge.DesignAutomation.Model;
using System.Net;
using Autodesk.Forge.Model;
using System;
using Autodesk.Forge.Core;
using Autodesk.Forge.DesignAutomation;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddDbContext<BackendDbContext>
        (o => { o.UseNpgsql(configuration["ConnectionStrings:Postgres"]);});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Welcome to Team HackOverflow API for 2022 Forge Hackathon");

#region Auth
app.MapGet("/token", async () =>
{
    var token = await new TwoLeggedApi().AuthenticateAsync(
        configuration["Forge:ClientId"],
        configuration["Forge:ClientSecret"],
        "client_credentials", new Scope[] { Scope.DataWrite, Scope.DataRead, Scope.BucketCreate, Scope.BucketRead });

    return new Token(token.access_token, DateTime.UtcNow.AddSeconds(token.expires_in));
});

#endregion

#region Models

app.MapGet("/models", async (BackendDbContext db) => await db.Models.ToListAsync());
app.MapPost("/add-model", async (ModelInput input, BackendDbContext db) =>
{
    var model = new Model(input);

    db.Models.Add(model);
    await db.SaveChangesAsync();
    
    return Results.Created($"/models/{model.Id}", model);
});
app.MapDelete("/models/{id}", async (Guid modelId, BackendDbContext db) =>
{
    if (await db.Models.FindAsync(modelId) is not { } model) return Results.NotFound();

    db.Models.Remove(model);
    await db.SaveChangesAsync();
    
    return Results.Ok(model);
});

#endregion

#region Activities
app.MapGet("/activity-types", () =>
{
    Dictionary<string, object> activityTypes = new Dictionary<string, object>();

    var activityTypesList = new List<string>();
    activityTypesList.Add("Architectural");
    activityTypesList.Add("Structural");
    activityTypesList.Add("Mechanical");
    activityTypesList.Add("Electrical");

    activityTypes.Add("activityTypes", activityTypesList);

    return activityTypes;
});
app.MapGet("/activities", async (BackendDbContext db) => await db.Activities.ToListAsync());
app.MapGet("/selected-activities", async (BackendDbContext db) => await db.SelectedActivities.ToListAsync());
app.MapPost("/selected-activities", async (List<Guid> selectedActivitiesIds, BackendDbContext db) =>
{
    foreach (var selectedActivityId in selectedActivitiesIds)
    {
        var selectedActivity = new SelectedActivity() { ActivityId = selectedActivityId };
        db.SelectedActivities.Add(selectedActivity);
    }
    
    await db.SaveChangesAsync();

    return Results.Ok();
});
app.MapDelete("/selected-activities/{id}", async (Guid activityId, BackendDbContext db) =>
{
    if (await db.SelectedActivities.FindAsync(activityId) is not { } selectedActivity) return Results.NotFound();

    db.SelectedActivities.Remove(selectedActivity);
    await db.SaveChangesAsync();

    return Results.Ok();
});
#endregion

#region ExtractionLog



app.MapGet("/extraction-log", async (BackendDbContext db) => await db.ExtractionLog.ToListAsync());
app.MapPost("/run-activities", async (List<Guid> modelIds, BackendDbContext db) =>
{
    var extractionLog = new ExtractionLog()
    {
        Id = Guid.NewGuid(),
    };

    await db.ExtractionLog.AddAsync(extractionLog);

    ForgeService service =
                new ForgeService(
                    new HttpClient(
                        new ForgeHandler(Microsoft.Extensions.Options.Options.Create(new ForgeConfiguration()
                        {
                            ClientId = configuration["Forge:ClientId"],
                            ClientSecret = configuration["Forge:ClientSecret"]
                        }))
                        {
                            InnerHandler = new HttpClientHandler()
                        })
                );
    var designAutomationClient = new DesignAutomationClient(service);

    var ActivityFullName = "";

    VersionsApi versionApi = new VersionsApi();
    versionApi.Configuration.AccessToken = userAccessToken;
    dynamic version = await versionApi.GetVersionAsync(projectId, versionId);
    dynamic versionItem = await versionApi.GetVersionItemAsync(projectId, versionId);

    string[] versionItemParams = ((string)version.data.relationships.storage.data.id).Split('/');
    string[] bucketKeyParams = versionItemParams[versionItemParams.Length - 2].Split(':');
    string bucketKey = bucketKeyParams[bucketKeyParams.Length - 1];
    string objectName = versionItemParams[versionItemParams.Length - 1];
    string downloadUrl = string.Format("https://developer.api.autodesk.com/oss/v2/buckets/{0}/objects/{1}", bucketKey, objectName);

    var auth = await new TwoLeggedApi().AuthenticateAsync(
        configuration["Forge:ClientId"],
        configuration["Forge:ClientSecret"],
        "client_credentials", new Scope[] { Scope.DataWrite, Scope.DataRead, Scope.BucketCreate, Scope.BucketRead });

    var downloadURL = new XrefTreeArgument()
    {
        Url = string.Format("https://developer.api.autodesk.com/oss/v2/buckets/{0}/objects/{1}", bucketKey, objectName),
        Verb = Verb.Get,
        Headers = new Dictionary<string, string>()
                {
                    { "Authorization", "Bearer " + auth.access_token }
                }
    };

    string bucketName = "revitdesigncheck" + NickName.ToLower();
    BucketsApi buckets = new BucketsApi();
    //dynamic token = await Credentials.Get2LeggedTokenAsync(new Scope[] { Scope.BucketCreate, Scope.DataWrite });
    buckets.Configuration.AccessToken = auth.access_token;
    PostBucketsPayload bucketPayload = new PostBucketsPayload(bucketName, null, PostBucketsPayload.PolicyKeyEnum.Transient);
    try
    {
        await buckets.CreateBucketAsync(bucketPayload, "US");
    }
    catch { }

    ObjectsApi objects = new ObjectsApi();
    dynamic signedUrl = await objects.CreateSignedResourceAsyncWithHttpInfo(bucketName, resultFilename, new PostBucketsSigned(5), "readwrite");

    var uploadUrl = new XrefTreeArgument()
    {
        Url = (string)(signedUrl.Data.signedUrl),
        Verb = Verb.Put
    };

    string callbackUrl = string.Format("{0}/api/forge/callback/designautomation/{1}/{2}/{3}/{4}", Credentials.GetAppSetting("FORGE_WEBHOOK_URL"), userId, hubId, projectId, versionId.Base64Encode());

    WorkItem workItemSpec = new WorkItem()
    {
        ActivityId = ActivityFullName,
        Arguments = new Dictionary<string, IArgument>()
        {
            { "inputFile", await BuildDownloadURL(credentials.TokenInternal, projectId, versionId) },
            { "result",  await BuildUploadURL(resultFilename) },
            { "onComplete", new XrefTreeArgument { Verb = Verb.Post, Url = callbackUrl } }
        }
    };

    // Download Revit file from ACC 
    // Use data management bucket created by Eric upload model {Guid}.rvt,  inputParams : { "Operation": "Mechanical" } Rvt Version: 2022
    // Queue up design automation
    // Create web hooks or poll if runing out of time
    // Register call backs

    WorkItemStatus workItemStatus = await designAutomationClient.CreateWorkItemAsync(workItemSpec);

    
});

#endregion

#region Buildings

app.MapGet("/buildings", async (BackendDbContext db) => await db.Buildings.ToListAsync());


#endregion

app.Run();
