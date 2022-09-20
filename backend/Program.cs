using Microsoft.EntityFrameworkCore;
using Backend.Entities;

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

#endregion

app.Run();
