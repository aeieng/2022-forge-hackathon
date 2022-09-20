namespace Backend.Entities;

public class Duct
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public double StraightDuctArea { get; set; }
    public double FittingArea { get; set; }
    public string SystemName { get; set; }
    public string MaterialType { get; set; }
}