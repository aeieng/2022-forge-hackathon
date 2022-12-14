using System.Text.Json;
using Autodesk.Forge;
using Autodesk.Forge.Core;
using Autodesk.Forge.DesignAutomation;
using Autodesk.Forge.DesignAutomation.Model;
using Autodesk.Forge.Model;
using Backend.Entities;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using AEIRevitDesignAutomation.Models;

#region Configs

const string hostUrl = "https://f632-67-161-93-69.ngrok.io";

const string Architectural = "Architectural";
const string Electrical = "Electrical";
const string Mechanical = "Mechanical";
string[] Operations = { Architectural, Electrical, Mechanical };

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
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#endregion

app.MapGet("/", () => "Welcome to Team HackOverflow API for 2022 Forge Hackathon");

#region Auth Endpoints
app.MapGet("/token", async () =>
{
    var token = await new TwoLeggedApi().AuthenticateAsync(
        configuration["Forge:ClientId"],
        configuration["Forge:ClientSecret"],
        "client_credentials", new Scope[] { Scope.DataWrite, Scope.DataRead, Scope.BucketCreate, Scope.BucketRead });

    return new Token(token.access_token, DateTime.UtcNow.AddSeconds(token.expires_in));
});
#endregion

#region Model Endpoints
app.MapGet("/models", async (BackendDbContext db) => await db.Models.ToListAsync());

app.MapPost("/add-model", async (ModelInput input, BackendDbContext db) =>
{
    var model = new Model(input);

    db.Models.Add(model);
    await db.SaveChangesAsync();
    
    return Results.Created($"/models/{model.Id}", model);
});

app.MapDelete("/models/{modelId}", async (Guid modelId, BackendDbContext db) =>
{
    if (await db.Models.FindAsync(modelId) is not { } model) return Results.NotFound();

    db.Models.Remove(model);
    await db.SaveChangesAsync();
    
    return Results.Ok(model);
});
#endregion

#region Activity Endpoints
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

app.MapDelete("/selected-activities/{activityId}", async (Guid activityId, BackendDbContext db) =>
{
    if (await db.SelectedActivities.FindAsync(activityId) is not { } selectedActivity) return Results.NotFound();

    db.SelectedActivities.Remove(selectedActivity);
    await db.SaveChangesAsync();

    return Results.Ok();
});
#endregion

#region Extraction Log Endpoints

app.MapGet("/extraction-log", async (BackendDbContext db) => await db.ExtractionLog.ToListAsync());

app.MapPost("/run", async (string operation, Guid modelId, BackendDbContext db) =>
{
    if (!Operations.Contains(operation)) return Results.BadRequest();

    var auth = await new TwoLeggedApi().AuthenticateAsync(
        configuration["Forge:ClientId"],
        configuration["Forge:ClientSecret"],
        "client_credentials", new []
        {
            Scope.DataWrite, 
            Scope.DataRead, 
            Scope.BucketCreate, 
            Scope.BucketRead
        });

    var forgeService = 
        new ForgeService(
            new HttpClient(
                new ForgeHandler(Microsoft.Extensions.Options.Options.Create(new ForgeConfiguration()
                    {
                        ClientId = configuration["Forge:ClientId"],
                        ClientSecret = configuration["Forge:ClientSecret"]
                    }))
                    {
                        InnerHandler = new HttpClientHandler()
                    }));
    
    var designAutomationClient = new DesignAutomationClient(forgeService);

    var versionApi = new VersionsApi
    {
        Configuration =
        {
            AccessToken = auth.access_token
        }
    };
    var version = await versionApi.GetVersionAsync("b.6f34ae9f-59a3-464a-9386-5b9a93a41484", "urn:adsk.wipprod:fs.file:vf.4cfhsnE7SAKz4LXuOXyYyA?version=1");
    var versionItemParams = ((string)version.data.relationships.storage.data.id).Split('/');
    var bucketKeyParams = versionItemParams[^2].Split(':');
    var bucketKey = bucketKeyParams[^1];
    var objectName = versionItemParams[^1];

    var downloadUrl = new XrefTreeArgument
    {
        Url = $"https://developer.api.autodesk.com/oss/v2/buckets/{bucketKey}/objects/{objectName}",
        Verb = Verb.Get,
        Headers = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + auth.access_token }
                }
    };

    // TODO: pick final bucket
    var bucketName = "arif_test";
    BucketsApi buckets = new BucketsApi
    {
        Configuration =
        {
            AccessToken = auth.access_token
        }
    };
    var bucketPayload = 
        new PostBucketsPayload(bucketName, null, PostBucketsPayload.PolicyKeyEnum.Transient);
    try
    {
        await buckets.CreateBucketAsync(bucketPayload, "US");
    }
    catch (Exception)
    { }

    ObjectsApi objects = new ObjectsApi();
    dynamic signedUrlResponse = await objects.CreateSignedResourceAsyncWithHttpInfo(bucketName, "result.json", new PostBucketsSigned(60), "readwrite");
    var signedUrl = (string)signedUrlResponse.Data.signedUrl;

    var uploadUrl = new XrefTreeArgument
    {
        Url = signedUrl,
        Verb = Verb.Put
    };

    var extractionLogId = Guid.NewGuid();
    // TODO: change to exposed callback URL
    string callbackUrl = $"{hostUrl}/process-results/{extractionLogId}"; //string.Format("{0}/api/forge/callback/designautomation/{1}/{2}/{3}/{4}", Credentials.GetAppSetting("FORGE_WEBHOOK_URL"), userId, hubId, projectId, versionId.Base64Encode());

    var inputParams = new XrefTreeArgument
    {
        Url = $"data:application/json,{{ \"Operation\": \"{operation}\" }}"
    };

    var workItemSpec = new WorkItem()
    {
        ActivityId = "CqRjmmTMt7TGXSOpPAuVuWGQYHPNwZXZ.AEIRevitDesignAutomationActivity+test",
        Arguments = new Dictionary<string, IArgument>()
        {
            { "rvtFile", downloadUrl },
            { "inputParams",  inputParams},
            { "result",  uploadUrl },
            { "onComplete", new XrefTreeArgument { Verb = Verb.Post, Url = callbackUrl } }
        }
    };

    WorkItemStatus workItemStatus = await designAutomationClient.CreateWorkItemAsync(workItemSpec);
    
    var extractionLog = new ExtractionLog
    {
        Id = extractionLogId,
        StartedRunAtUtc = DateTime.UtcNow,
        ModelId = modelId,
        Operation = operation,
        ResultSignedUrl = signedUrl,
        Status = workItemStatus.Status == Status.Inprogress ? Status.Inprogress.ToString() : "Started",
        DesignAutomationWorkItemId = workItemStatus.Id
    };
    
    await db.ExtractionLog.AddAsync(extractionLog);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapPost("/process-results/{extractionLogId}", async (Guid extractionLogId, BackendDbContext db) =>
{
    var extractionLog = db.ExtractionLog.FirstOrDefault(o => o.Id == extractionLogId);
    if (extractionLog == default) return Results.NotFound("No extraction log found");

    var model = db.Models.FirstOrDefault(o => o.Id == extractionLog.ModelId);
    if (model == default)
    {
        extractionLog.Status = "Failed";
        await db.SaveChangesAsync();
        return Results.NotFound("No model found");
    }

    string resultJson;
    try
    {
        using var httpClient = new HttpClient();
        using var httpResponse = await httpClient.GetAsync(extractionLog.ResultSignedUrl, HttpCompletionOption.ResponseHeadersRead);
        httpResponse.EnsureSuccessStatusCode();
        resultJson = await httpResponse.Content.ReadAsStringAsync();
    }
    catch (Exception e)
    {
        extractionLog.Status = "Failed";
        await db.SaveChangesAsync();
        return Results.Problem($"Exception occurred: {e}");
    }


    switch (extractionLog.Operation)
    {
        case Architectural:
            var aResponse = JsonSerializer.Deserialize<ArchitecturalResponse>(resultJson);
            model.ModelData.ExteriorWallArea = aResponse.ExteriorWallArea;
            model.ModelData.GlazingArea = aResponse.GlazingArea;
            if (model.Rooms?.Any() ?? false)
            {
                model.Rooms.Clear();
                await db.SaveChangesAsync();
            }
            model.Rooms = aResponse.Rooms.Select(o => new Room
            {
                Id = Guid.NewGuid().ToString(),
                ModelId = extractionLog.ModelId,
                Name = o.Name,
                Number = o.Number,
                ElementId = o.ElementId,
                FloorArea = o.Area
            }).ToList();
            break;

        case Electrical:
            var eResponse = JsonSerializer.Deserialize<ElectricalResponse>(resultJson);
            model.ModelData.NumberOfCircuits = eResponse.NumberOfCircuits;
            model.ModelData.NumberOfLightingFixtures = eResponse.NumberOfLightingFixtures;
            break;

        case Mechanical:
            var mResponse = JsonSerializer.Deserialize<MechanicalResponse>(resultJson);
            model.ModelData.DuctSurfaceArea = mResponse.DuctSurfaceArea;
            model.ModelData.TotalPipeLength = mResponse.TotalPipeLength;
            break;

        default:
            extractionLog.Status = "Failed";
            await db.SaveChangesAsync();
            return Results.Problem($"Invalid operation: {extractionLog.Operation}");
    }

    extractionLog.Status = "Success";
    await db.SaveChangesAsync();

    return Results.Ok();
});

#endregion

#region Building Endpoints

// Calculate

app.MapGet("/building", async (Guid buildingId, BackendDbContext db) =>
{
    return await db.Buildings.FirstOrDefaultAsync(i => i.Id == buildingId);
});

app.MapGet("/buildings", async (BackendDbContext db) => await db.Buildings.ToListAsync());

app.MapGet("/building-cost", async (Guid buildingId, BackendDbContext db) => 
    await db.BuildingCosts.FirstOrDefaultAsync(i => i.BuildingId == buildingId));

app.MapGet("/building-program", async (Guid buildingId, BackendDbContext db) =>
    await db.BuildingRoomTypes.Where(i => i.BuildingId == buildingId).ToListAsync());

app.MapGet("/building-operational-carbon", async (Guid buildingId, BackendDbContext db) =>
    await db.BuildingOperationalCarbons.FirstOrDefaultAsync(i => i.BuildingId == buildingId));

app.MapGet("/building-materials", async (Guid buildingId, BackendDbContext db) => 
    await db.Materials.Where(i => i.BuildingId == buildingId).ToListAsync());

app.MapPost("/building", async (CreateBuildingInput input, BackendDbContext db) =>
{
    var building = new Building(input);
    await db.Buildings.AddAsync(building);
    await db.SaveChangesAsync();
});

app.MapPut("/building", async (Building building, BackendDbContext db ) =>
{
    db.Buildings.Update(building);
    await db.SaveChangesAsync();
});

app.MapPost("/building-cost", async (BuildingCost buildingCostInput, BackendDbContext db) =>
{
    var buildingCost = await db.BuildingCosts.FirstOrDefaultAsync(i => i.BuildingId == buildingCostInput.BuildingId);
    
    if (buildingCost == default)
    {
        await db.BuildingCosts.AddAsync(buildingCostInput);
    }
    else
    {
        buildingCost.ArchitecturalCost = buildingCostInput.ArchitecturalCost;
        buildingCost.StructuralCost = buildingCostInput.StructuralCost;
        buildingCost.MechanicalCost = buildingCostInput.MechanicalCost;
        buildingCost.ElectricalCost = buildingCostInput.ElectricalCost;
        buildingCost.PipingCost = buildingCostInput.PipingCost;
    }
    
    await db.SaveChangesAsync();
});

app.MapPost("/building-program", async (Guid buildingId, List<BuildingRoomTypeInput> buildingRoomTypesInput, BackendDbContext db) =>
{
    var existing = db.BuildingRoomTypes.Where(i => i.BuildingId == buildingId);
    db.BuildingRoomTypes.RemoveRange(existing);

    var buildingRoomTypes = buildingRoomTypesInput.Select(input => new BuildingRoomType(buildingId, input)).ToList();
    await db.BuildingRoomTypes.AddRangeAsync(buildingRoomTypes);
    await db.SaveChangesAsync();

    var building = await db.Buildings.FirstOrDefaultAsync(i => i.Id == buildingId);
    if (building != default)
    {
        building.CalculateLoad(buildingRoomTypes);
        await db.SaveChangesAsync();
    }
});

app.MapPost("/building-operational-carbon", async (BuildingOperationalCarbon input, BackendDbContext db) =>
{
    var buildingOperationalCarbon = await db.BuildingOperationalCarbons.FirstOrDefaultAsync(i => i.BuildingId == input.BuildingId);
    
    if (buildingOperationalCarbon == default)
    {
        await db.BuildingOperationalCarbons.AddAsync(input);
    }
    else
    {
        buildingOperationalCarbon.ElectricityCarbonIntensity = input.ElectricityCarbonIntensity;
        buildingOperationalCarbon.ElectricityEnergySourcePercentage = input.ElectricityEnergySourcePercentage;
        buildingOperationalCarbon.NaturalGasCarbonIntensity = input.NaturalGasCarbonIntensity;
        buildingOperationalCarbon.NaturalGasEnergySourcePercentage = input.NaturalGasEnergySourcePercentage;
        buildingOperationalCarbon.OtherEnergySourceCarbonIntensity = input.OtherEnergySourceCarbonIntensity;
        buildingOperationalCarbon.OtherEnergySourcePercentage = input.OtherEnergySourcePercentage;
    }
    
    await db.SaveChangesAsync();

});

app.MapPost("/building-materials", async (Guid buildingId, List<MaterialInput> inputs, BackendDbContext db) =>
{
    var existing = db.Materials.Where(i => i.BuildingId == buildingId);
    db.Materials.RemoveRange(existing);
    
    var materials = inputs.Select(input => new Material(buildingId, input)).ToList();
    await db.Materials.AddRangeAsync(materials);
    await db.SaveChangesAsync();
    
    /*
    var materialIds = materials.Select(i => i.Id).ToHashSet();
    foreach (var input in inputs)
    {
        if (input.Id.HasValue && materialIds.Contains(input.Id.Value))
        {
            var material = await materials.FirstAsync(i => i.Id == input.Id.Value);
            material.Category = input.Category;
            material.SubCategory = input.SubCategory;
            material.Name = input.Name;
            material.Quantity = input.Quantity;
            material.Unit = input.Unit;
            material.BaselineEpd = input.BaselineEpd;
            material.AchievableEpd = input.AchievableEpd;
            material.RealizedEpd = input.RealizedEpd;

            db.Materials.Update(material);
        }
        else
        {
            var newMaterial = new Material(buildingId, input);
            await db.Materials.AddAsync(newMaterial);
        }
    }

    await db.SaveChangesAsync();*/
});

#endregion

app.Run();
