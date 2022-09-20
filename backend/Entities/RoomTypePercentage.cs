using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class RoomTypePercentage
{
    [Key]
    public Guid RoomId { get; set; }
    public Guid RoomTypeId { get; set; }
    public double Percentage { get; set; }
}