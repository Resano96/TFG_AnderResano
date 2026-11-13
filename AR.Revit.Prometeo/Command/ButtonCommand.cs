using AR.Revit.Prometeo.Services;
using AR.Revit.Prometeo.UI.Services;
using AR.Revit.Prometeo.UI.ViewModels;
using AR.Revit.Prometeo.UI.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Interop;

namespace AR.Revit.Prometeo.Command;

[Transaction(TransactionMode.Manual)]
public class ButtonCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            // 1. Documento abierto
            Document? doc = commandData.Application.ActiveUIDocument?.Document;
            if (doc == null)
            {
                message = "Abre un modelo antes de usar el plugin.";
                return Result.Failed;
            }

            // 2. Lector del modelo de Revit
            var reader = new RevitMeasurementReader(doc);
            var viewModel = new MeasurementsViewModel(reader.ReadModel, FileDialogs.AskExcelPath);

            // 3. Ventana modal con Revit como propietaria
            var window = new MeasurementsWindow(viewModel);
            new WindowInteropHelper(window).Owner = commandData.Application.MainWindowHandle;
            window.ShowDialog();

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            message = ex.Message;
            return Result.Failed;
        }
    }
}
