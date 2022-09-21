using AEIRevitDesignAutomation.Common;
using AEIRevitDesignAutomation.Models;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using DesignAutomationFramework;
using System;
using System.Linq;
using Autodesk.Revit.DB.Plumbing;

namespace AEIRevitDesignAutomation.Operations
{
    internal static class MechanicalOperation
    {
        internal static void Run(DesignAutomationData data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            var doc = data.RevitDoc ?? throw new InvalidOperationException("Could not open document.");

            var ducts = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(MEPCurve))
                .OfCategory(BuiltInCategory.OST_DuctCurves)
                .ToElements();

            var ductSurfaceArea = ducts.Select(o =>
                {
                    var parameters = o.ParametersMap;
                    if (!parameters.Contains("Area")) return 0D;
                    var area = parameters.get_Item("Area");
                    return area.HasValue ? area.AsDouble() : 0D;
                })
                .Sum();

            var pipes = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(MEPCurve))
                .OfCategory(BuiltInCategory.OST_PipeCurves)
                .ToElements();

            var totalPipeLength = pipes.Select(o =>
                {
                    var length = o.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                    return length.HasValue ? length.AsDouble() : 0D;
                })
                .Sum();

            var result = new MechanicalResponse
            {
                DuctSurfaceArea = ductSurfaceArea,
                TotalPipeLength = totalPipeLength
            };

            Helpers.SaveResultAsJson(result);
        }
    }
}
