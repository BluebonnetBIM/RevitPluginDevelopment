using System;
using Autodesk.Revit.UI;

namespace RevitApplication25
{
    public class Constructor : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}
