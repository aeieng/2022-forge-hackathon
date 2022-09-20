namespace Backend.Entities;

public class RoomType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double VentilationRate { get; set; }
    public double PeopleDensity { get; set; }
    public double PeopleSensibleRate { get; set; }
    public double PeopleLatentRate { get; set; }
    public double LightingPowerDensity { get; set; }
    public double EquipmentPowerDensity { get; set; }
}