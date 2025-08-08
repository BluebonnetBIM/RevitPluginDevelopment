using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace Navis_Exporter
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elem)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // Define the view name to export
            if (doc.Title == "")
            {
                // Define the View name were looking for as a variable
                string viewName = "";

                // Find the 3D view
                View3D view3D = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Cast<View3D>().FirstOrDefault(v => !v.IsTemplate && v.Name.Equals(viewName, StringComparison.OrdinalIgnoreCase));

                // Define export path
                string userProfile = Environment.GetEnvironmentVariable("USERPROFILE");
                string exportPath = @$"{userProfile}\Desktop";

                // Ensure direcotry exists
                string dir = Path.GetDirectoryName(exportPath);
                if (!Directory.Exists(dir))
                {
                    message = $"Export failed due to directory {dir} not existing.";
                    return Result.Failed;
                }

                try
                {
                    // Set export options
                    NavisworksExportOptions nwcOptions = new NavisworksExportOptions { ExportLinks = true, ExportScope = NavisworksExportScope.View, ViewId = view3D.Id };

                    // Export the 3D view
                    doc.Export(dir, viewName, nwcOptions);

                    // Show task dialog for export successS
                    TaskDialog.Show("Export", $"3D view exported to:\n{exportPath}");

                    return Result.Succeeded;
                }

                catch (Exception ex)
                {
                    message = $"Error during export: {ex.Message}";
                    return Result.Failed;
                }
            }

            else
            {
                message = "Frost Arboretum is not the active Document.";
                return Result.Failed;
            }
        }
    }
}
