namespace Backend.Entities;

public class ExtractionLog
{
    public Guid Id { get; set; }
    public DateTime StartedRunAtUtc { get; set; }
    public Guid ModelId { get; set; }
    public string Operation { get; set; }
    public string Status { get; set; }
    public string ResultSignedUrl { get; set; }
    public string? DesignAutomationWorkItemId { get; set; }
    public string? DesignAutomationLog { get; set; }
}

