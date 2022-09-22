using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class BuildingRoomType
{
    public Guid Id { get; set; }
    public Guid BuildingId { get; set; }
    
    public string RoomTypeName { get; set; }
    public double Percentage { get; set; }
    
    public double PeopleDensity { get; set; }
    
    public double LightingDensity { get; set; }
    
    public double EquipmentDensity { get; set; }

    public BuildingRoomType()
    {
    }

    public BuildingRoomType(Guid buildingId, BuildingRoomTypeInput input)
    {
        Id = input.Id;
        BuildingId = buildingId;
        RoomTypeName = input.RoomTypeName;
        Percentage = input.Percentage;
        PeopleDensity = input.PeopleDensity;
        LightingDensity = input.LightingDensity;
        EquipmentDensity = input.EquipmentDensity;
    }
}

public class BuildingRoomTypeInput
{
    public Guid Id { get; set; }
    public string RoomTypeName { get; set; }
    public int Percentage { get; set; }
    public int PeopleDensity { get; set; }
    public int LightingDensity { get; set; }
    public int EquipmentDensity { get; set; }
}