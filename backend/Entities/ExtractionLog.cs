namespace Backend.Entities;

public class ExtractionLog
{
    public Guid Id { get; set; }
    public DateTime LastRun { get; set; }
    public Guid ModelId { get; set; }
    public string Status { get; set; }
}

