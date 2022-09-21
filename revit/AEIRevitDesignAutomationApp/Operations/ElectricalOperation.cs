using AEIRevitDesignAutomation.Common;
using AEIRevitDesignAutomation.Models;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using System;
using System.Linq;
using Autodesk.Revit.DB.Electrical;

namespace AEIRevitDesignAutomation.Operations
{
    internal static class ElectricalOperation
    {
        internal static void Run(DesignAutomationData data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            var doc = data.RevitDoc ?? throw new InvalidOperationException("Could not open document.");

            var circuits = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(ElectricalSystem))
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .ToElements();

            var numberOfCircuits = circuits.Count();

            var numberOfLightingFixtures = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_LightingFixtures)
                .Count();

            var result = new ElectricalResponse
            {
                NumberOfCircuits = numberOfCircuits,
                NumberOfLightingFixtures = numberOfLightingFixtures
            };

            Helpers.SaveResultAsJson(result);
        }
    }
}
