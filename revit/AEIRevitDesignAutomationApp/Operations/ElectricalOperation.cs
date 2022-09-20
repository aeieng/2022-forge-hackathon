using AEIRevitDesignAutomation.Common;
using AEIRevitDesignAutomation.Models;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using DesignAutomationFramework;
using System;
using System.Linq;

namespace AEIRevitDesignAutomation.Operations
{
    internal static class ElectricalOperation
    {
        internal static void Run(DesignAutomationData data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            var doc = data.RevitDoc ?? throw new InvalidOperationException("Could not open document.");

            var numberOfCircuits = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(ElectricalSystem))
                .Count();

            var numberOfLightingFixtures = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(LightingFixture))
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
