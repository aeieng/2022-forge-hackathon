using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class BuildingCost
{
    [Key]
    public Guid BuildingId { get; set; }
    public double ArchitecturalCost { get; set; }
    public double StructuralCost { get; set; }
    public double MechanicalCost { get; set; }
    public double PipingCost { get; set; }
    public double ElectricalCost { get; set; }
}