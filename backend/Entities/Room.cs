namespace Backend.Entities;

public class Room
{
    public string Id { get; set; }
    public string ModelId { get; set; }
    public string Number { get; set; }
    public string Name { get; set; }
    public double FloorArea { get; set; }
    public double ExteriorWallArea { get; set; }
    public double ExteriorWindowArea { get; set; }
}

