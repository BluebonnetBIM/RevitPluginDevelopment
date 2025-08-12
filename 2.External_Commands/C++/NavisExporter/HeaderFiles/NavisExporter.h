// NavisExporter.h - 

#pragma once

#include <iostream>
#include <filesystem>

using namespace System;
using namespace System::Collections::Generic;
using namespace Autodesk::Revit::ApplicationServices;
using namespace Autodesk::Revit::Attributes;
using namespace Autodesk::Revit::UI;
using namespace Autodesk::Revit::DB;

namespace NavisExporter {

	[Transaction(TransactionMode::Manual)]

	public ref class Command : IExternalCommand {

	public:
		virtual Result Execute(ExternalCommandData^ commandData, String^% message, ElementSet^ element);
	};
}