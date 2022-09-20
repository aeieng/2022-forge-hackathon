namespace Backend.Entities;

public class Model
{
    public Guid Id { get; set; }
    public string AutodeskItemId { get; set; }
    public string Name { get; set; }
    public ModelType Type { get; set; }
    public string RevitVersion { get; set; }
    public string AutodeskHubId { get; set; }
    public string AutodeskProjectId { get; set; }

    public Model() { }

    public Model(ModelInput input)
    {
        Id = Guid.NewGuid();
        AutodeskItemId = input.AutodeskItemId;
        Name = input.Name;
        Type = input.Type;
        RevitVersion = input.RevitVersion;
        AutodeskHubId = input.AutodeskHubId;
        AutodeskProjectId = input.AutodeskProjectId;
    }
}

public class ModelInput
{
    public string AutodeskItemId { get; set; }
    public string Name { get; set; }
    public ModelType Type { get; set; }
    public string RevitVersion { get; set; }
    public string AutodeskHubId { get; set; }
    public string AutodeskProjectId { get; set; }
}
