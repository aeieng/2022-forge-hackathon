using System.Collections.Generic;


namespace Backend.Entities
{
    internal class RoomData
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public int ElementId { get; set; }
        public double Area { get; set; }
    }

    internal class ArchitecturalResponse
    {
        public List<RoomData> Rooms { get; set; }
        public double ExteriorWallArea { get; set; }
        public double GlazingArea { get; set; }
    }
}
