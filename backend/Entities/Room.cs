using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

public class Room
{
    public string Id { get; set; }
    [ForeignKey("Model")]
    public Guid ModelId { get; set; }
    public virtual Model Model { get; set; }
    public string Number { get; set; }
    public string Name { get; set; }
    public double FloorArea { get; set; }
    public double ExteriorWallArea { get; set; }
    public double ExteriorWindowArea { get; set; }
}
