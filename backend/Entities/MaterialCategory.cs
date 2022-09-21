using System.Text.Json.Serialization;

namespace Backend.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MaterialCategory
{
    Enclosure,
    Interiors,
    Mechanical,
    Electrical,
    Site,
    Structure
}