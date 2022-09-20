using AEIRevitDesignAutomation.Common;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using DesignAutomationFramework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace AEIRevitDesignAutomation.Operations
{
    internal static class MechanicalOperation
    {
        internal static void Run(DesignAutomationData data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            var doc = data.RevitDoc ?? throw new InvalidOperationException("Could not open document.");

            var ducts = GetDucts(doc);
            var totalArea = 0D;
            foreach (var duct in ducts)
            {
                var parameters = duct.ParametersMap;
                var areaParam = parameters.get_Item("Area");
                totalArea += areaParam.AsDouble();
            }

            dynamic result = new ExpandoObject();
            // TODO
            result.DuctSurfaceArea = totalArea;

            Helpers.SaveResultAsJson(result);
        }



        private static IEnumerable<Duct> GetDucts(Document doc) =>
            new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Duct))
                .Cast<Duct>();

    }

}
