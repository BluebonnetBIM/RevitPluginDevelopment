// HelloRevitCpp.cpp - This is the main DLL file

#include "HelloRevitCpp.h"

using namespace HelloRevitCpp;

Result Command::Execute(ExternalCommandData^ commandData, String^% message, ElementSet^ element) {

	TaskDialog::Show("Revit", "Test");
	return Result::Succeeded;
}