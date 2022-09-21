# AEIRevitDesignAutomationApp

[![.net](https://img.shields.io/badge/.net-4.7|4.8-green.svg)](http://www.microsoft.com/en-us/download/details.aspx?id=30653)
[![Design Automation](https://img.shields.io/badge/Design%20Automation-v3-green.svg)](https://forge.autodesk.com/en/docs/design-automation/v3/developers_guide/overview/)
[![visual studio](https://img.shields.io/badge/Visual%20Studio-2017|2019-green.svg)](https://www.visualstudio.com/)
[![revit](https://img.shields.io/badge/revit-2018|2019|2020|2021-red.svg)](https://www.autodesk.com/products/revit/overview/)

## Description

AEIRevitDesignAutomationApp is an application that takes in a rvt file and params.json (which determinies Operation mode), and result.json with model metrics.

## Dependencies

This project was built in Visual Studio 2022. Download it [here](https://www.visualstudio.com/).

This sample references Nuget pakcages [Revit_All_Main_Versions_API_x64](https://www.nuget.org/packages/Revit_All_Main_Versions_API_x64) and [DesignAutomationBridge](https://www.nuget.org/packages/Autodesk.Forge.DesignAutomation.Revit) for Revit 2022.

## Build AEIRevitDesignAutomationApp.sln

Clone this repository and open `AEIRevitDesignAutomation.sln` in Visual Studio 2022.

Restore all Nuget packages.

Build `AEIRevitDesignAutomation.sln` in `Release 2022` or `Debug 2022` configuration.

## Run Design Automation Plugin Locally

1. Clone the other repository and find the corresponding version of test handler (such as DesignAutomationHandler2021)
   https://github.com/Autodesk-Forge/design.automation-csharp-revit.local.debug.tool
2. Build the test handler project.
3. Start to debug `AEIRevitDesignAutomation.sln`
4. Under the Add-Ins ribbon tab, click the **External Tools** drop-down, then click **DesignAutomationHandler**. It will start to run **AEIRevitDesignAutomationApp** plugin. Set a breakpoint to debug.
