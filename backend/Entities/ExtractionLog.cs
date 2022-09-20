namespace Backend.Entities;

public class ExtractionLog
{
    public Guid Id { get; set; }
    public DateTime StartedRunAtUtc { get; set; }
    public List<Guid> ModelIds { get; set; }
    public string Status { get; set; }
}

