using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Runtime.InteropServices;

namespace MiPlugin.Command;

    [Transaction(TransactionMode.Manual)]
    internal class ButtonCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
        UIDocument uidoc = commandData.Application.ActiveUIDocument;
        Document doc = uidoc.Document;

        TaskDialog.Show("Title ", "It works");
            return Result.Succeeded;
            
        }
    }
