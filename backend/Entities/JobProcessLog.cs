namespace Backend.Entities;

public class JobProcessLog
{
    public Guid Id { get; set; }
    public DateTime LastRun { get; set; }
    public string Status { get; set; }
}

