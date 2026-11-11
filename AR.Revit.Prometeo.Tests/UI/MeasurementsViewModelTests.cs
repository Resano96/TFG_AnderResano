using AR.Revit.Prometeo.Domain.Models;
using AR.Revit.Prometeo.UI.ViewModels;

namespace AR.Revit.Prometeo.Tests.UI;

public class MeasurementsViewModelTests
{
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
        var viewModel = new MeasurementsViewModel(() => { calls++; return FakeData(); });

        Assert.Equal(0, calls);
        Assert.Empty(viewModel.Groups);
    }

    [Fact]
    public void MeasureCommand_CanAlwaysExecute()
    {
        var viewModel = new MeasurementsViewModel(FakeData);

        Assert.True(viewModel.MeasureCommand.CanExecute(null));
    }

    [Fact]
    public void MeasureCommand_FillsGroupsByCategory()
    {
        var viewModel = new MeasurementsViewModel(FakeData);

        viewModel.MeasureCommand.Execute(null);

        Assert.Equal(new[] { "Puertas", "Suelos" }, viewModel.Groups.Select(g => g.Name));
        Assert.Equal(30.0, viewModel.Groups.Single(g => g.Name == "Suelos").TotalArea);
    }

    [Fact]
    public void MeasureCommand_ExecutedTwice_DoesNotDuplicateGroups()
    {
        var viewModel = new MeasurementsViewModel(FakeData);

        viewModel.MeasureCommand.Execute(null);
        viewModel.MeasureCommand.Execute(null);

        Assert.Equal(2, viewModel.Groups.Count);
    }

    [Fact]
    public void MeasureCommand_ReadsModelEachTime()
    {
        // Si el modelo de Revit cambia entre dos clics, la tabla debe reflejarlo
        var calls = 0;
        var viewModel = new MeasurementsViewModel(() => { calls++; return FakeData(); });

        viewModel.MeasureCommand.Execute(null);
        viewModel.MeasureCommand.Execute(null);

        Assert.Equal(2, calls);
    }

    [Fact]
    public void MeasureCommand_EmptyModel_LeavesGroupsEmpty()
    {
        var viewModel = new MeasurementsViewModel(() => new List<Measurement>());

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
        var viewModel = new MeasurementsViewModel(MixedData);

        Assert.True(viewModel.FilterCommand.CanExecute(null));
    }

    [Fact]
    public void FilterCommand_ShowsOnlyMainGroups()
    {
        var viewModel = new MeasurementsViewModel(MixedData);

        viewModel.FilterCommand.Execute(null);

        Assert.Equal(new[] { "Muros", "Puertas" }, viewModel.Groups.Select(g => g.Name));
    }

    [Fact]
    public void FilterCommand_WorksWithoutMeasuringFirst()
    {
        // Filtrar lee el modelo por sí mismo: no depende de haber pulsado Medir
        var calls = 0;
        var viewModel = new MeasurementsViewModel(() => { calls++; return MixedData(); });

        viewModel.FilterCommand.Execute(null);

        Assert.Equal(1, calls);
        Assert.NotEmpty(viewModel.Groups);
    }

    [Fact]
    public void MeasureAfterFilter_ShowsAllGroupsAgain()
    {
        var viewModel = new MeasurementsViewModel(MixedData);

        viewModel.FilterCommand.Execute(null);
        viewModel.MeasureCommand.Execute(null);

        Assert.Equal(4, viewModel.Groups.Count);
    }

    // Exportar aún no está implementado: este test fallará (a propósito) cuando lo implementes,
    // y entonces habrá que sustituirlo por tests de su comportamiento.
    [Fact]
    public void ExportCommand_IsDisabledForNow()
    {
        var viewModel = new MeasurementsViewModel(FakeData);

        Assert.False(viewModel.ExportCommand.CanExecute(null));
    }
}
