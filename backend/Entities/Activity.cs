using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

public class Activity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    [Column(TypeName = "varchar(32)")]
    public ModelType ModelType { get; set; }
    public string AppBundleId { get; set; }
    [Column(TypeName = "varchar(32)")]
    public ActivityType ActivityType { get; set; }
}

public class ActivityInput
{
    public List<Guid> ActivityIds { get; set; }
}