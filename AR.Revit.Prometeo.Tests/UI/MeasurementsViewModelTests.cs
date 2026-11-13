using AR.Revit.Prometeo.Domain.Models;
using AR.Revit.Prometeo.UI.ViewModels;
using ClosedXML.Excel;
using System.IO;

namespace AR.Revit.Prometeo.Tests.UI;

public class MeasurementsViewModelTests
{
    // Simula que el usuario cancela el diálogo "Guardar como".
    // Se usa en los tests que no exportan: así nunca se abre ninguna ventana.
    private static string? Cancel() => null;

    private static List<Measurement> FakeData() => new()
    {
        new Measurement { Id = "1", Category = "Suelos",  Area = 12 },
        new Measurement { Id = "2", Category = "Suelos",  Area = 18 },
        new Measurement { Id = "3", Category = "Puertas" },
    };

    [Fact]
    public void Constructor_DoesNotReadModel_AndGroupsIsEmpty()
    {
        var calls = 0;
        var viewModel = new MeasurementsViewModel(() => { calls++; return FakeData(); }, Cancel);

        Assert.Equal(0, calls);
        Assert.Empty(viewModel.Groups);
    }

    [Fact]
    public void MeasureCommand_CanAlwaysExecute()
    {
        var viewModel = new MeasurementsViewModel(FakeData, Cancel);

        Assert.True(viewModel.MeasureCommand.CanExecute(null));
    }

    [Fact]
    public void MeasureCommand_FillsGroupsByCategory()
    {
        var viewModel = new MeasurementsViewModel(FakeData, Cancel);

        viewModel.MeasureCommand.Execute(null);

        Assert.Equal(new[] { "Puertas", "Suelos" }, viewModel.Groups.Select(g => g.Name));
        Assert.Equal(30.0, viewModel.Groups.Single(g => g.Name == "Suelos").TotalArea);
    }

    [Fact]
    public void MeasureCommand_ExecutedTwice_DoesNotDuplicateGroups()
    {
        var viewModel = new MeasurementsViewModel(FakeData, Cancel);

        viewModel.MeasureCommand.Execute(null);
        viewModel.MeasureCommand.Execute(null);

        Assert.Equal(2, viewModel.Groups.Count);
    }

    [Fact]
    public void MeasureCommand_ReadsModelEachTime()
    {
        // Si el modelo de Revit cambia entre dos clics, la tabla debe reflejarlo
        var calls = 0;
        var viewModel = new MeasurementsViewModel(() => { calls++; return FakeData(); }, Cancel);

        viewModel.MeasureCommand.Execute(null);
        viewModel.MeasureCommand.Execute(null);

        Assert.Equal(2, calls);
    }

    [Fact]
    public void MeasureCommand_EmptyModel_LeavesGroupsEmpty()
    {
        var viewModel = new MeasurementsViewModel(() => new List<Measurement>(), Cancel);

        viewModel.MeasureCommand.Execute(null);

        Assert.Empty(viewModel.Groups);
    }

    private static List<Measurement> MixedData() => new()
    {
        new Measurement { Id = "1", Category = "Muros",     Kind = ElementKind.Wall, Area = 20 },
        new Measurement { Id = "2", Category = "Puertas",   Kind = ElementKind.Door },
        new Measurement { Id = "3", Category = "Mobiliario", Kind = ElementKind.Other },
        new Measurement { Id = "4", Category = "Sanitarios", Kind = ElementKind.Other },
    };

    [Fact]
    public void FilterCommand_CanAlwaysExecute()
    {
        var viewModel = new MeasurementsViewModel(MixedData, Cancel);

        Assert.True(viewModel.FilterCommand.CanExecute(null));
    }

    [Fact]
    public void FilterCommand_ShowsOnlyMainGroups()
    {
        var viewModel = new MeasurementsViewModel(MixedData, Cancel);

        viewModel.FilterCommand.Execute(null);

        Assert.Equal(new[] { "Muros", "Puertas" }, viewModel.Groups.Select(g => g.Name));
    }

    [Fact]
    public void FilterCommand_WorksWithoutMeasuringFirst()
    {
        // Filtrar lee el modelo por sí mismo: no depende de haber pulsado Medir
        var calls = 0;
        var viewModel = new MeasurementsViewModel(() => { calls++; return MixedData(); }, Cancel);

        viewModel.FilterCommand.Execute(null);

        Assert.Equal(1, calls);
        Assert.NotEmpty(viewModel.Groups);
    }

    [Fact]
    public void MeasureAfterFilter_ShowsAllGroupsAgain()
    {
        var viewModel = new MeasurementsViewModel(MixedData, Cancel);

        viewModel.FilterCommand.Execute(null);
        viewModel.MeasureCommand.Execute(null);

        Assert.Equal(4, viewModel.Groups.Count);
    }

    [Fact]
    public void ExportCommand_IsDisabledBeforeMeasuring()
    {
        // Sin datos en la tabla no hay nada que exportar
        var viewModel = new MeasurementsViewModel(FakeData, Cancel);

        Assert.False(viewModel.ExportCommand.CanExecute(null));
    }

    [Fact]
    public void ExportCommand_IsEnabledAfterMeasuring()
    {
        var viewModel = new MeasurementsViewModel(FakeData, Cancel);

        viewModel.MeasureCommand.Execute(null);

        Assert.True(viewModel.ExportCommand.CanExecute(null));
    }

    [Fact]
    public void Export_UserCancels_DoesNothing()
    {
        var viewModel = new MeasurementsViewModel(FakeData, Cancel);
        viewModel.MeasureCommand.Execute(null);

        viewModel.ExportCommand.Execute(null);

        Assert.Equal(string.Empty, viewModel.Status);
    }

    [Fact]
    public void Export_WritesFileAndReportsSuccess()
    {
        var path = TempExcelPath();
        try
        {
            var viewModel = new MeasurementsViewModel(FakeData, () => path);
            viewModel.MeasureCommand.Execute(null);

            viewModel.ExportCommand.Execute(null);

            Assert.True(File.Exists(path));
            Assert.Contains("Exportado", viewModel.Status);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Export_AfterFilter_ExportsOnlyFilteredElements()
    {
        // Se exporta lo que se ve: tras Filtrar, solo los 2 elementos principales de MixedData
        var path = TempExcelPath();
        try
        {
            var viewModel = new MeasurementsViewModel(MixedData, () => path);
            viewModel.FilterCommand.Execute(null);

            viewModel.ExportCommand.Execute(null);

            using var workbook = new XLWorkbook(path);
            var dataRows = workbook.Worksheet(1).RowsUsed().Count() - 1; // menos la cabecera
            Assert.Equal(2, dataRows);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Export_FileInUse_ShowsErrorAndDoesNotThrow()
    {
        // Simula el fichero abierto en Excel: se bloquea mientras se intenta exportar
        var path = TempExcelPath();
        try
        {
            using (new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var viewModel = new MeasurementsViewModel(FakeData, () => path);
                viewModel.MeasureCommand.Execute(null);

                viewModel.ExportCommand.Execute(null);

                Assert.Contains("No se pudo guardar", viewModel.Status);
            }
        }
        finally
        {
            File.Delete(path);
        }
    }

    // Ruta única en la carpeta temporal, para que los tests no choquen entre sí
    private static string TempExcelPath() =>
        Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.xlsx");
}
