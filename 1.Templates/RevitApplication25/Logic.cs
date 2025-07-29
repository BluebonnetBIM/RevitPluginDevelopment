using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitApplication25
{
    [Transaction(TransactionMode.ReadOnly)]
    public class HelloWorld : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elem)
        {
            return Result.Succeeded;
        }
    }
}