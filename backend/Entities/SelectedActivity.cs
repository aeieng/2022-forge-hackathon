using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class SelectedActivity
{
    [Key]
    public Guid ActivityId { get; set; }
}

