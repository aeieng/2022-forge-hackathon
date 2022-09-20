using System.Text.Json.Serialization;

namespace Backend.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActivityType
{
    ModelDerivative,
    DesignAutomation
}