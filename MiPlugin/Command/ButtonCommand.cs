using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using MiPlugin.Domain.Models;
using MiPlugin.UI.ViewModels;
using MiPlugin.UI.Views;

using System.Windows.Interop;

namespace MiPlugin.Command;

    [Transaction(TransactionMode.Manual)]
    public class ButtonCommand : IExternalCommand
    {
        private static List<Measurement> FakeData() => new()
        {
        new Measurement { Id = "1", Category = "Suelos",  Family = "Suelo genérico 30cm", Area = 12 },
        new Measurement { Id = "2", Category = "Suelos",  Family = "Suelo genérico 30cm", Area = 18 },
        new Measurement { Id = "3", Category = "Puertas", Family = "Puerta simple 80x210" },
        new Measurement { Id = "4", Category = "Puertas", Family = "Puerta doble 160x210" },
        };




    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
        var viewModel = new MeasurementsViewModel(FakeData);
        var window = new MeasurementsWindow(viewModel);

        new WindowInteropHelper(window).Owner = commandData.Application.MainWindowHandle;
        window.ShowDialog();

        return Result.Succeeded;
        }
    }
