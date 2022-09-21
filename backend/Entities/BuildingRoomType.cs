using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class BuildingRoomType
{
    public Guid Id { get; set; }
    public Guid BuildingId { get; set; }
    public Guid RoomTypeId { get; set; }
    public double Percentage { get; set; }
    public double? Area { get; set; }
    
    public BuildingRoomType() {}

    public BuildingRoomType(Guid buildingId, BuildingRoomTypeInput buildingRoomTypeIput)
    {
        Id = Guid.NewGuid();
        BuildingId = buildingId;
        RoomTypeId = buildingRoomTypeIput.RoomTypeId;
        Percentage = buildingRoomTypeIput.Percentage;
    }
}

public class BuildingRoomTypeInput
{
    public Guid? Id { get; set; }
    public Guid RoomTypeId { get; set; }
    public double Percentage { get; set; }
}