namespace Backend.Entities;

public class Building
{
    // General
    public Guid Id { get; set; }
    public string ProjectNumber { get; set; }
    public string Name { get; set; }
    public string PrimaryBuildingType { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    // Geometry
    public double ExtractedFloorArea { get; set; }
    public double? FloorAreaOverride { get; set; }
    public double ExtractedExteriorWallArea { get; set; }
    public double? ExteriorWallAreaOverride { get; set; }
    public double ExtractedExteriorGlazingArea { get; set; }
    public double? ExteriorGlazingAreaOverride { get; set; }
    public int NumberOfFloors { get; set; }
    public double Height { get; set; }
    
    // Program
    public List<Model> Models { get; set; }
    public List<Room> Rooms { get; set; }
    public List<RoomTypePercentage> RoomTypePercentages { get; set; }
    
    // Envelope
    public double ExteriorWallUValue { get; set; }
    public double GlazingUValue { get; set; }
    public double GlazingSolarHeatGainCoefficient { get; set; }
    public double InfiltrationRate { get; set; }

    // Operational Carbon
    public double OperationalEnergyUseIntensity { get; set; } // kBtu/ ft^2 / yr
    public double NaturalGasCarbonIntensity { get; set; } // kg CO2e / MBtu
    public double NaturalGasEnergySourcePercentage { get; set; } 
    public double ElectricityCarbonIntensity { get; set; } // lb CO2e / MWh
    public double ElectricityEnergySourcePercentage { get; set; }
    public double OtherEnergySourceCarbonIntensity { get; set; } // kg CO2e / kWh
    public double OtherEnergySourcePercentage { get; set; }

    
    // Embodied

    // Compute Operational Carbon
    // Compute Embodied Carbon
    // Compute Loads

}