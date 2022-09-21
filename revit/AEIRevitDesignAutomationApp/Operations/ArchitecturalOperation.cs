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

            var insertFamilyInstances = new FilteredElementCollector(doc, windowElementIds)
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>();

            var windowFamilyInstances = insertFamilyInstances
                .Where(o => o.Category.Name.IndexOf("window", StringComparison.InvariantCultureIgnoreCase) >= 0);

            // Get geometry instances from the window elements, inspect faces, and get
            // the one with largest surface area per solid, which will be the window area,
            // then sum them all.
            var geoElementsPerWindow = windowFamilyInstances
                .Select(windowFamilyInstance => windowFamilyInstance
                    .get_Geometry(new Options())
                    .Select(o => o as GeometryInstance)
                    .Where(o => o != null)
                    .Select(o => o.GetInstanceGeometry()));

            var glazingArea = 0D;
            foreach (var geoElements in geoElementsPerWindow)
            {
                var solids = geoElements
                    .SelectMany(o => o.Select(p => p as Solid))
                    .Where(o => o != null && !o.Faces.IsEmpty);

                var orderedFaces = solids
                    .SelectMany(o => o.Faces.Cast<Face>())
                    .OrderByDescending(o => o.Area);

                var largestFaceArea = orderedFaces.FirstOrDefault()?.Area ?? 0D;

                glazingArea += largestFaceArea;
            }

            // Get Room data
            var rooms = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(SpatialElement))
                .OfCategory(BuiltInCategory.OST_Rooms)
                .Where(o => o is Room room && room.Area > 0D)
                .Cast<Room>()
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
