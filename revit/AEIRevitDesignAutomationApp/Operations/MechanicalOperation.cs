﻿using AEIRevitDesignAutomation.Common;
using AEIRevitDesignAutomation.Models;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using DesignAutomationFramework;
using System;
using System.Linq;

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
                .OfClass(typeof(Duct))
                .Cast<Duct>();

            var ductSurfaceArea = ducts.Select(o =>
                {
                    var parameters = o.ParametersMap;
                    return parameters.Contains("Area") ? parameters.get_Item("Area").AsDouble() : 0D;
                })
                .Sum();

            var result = new MechanicalResponse
            {
                DuctSurfaceArea = ductSurfaceArea
            };

            Helpers.SaveResultAsJson(result);
        }
    }
}