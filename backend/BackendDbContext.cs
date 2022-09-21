using Microsoft.EntityFrameworkCore;
using Backend.Entities;

public class BackendDbContext : DbContext
{
    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options) { }

    public DbSet<Model> Models { get; set; } = default!;
    public DbSet<Building> Buildings { get; set; } = default!;
    public DbSet<BuildingCost> BuildingCosts { get; set; } = default!;
    public DbSet<BuildingRoomType> BuildingRoomTypes { get; set; } = default!;
    public DbSet<BuildingOperationalCarbon> BuildingOperationalCarbons { get; set; }
    public DbSet<BuildingEmbodiedCarbon> BuildingEmbodiedCarbons { get; set; }
    
    public DbSet<Room> Rooms { get; set; } = default!;
    
    public DbSet<RoomType> RoomTypes { get; set; } = default!;
    public DbSet<Activity> Activities { get; set; } = default!;
    public DbSet<ExtractionLog> ExtractionLog { get; set; } = default!;
    public DbSet<SelectedActivity> SelectedActivities { get; set; } = default!;

    public DbSet<Material> Materials { get; set; } = default!;
    
}