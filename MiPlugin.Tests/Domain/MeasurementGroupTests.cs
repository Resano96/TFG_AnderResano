using MiPlugin.Domain.Models;

namespace MiPlugin.Tests.Domain;

public class MeasurementGroupTests
{
    private static Measurement WithArea(double area) => new() { Category = "Suelos", Area = area };
    private static Measurement WithoutArea() => new() { Category = "Puertas" };

    [Fact]
    public void Count_ReturnsNumberOfItems()
    {
        var group = new MeasurementGroup("Puertas", new[] { WithoutArea(), WithoutArea(), WithoutArea() });

        Assert.Equal(3, group.Count);
    }

    [Fact]
    public void TotalArea_SumsAllAreas()
    {
        var group = new MeasurementGroup("Suelos", new[] { WithArea(12), WithArea(18) });

        Assert.Equal(30.0, group.TotalArea);
    }

    [Fact]
    public void TotalArea_WithoutAreas_ReturnsNull()
    {
        var group = new MeasurementGroup("Puertas", new[] { WithoutArea(), WithoutArea() });

        Assert.Null(group.TotalArea);
    }

    [Fact]
    public void TotalArea_MixedItems_IgnoresItemsWithoutArea()
    {
        var group = new MeasurementGroup("Mixto", new[] { WithArea(10), WithoutArea() });

        Assert.Equal(10.0, group.TotalArea);
    }

    [Fact]
    public void Summary_WithArea_ShowsTotalInSquareMeters()
    {
        var group = new MeasurementGroup("Suelos", new[] { WithArea(12), WithArea(18) });

        // Se calcula el esperado con el mismo formato para no depender del idioma del PC (30,00 vs 30.00)
        Assert.Equal($"{30.0:N2} m²", group.Summary);
    }

    [Fact]
    public void Summary_WithoutArea_ShowsUnits()
    {
        var group = new MeasurementGroup("Puertas", new[] { WithoutArea(), WithoutArea() });

        Assert.Equal("2 ud.", group.Summary);
    }

    [Fact]
    public void EmptyGroup_HasZeroCountAndNoArea()
    {
        var group = new MeasurementGroup("Vacío", Array.Empty<Measurement>());

        Assert.Equal(0, group.Count);
        Assert.Null(group.TotalArea);
        Assert.Equal("0 ud.", group.Summary);
    }
}
