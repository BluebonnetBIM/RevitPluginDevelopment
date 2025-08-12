// NavisExporter.cp - Source File for the Navisworks Exporter

#include "NavisExporter.h"

using namespace NavisExporter;

Result Command::Execute(ExternalCommandData^ commandData, String^% message, ElementSet^ element) {

	UIApplication^ uiApp = commandData->Application;
	UIDocument^ uiDoc = uiApp->ActiveUIDocument;
	Document^ doc = uiDoc->Document;

	// Define the view name to export
	if (doc->Title == "") {
		
		// Define the view name were looking for as a variable
		String^ viewName = "";

		// Find the 3D view
		FilteredElementCollector^ collector = gcnew FilteredElementCollector(doc);
		IEnumerable<Element^>^ view3DCollection = collector->OfClass(View3D::typeid);
		View3D^ view3D;

		for each (Element^ element in view3DCollection) {

			View3D^ view = dynamic_cast<View3D^>(element);

			if (view != nullptr && !view->IsTemplate) {

				if (view->Name == viewName) {

					view3D = view;
					/*TaskDialog::Show("Test", view3D->Id->ToString());*/
				}
			}
		}

		// Define the export path
		String^ userProfile = Environment::GetEnvironmentVariable("USERPROFILE");
		String^ exportPath = String::Format("{0}\\Desktop", userProfile);

		// Export the NWC
		try {
			// Initialize export options
			NavisworksExportOptions^ nwcOptions = gcnew NavisworksExportOptions();

			// Set export options
			nwcOptions->ExportLinks = true;
			nwcOptions->ExportScope = NavisworksExportScope::View;
			nwcOptions->ViewId = view3D->Id;

			doc->Export(exportPath, viewName, nwcOptions);

			TaskDialog::Show("NWC Export", String::Format("3D view exported to:\n {0}", exportPath));

			return Result::Succeeded;
		}
		catch (Exception^ ex) {
			String^% message = String::Format("Error during export: {0}", ex);
			return Result::Failed;
		}
	}

	TaskDialog::Show("Navis Exporter", "");
}