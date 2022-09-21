using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class BuildingEmbodiedCarbon
{
    [Key]
    public Guid BuildingId { get; set; }
    public double TotalEmbodiedCarbonUseIntensity { get; set; }
}