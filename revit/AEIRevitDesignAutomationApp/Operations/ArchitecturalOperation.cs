using AEIRevitDesignAutomation.Common;
using AEIRevitDesignAutomation.Models;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using DesignAutomationFramework;
using System;
using System.Linq;

namespace AEIRevitDesignAutomation.Operations
{
    internal static class ArchitecturalOperation
    {
        internal static void Run(DesignAutomationData data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            var doc = data.RevitDoc ?? throw new InvalidOperationException("Could not open document.");

            // Get Exterior walls
            var exteriorWalls = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .Where(o => o?.WallType.Function == WallFunction.Exterior)
                .ToList();

            // Compute total exterior wall surface area (gross)
            var grossExteriorWallArea = exteriorWalls.Sum(GetGrossWallArea);

            // Get Window elements
            var windowElementIds = exteriorWalls
                .Select(o => o.FindInserts(true, false, true, true))
                .SelectMany(o => o)
                .ToHashSet();

            var windowElements = new FilteredElementCollector(doc, windowElementIds)
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .Where(o => o.Category.Name.IndexOf("Window", StringComparison.InvariantCultureIgnoreCase) >= 0);

            // Get geometry instances from the window elements, inspect faces, and get
            // the one with largest surface area per solid, which will be the window area,
            // then sum them all.
            var glazingArea = windowElements
                .Select(windowElement => windowElement
                    .get_Geometry(new Options())
                    .Select(o => o as GeometryInstance)
                    .Where(o => o != null)
                    .Select(o => o.GetInstanceGeometry())
                    .SelectMany(o => o.Select(p => p as Solid))
                    .Where(o => o != null && !o.Faces.IsEmpty)
                    .Select(o => o.Faces
                        .Cast<Face>()
                        .OrderByDescending(p => p.Area)
                        .FirstOrDefault()?.Area ?? 0D)
                    .OrderByDescending(o => o)
                    .First())
                .Sum();

            // Get Room data
            var rooms = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Room))
                .OfCategory(BuiltInCategory.OST_Rooms)
                .Cast<Room>()
                .Where(o => o.Area > 0D)
                .OrderBy(o => o.Number);

            var roomData = rooms.Select(o => new RoomData
            {
                ElementId = o.Id.IntegerValue,
                Name = o.Name,
                Number = o.Number,
                Area = o.Area
            }).ToList();

            var result = new ArchitecturalResponse
            {
                Rooms = roomData,
                ExteriorWallArea = grossExteriorWallArea,
                GlazingArea = glazingArea
            };

            Helpers.SaveResultAsJson(result);
        }

        internal static double GetGrossWallArea(Wall wall) =>
            wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
    }

}
