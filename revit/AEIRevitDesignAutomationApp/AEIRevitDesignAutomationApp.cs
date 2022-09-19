using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AEIRevitDesignAutomation
{
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    // ReSharper disable once InconsistentNaming
    public class AEIRevitDesignAutomationApp : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnStartup(ControlledApplication app)
        {
            DesignAutomationBridge.DesignAutomationReadyEvent += HandleDesignAutomationReadyEvent;
            return ExternalDBApplicationResult.Succeeded;
        }

        public ExternalDBApplicationResult OnShutdown(ControlledApplication app)
        {
            return ExternalDBApplicationResult.Succeeded;
        }

        public void HandleDesignAutomationReadyEvent(object sender, DesignAutomationReadyEventArgs e)
        {
            try
            {
                var inputParamsJson = File.ReadAllText(".\\params.json");
                var inputParams = JsonSerializer.Deserialize<InputParams>(inputParamsJson);
                var data = e.DesignAutomationData;

                var operation = inputParams?.Operation;
                switch (operation)
                {
                    case "exteriorWallArea":
                        ExteriorWallArea(data);
                        e.Succeeded = true;
                        break;

                    default:
                        ErrorOperation($"Invalid operation specified: {operation}");
                        e.Succeeded = false;
                        break;
                }
            }
            catch (Exception exception)
            {
                ErrorOperation($"Exception occurred: {exception}");
                e.Succeeded = false;
            }
        }

        public static void ErrorOperation(string error)
        {
            dynamic dResult = new ExpandoObject();
            dResult.Error = error;

            SaveResultAsJson(dResult);
        }

        public static void ExteriorWallArea(DesignAutomationData data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            var doc = data.RevitDoc ?? throw new InvalidOperationException("Could not open document.");

            var exteriorWalls = GetExteriorWalls(doc);
            var grossWallArea = exteriorWalls.Sum(GetGrossWallArea);
            var grossWallArea2 = exteriorWalls.Sum(GetGrossWallArea2);

            dynamic dResult = new ExpandoObject();
            dResult.ExteriorWallArea = grossWallArea;

            SaveResultAsJson(dResult);
        }




        private static void SaveResultAsJson(dynamic dResult)
        {
            var json = JsonSerializer.Serialize(dResult, new JsonSerializerOptions { WriteIndented = true });

            // ReSharper disable once StringLiteralTypo
            const string path = ".\\result.json";
            File.WriteAllText(path, json);
        }

        private static IEnumerable<Wall> GetExteriorWalls(Document doc) =>
            new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .Where(o => o?.WallType.Function == WallFunction.Exterior);

        public static double GetGrossWallArea(Wall wall)
        {
            return wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
        }

        public static double GetGrossWallArea2(Wall wall)
        {
            var wallGeometry = wall.get_Geometry(new Options());
            Solid wallSolid = null;

            foreach (var geomObject in wallGeometry)
            {
                // Walls and some columns will have a solid directly in its geometry
                if (geomObject is Solid solid1 && solid1.Volume > 0D)
                {
                    wallSolid = solid1;
                    break;
                }

                // You can obtain wall solid with method in the top but some other elements need this type conversion
                if (!(geomObject is GeometryInstance geomInst)) continue;

                // Instance geometry is obtained so that the intersection works as
                // expected without requiring transformation
                var instElem = geomInst.GetInstanceGeometry();

                // There is usually only one solid but for safety, select the one with largest volume
                var solid2 = instElem
                    .Where(o => o is Solid)
                    .Cast<Solid>()
                    .Where(o => o.Volume > 0D)
                    .OrderByDescending(o => o.Volume)
                    .FirstOrDefault();

                if (solid2 == null) continue;

                wallSolid = solid2;
                break;
            }

            return wallSolid == null ? 0D : wallSolid.Faces.Cast<Face>().Sum(face => face.Area);
        }
    }

    public class InputParams
    {
        public string Operation { get; set; }
    }
}
