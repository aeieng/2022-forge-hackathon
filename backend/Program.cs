using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services
    .AddDbContext<BackendDbContext>
        (o => { o.UseNpgsql(configuration["ConnectionStrings:Postgres"]);});

var app = builder.Build();

app.MapGet("/models", (BackendDbContext context) => context.Models);
app.MapPost("/models-post", () => { });

app.Run();
