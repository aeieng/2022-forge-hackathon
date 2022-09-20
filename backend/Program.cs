using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using Autodesk.Forge;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

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
    db.Models.Add(new Model(input));
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

app.MapGet("/activities", async (BackendDbContext db) => await db.Activities.ToListAsync());
app.MapGet("/selected-activities", async (BackendDbContext db) => await db.Activities.ToListAsync());
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

    // Download Revit file from ACC 
    // Use data management bucket created by Eric upload model {Guid}.rvt,  inputParams : { "Operation": "Mechanical" } Rvt Version: 2022
    // Queue up design automation
    // Create web hooks or poll if runing out of time
    // Register call backs

    await db.ExtractionLog.AddAsync(extractionLog);
});

#endregion

#region Buildings

app.MapGet("/buildings", async (BackendDbContext db) => await db.Buildings.ToListAsync());


#endregion

app.Run();
