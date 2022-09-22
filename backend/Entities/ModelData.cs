using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Owned]
public class ModelData
{
    //[ForeignKey("Model")]
    //public Guid ModelId { get; set; }
    //public virtual Model Model { get; set; }

    // Architectural
    public double ExteriorWallArea { get; set; }
    public double GlazingArea { get; set; }
    /* Rooms data is 1:many directly on Model */

    // Electrical
    public int NumberOfCircuits { get; set; }
    public int NumberOfLightingFixtures { get; set; }


    // Mechanical
    public double DuctSurfaceArea { get; set; }
    public double TotalPipeLength { get; set; }
}