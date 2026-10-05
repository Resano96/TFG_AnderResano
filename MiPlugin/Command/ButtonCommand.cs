using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using MiPlugin.Core.Models;
using System.Runtime.InteropServices;

namespace MiPlugin.Command;

    [Transaction(TransactionMode.Manual)]
    internal class ButtonCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
        UIDocument uidoc = commandData.Application.ActiveUIDocument;
        Document doc = uidoc.Document;

        Reference r = uidoc.Selection.PickObject(ObjectType.Element, "Selecciona un elemento");
        Element elem = doc.GetElement(r);


        ElementId ID = elem.Id;
        Parameter largo =elem.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED);
        Parameter alto =elem.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);

        double longM = UnitUtils.ConvertFromInternalUnits(largo.AsDouble(), UnitTypeId.Meters);
        Muro muro = new Muro(ID.ToString(), largo.AsDouble(), alto.AsDouble());



        Parameter area =elem.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED);


        TaskDialog.Show("Titulo ", muro.Mostrar());
            return Result.Succeeded;
            
        }
    }
