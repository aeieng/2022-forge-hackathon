using System.Text.Json.Serialization;

namespace Backend.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MaterialSubCategory
{
    BasementConstruction,
    Foundations,
    ExteriorEnclosure,
    SuperStructure,
    Roofing,
    InteriorConstruction,
    Mechanical,
    Electrical
}