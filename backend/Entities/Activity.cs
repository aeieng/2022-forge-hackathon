namespace Backend.Entities;

public class Activity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ModelType ModelType { get; set; }
    public string AppBundleId { get; set; }
    public ActivityType ActivityType { get; set; }
}
