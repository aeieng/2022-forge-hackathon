namespace Backend.Entities;

public class Building
{
    public Guid Id { get; set; }
    public string ProjectNumber { get; set; }
    public string Name { get; set; }
    
    public double FloorArea { get; set; }
    public double Height { get; set; }
    public int NumberOfFloors { get; set; }
    public string? PrimaryBuildingType { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

    public Building()
    {
    }

    public Building(CreateBuildingInput input)
    {
        Id = Guid.NewGuid();
        ProjectNumber = input.ProjectNumber;
        Name = input.Name;
        
        /*
        if (!string.IsNullOrEmpty(input.PrimaryBuildingType))
        {
            PrimaryBuildingType = input.PrimaryBuildingType;
        }

        if (input.Latitude.HasValue)
        {
            Latitude = input.Latitude;
        }
        
        if (input.Longitude.HasValue)
        {
            Longitude = input.Longitude;
        } */
    }
}

public class CreateBuildingInput
{
    public string ProjectNumber { get; set; }
    public string Name { get; set; }
}