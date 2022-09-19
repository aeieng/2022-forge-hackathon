using Microsoft.EntityFrameworkCore;
using Backend.Entities;

public class BackendDbContext : DbContext
{
    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options) { }

    public DbSet<Model> Models { get; set; } = default!;
    public DbSet<Space> Spaces { get; set; } = default!;
    public DbSet<Activity> Activities { get; set; } = default!;
    public DbSet<JobProcessLog> JobProcessLogs { get; set; } = default!;
    public DbSet<ModelMeta> ModelMetas { get; set; } = default!;    
}