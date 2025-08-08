using System;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitCommand25
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elem)
        {
            return Result.Suceeded;
        }
    }
}
