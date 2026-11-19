using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR.Revit.Prometeo.Command
{
    [Transaction(TransactionMode.Manual)]
    public class ParamButton : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Document? doc = commandData.Application.ActiveUIDocument?.Document;
                if (doc == null)
                {
                    message = "Abre un modelo antes de usar el plugin.";
                    return Result.Failed;
                }
                TaskDialog.Show("Parámetros", "Próximamente");
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
