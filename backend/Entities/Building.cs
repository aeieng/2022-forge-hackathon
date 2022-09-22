namespace Backend.Entities;

public class Building
{
    public Guid Id { get; set; }
    public string ProjectNumber { get; set; }
    public string Name { get; set; }
    
    public double? FloorArea { get; set; }
    public double? Height { get; set; }
    public int? NumberOfFloors { get; set; }
    public string? PrimaryBuildingType { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    
    public double? LightingCoolingLoad { get; set; }
    public double? EquipmentCoolingLoad { get; set; }
    public double? PeopleCoolingLoad { get; set; }

    public Building()
    {
    }

    public void CalculateLoad(List<BuildingRoomType> roomTypes)
    {
        LightingCoolingLoad = 0;
        EquipmentCoolingLoad = 0;
        PeopleCoolingLoad = 0;
        
        foreach (var roomType in roomTypes)
        {
            LightingCoolingLoad += roomType.LightingDensity * (FloorArea ?? 0D) * (roomType.Percentage/100) * 3.413;
            EquipmentCoolingLoad += roomType.EquipmentDensity * (FloorArea ?? 0D) * (roomType.Percentage/100) * 3.413;
            PeopleCoolingLoad += (FloorArea ?? 0D) * roomType.PeopleDensity / 1000 * 250 * (roomType.Percentage/100);
        }
    }

    public Building(CreateBuildingInput input)
    {
        Id = Guid.NewGuid();
        ProjectNumber = input.ProjectNumber;
        Name = input.Name;
    }
}

public class CreateBuildingInput
{
    public string ProjectNumber { get; set; }
    public string Name { get; set; }
}