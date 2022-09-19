using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using DesignAutomationFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AEIRevitDesignAutomation
{
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
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

                var operation = inputParams?.Operation;
                switch (operation)
                {
                    case "spaceNames":
                        GetAllSpaceDisplayNames(e.DesignAutomationData);
                        e.Succeeded = true;
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid operation specified: {operation}");
                };
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                e.Succeeded = false;
            }
        }

        public static void GetAllSpaceDisplayNames(DesignAutomationData data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            var doc = data.RevitDoc ?? throw new InvalidOperationException("Could not open document.");

            //var rvtApp = data.RevitApp ?? throw new InvalidDataException(nameof(data.RevitApp));
            //var modelPath = data.FilePath;
            //if (String.IsNullOrWhiteSpace(modelPath)) throw new InvalidDataException(nameof(modelPath));

            IEnumerable<Space> spaces = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(SpatialElement))
                .OfCategory(BuiltInCategory.OST_MEPSpaces)
                .Where(e => e.GetType() == typeof(Space) && ((Space)e).Area > 0)
                .Cast<Space>()
                .OrderBy(e => e.Number);

            var spaceDisplayNames = spaces.Select(e => $"{e.Number} {e.Name}").ToList();
            var json = JsonSerializer.Serialize(spaceDisplayNames, new JsonSerializerOptions { WriteIndented = true });

            // ReSharper disable once StringLiteralTypo
            const string path = ".\\result.json";
            File.WriteAllText(path, json);
        }
    }

    public class InputParams
    {
        public string Operation { get; set; }
    }
}
