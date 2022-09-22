namespace Backend.Entities;

public class Room
{
    public string Id { get; set; }
    public string BuildingId { get; set; }
    public string ElementId { get; set; }
    public string Number { get; set; }
    public string Name { get; set; }
    public double Area { get; set; }
}

