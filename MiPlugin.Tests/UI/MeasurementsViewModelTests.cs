using MiPlugin.Domain.Models;
using MiPlugin.UI.ViewModels;

namespace MiPlugin.Tests.UI;

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

    // Filtrar y Exportar aún no están implementados: estos tests fallarán (a propósito)
    // cuando los implementes, y entonces habrá que sustituirlos por tests de su comportamiento.
    [Fact]
    public void FilterCommand_IsDisabledForNow()
    {
        var viewModel = new MeasurementsViewModel(FakeData);

        Assert.False(viewModel.FilterCommand.CanExecute(null));
    }

    [Fact]
    public void ExportCommand_IsDisabledForNow()
    {
        var viewModel = new MeasurementsViewModel(FakeData);

        Assert.False(viewModel.ExportCommand.CanExecute(null));
    }
}
