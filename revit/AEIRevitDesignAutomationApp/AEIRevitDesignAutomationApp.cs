using AEIRevitDesignAutomation.Models;
using AEIRevitDesignAutomation.Operations;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using System;
using System.Dynamic;
using System.IO;
using System.Text.Json;
using Autodesk.Revit.Attributes;

namespace AEIRevitDesignAutomation
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
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
                    case "Architectural":
                        ArchitecturalOperation.Run(data);
                        e.Succeeded = true;
                        break;

                    case "Mechanical":
                        MechanicalOperation.Run(data);
                        e.Succeeded = true;
                        break;

                    case "Electrical":
                        ElectricalOperation.Run(data);
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

            Common.Helpers.SaveResultAsJson(dResult);
        }
    }


}
