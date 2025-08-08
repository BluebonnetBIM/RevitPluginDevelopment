using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using System.Text;
using Autodesk.Revit.UI.Events;

namespace Assign_Worksets
{
    [Transaction(TransactionMode.Manual)]
    public class Assign_Worksets : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet element)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            // Create filtered element collectors
            FilteredElementCollector pipeCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_FabricationPipework).WhereElementIsNotElementType();
            FilteredElementCollector hangerCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_FabricationHangers).WhereElementIsNotElementType();

            // Create a Dictionary to hold the service names and their corresponding elements
            Dictionary<string, List<Element>> serviceGroups = new Dictionary<string, List<Element>>();

            // Group pipe elements by service abbreviation
            foreach (Element elem in pipeCollector)
            {
                Parameter serviceParameter = elem.get_Parameter(BuiltInParameter.FABRICATION_SERVICE_ABBREVIATION);
                if (serviceParameter != null && serviceParameter.HasValue)
                {
                    string serviceAbbreviation = serviceParameter.AsString();
                    if (!serviceGroups.ContainsKey(serviceAbbreviation))
                    {
                        serviceGroups[serviceAbbreviation] = new List<Element>();
                    }
                    serviceGroups[serviceAbbreviation].Add(elem);
                }
            }

            // Group hanger elements by service abbreviation
            foreach (Element elem in hangerCollector)
            {
                Parameter serviceParameter = elem.get_Parameter(BuiltInParameter.FABRICATION_SERVICE_ABBREVIATION);
                if (serviceParameter != null && serviceParameter.HasValue)
                {
                    string serviceAbbreviation = serviceParameter.AsString();
                    if (!serviceGroups.ContainsKey(serviceAbbreviation))
                    {
                        serviceGroups[serviceAbbreviation] = new List<Element>();
                    }
                    serviceGroups[serviceAbbreviation].Add(elem);
                }
            }

            // Iterate through every key:value pair in the serviceGroups dictionary
            foreach (var group in serviceGroups)
            {
                // Assign key and value to variables
                string worksetName = group.Key.ToString();
                List<Element> elementsToAssign = group.Value;

                // Call the GetWorkset method and assign the result to the workset variable
                Workset workset = GetWorkset(doc, worksetName);

                if (workset != null)
                {
                    using (Transaction trans = new Transaction(doc, "Assign Worksets"))
                    {
                        trans.Start();

                        foreach (Element elem in elementsToAssign)
                        {
                            WorksetId worksetId = workset.Id;
                            elem.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).Set(worksetId.IntegerValue);
                        }
                        trans.Commit();
                    }
                }
            }

            // Dialog box to show What worksets were create and how many elements were assigned to each
            StringBuilder finalDialog = new StringBuilder();

            foreach (var group in serviceGroups)
            {
                string service = group.Key.ToString();
                int elements = group.Value.Count();
                finalDialog.AppendLine($"{service}: {elements}");
            }
            TaskDialog.Show("Assign Worksets", finalDialog.ToString());

            return Result.Succeeded;
        }

        private static Workset GetWorkset(Document doc, string worksetName)
        {
            // Get all worksets in the current document
            FilteredWorksetCollector worksets = new FilteredWorksetCollector(doc);

            // Determine if the workset exists
            foreach (Workset workset in worksets)
            {
                if (workset.Name.Equals(worksetName))
                {
                    return workset;
                }
            }

            using (Transaction trans = new Transaction(doc, "Create Workset"))
            {
                trans.Start();
                Workset newWorkset = Workset.Create(doc, worksetName);
                trans.Commit();

                return newWorkset;
            }
        }
    }
}