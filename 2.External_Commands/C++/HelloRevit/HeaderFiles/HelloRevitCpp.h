// HelloRevitCpp.h

#pragma once

using namespace System;
using namespace Autodesk::Revit::ApplicationServices;
using namespace Autodesk::Revit::Attributes;
using namespace Autodesk::Revit::DB;
using namespace Autodesk::Revit::UI;

namespace HelloRevitCpp {

	[Transaction(TransactionMode::Manual)]

	public ref class Command : IExternalCommand {

	public:
		virtual Result Execute(ExternalCommandData^ commandData, String^% message, ElementSet^ element);
	};
}