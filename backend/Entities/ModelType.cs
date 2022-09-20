using System.Text.Json.Serialization;

namespace Backend.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModelType
{
    Architectural,
    Structural,
    Mechanical,
    Electrical,
    Piping
}