using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Entities;

public class Model
{
    public Guid Id { get; set; }
    public string AutodeskItemId { get; set; }
    public string Name { get; set; }
    [Column(TypeName = "varchar(32)")]
    public ModelType Type { get; set; }
    public string RevitVersion { get; set; }
    public string AutodeskHubId { get; set; }
    public string AutodeskProjectId { get; set; }
    public string DerivativeId { get; set; }
    [ForeignKey("Building")]
    public Guid BuildingId { get; set; }
    public ModelData ModelData { get; set; }
    public virtual Building Building { get; set; }
    public virtual List<Room> Rooms { get; set; }

    public Model() { }

    public Model(ModelInput input)
    {
        Id = Guid.NewGuid();
        BuildingId = input.BuildingId;
        AutodeskItemId = input.AutodeskItemId;
        AutodeskProjectId = input.AutodeskProjectId;
        AutodeskHubId = input.AutodeskHubId;
        DerivativeId = input.DerivativeId;
        Name = input.Name;
        Type = input.Type;
        RevitVersion = input.RevitVersion;
    }
}

public class ModelInput
{
    public string AutodeskItemId { get; set; }
    public string AutodeskProjectId { get; set; }
    public string AutodeskHubId { get; set; }
    public string DerivativeId { get; set; }
    public Guid BuildingId { get; set; }
    public string Name { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ModelType Type { get; set; }
    public string RevitVersion { get; set; }
}
