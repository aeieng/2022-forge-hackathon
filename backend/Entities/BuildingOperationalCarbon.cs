using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class BuildingOperationalCarbon
{
    [Key]
    public Guid BuildingId { get; set; }
    public double? OperationalEnergyUseIntensity { get; set; } // kBtu/ ft^2 / yr
    public double? NaturalGasCarbonIntensity { get; set; } // kg CO2e / MBtu
    public double? NaturalGasEnergySourcePercentage { get; set; } 
    public double? ElectricityCarbonIntensity { get; set; } // lb CO2e / MWh
    public double? ElectricityEnergySourcePercentage { get; set; }
    public double? OtherEnergySourceCarbonIntensity { get; set; } // kg CO2e / kWh
    public double? OtherEnergySourcePercentage { get; set; }
    
    public double? TotalOperatingCarbonUseIntensity { get; set; }
}