using AEIRevitDesignAutomation.Common;
using AEIRevitDesignAutomation.Models;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AEIRevitDesignAutomation.Operations
{
    internal static class ArchitecturalOperation
    {
        internal static void Run(DesignAutomationData data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            var doc = data.RevitDoc ?? throw new InvalidOperationException("Could not open document.");

            var exteriorWalls = GetExteriorWalls(doc);
            var grossExteriorWallArea = exteriorWalls.Sum(GetGrossWallArea);

            var windowElementIds = exteriorWalls
                .Select(o => o.FindInserts(true, false, true, true))
                .SelectMany(o => o)
                .ToHashSet();

            var windowElements = new FilteredElementCollector(doc, windowElementIds)
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .Where(o => o.Category.Name.IndexOf("Window", StringComparison.InvariantCultureIgnoreCase) >= 0);

            var glazingArea = 0D;
            foreach (var windowElement in windowElements)
            {
                var solids = windowElement
                    .get_Geometry(new Options())
                    .Select(o => o as GeometryInstance)
                    .Where(o => o != null)
                    .Select(o => o.GetInstanceGeometry())
                    .SelectMany(o => o.Select(p => p as Solid))
                    .Where(o => o != null && !o.Faces.IsEmpty)
                    .ToList();

                // Inspecting window as solids will yield inside and outside surfaces along with trim. To avoid
                // double-counting the main surface area, sort to pick the largest face and use only its area.
                var largestFaceArea = solids
                    .Select(o => o.Faces.Cast<Face>().OrderByDescending(p => p.Area).FirstOrDefault()?.Area ?? 0D)
                    .OrderByDescending(o => o)
                    .First();

                glazingArea += largestFaceArea;
            }
            
            var result = new ArchitecturalResponse(new List<RoomData>(), grossExteriorWallArea, glazingArea);
            Helpers.SaveResultAsJson(result);
        }

        private static IEnumerable<Wall> GetExteriorWalls(Document doc) =>
            new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .Where(o => o?.WallType.Function == WallFunction.Exterior);

        internal static double GetGrossWallArea(Wall wall) =>
            wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();

        //internal static double GetWallWindowArea(Wall wall)
        //{
        //    var inserts = wall.FindInserts()
        //    return 0D;
        //}
    }

}
