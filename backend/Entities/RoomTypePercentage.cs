namespace Backend.Entities;

public class RoomTypePercentage
{
    public Guid RoomId { get; set; }
    public Guid RoomTypeId { get; set; }
    public double Percentage { get; set; }
}