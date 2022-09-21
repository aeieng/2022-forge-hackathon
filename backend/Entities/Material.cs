using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

public class Material
{
    public Guid Id { get; set; }
    public Guid BuildingId { get; set; }
    [Column(TypeName = "varchar(32)")]
    public MaterialCategory Category { get; set; }
    [Column(TypeName = "varchar(32)")]
    public MaterialSubCategory SubCategory { get; set; }
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; }
    public double BaselineEpd { get; set; }
    public double AchievableEpd { get; set; }
    public double RealizedEpd { get; set; }

    public Material()
    {
    }

    public Material(Guid buildingId, MaterialInput materialInput)
    {
        Id = Guid.NewGuid();
        BuildingId = buildingId;
        Category = materialInput.Category;
        SubCategory = materialInput.SubCategory;
        Name = materialInput.Name;
        Quantity = materialInput.Quantity;
        Unit = materialInput.Unit;
        BaselineEpd = materialInput.BaselineEpd;
        AchievableEpd = materialInput.AchievableEpd;
        RealizedEpd = materialInput.RealizedEpd;
    }
}

public class MaterialInput
{
    public Guid? Id { get; set; }
    [Column(TypeName = "varchar(32)")]
    public MaterialCategory Category { get; set; }
    [Column(TypeName = "varchar(32)")]
    public MaterialSubCategory SubCategory { get; set; }
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; }
    public double BaselineEpd { get; set; }
    public double AchievableEpd { get; set; }
    public double RealizedEpd { get; set; }
}