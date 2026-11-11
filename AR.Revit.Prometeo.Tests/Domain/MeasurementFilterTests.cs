using AR.Revit.Prometeo.Domain.Models;
using AR.Revit.Prometeo.Domain.Services;

namespace AR.Revit.Prometeo.Tests.Domain;

public class MeasurementFilterTests
{
    [Theory]
    [InlineData(ElementKind.Wall)]
    [InlineData(ElementKind.Floor)]
    [InlineData(ElementKind.Window)]
    [InlineData(ElementKind.Door)]
    public void MainElements_KeepsMainKinds(ElementKind kind)
    {
        var measurements = new[] { new Measurement { Kind = kind } };

        var result = MeasurementFilter.MainElements(measurements);

        Assert.Single(result);
    }

    [Fact]
    public void MainElements_DiscardsOther()
    {
        var measurements = new[] { new Measurement { Kind = ElementKind.Other } };

        var result = MeasurementFilter.MainElements(measurements);

        Assert.Empty(result);
    }

    [Fact]
    public void MainElements_MixedList_KeepsOnlyMainElements()
    {
        var measurements = new[]
        {
            new Measurement { Id = "1", Kind = ElementKind.Wall },
            new Measurement { Id = "2", Kind = ElementKind.Other },
            new Measurement { Id = "3", Kind = ElementKind.Door },
            new Measurement { Id = "4", Kind = ElementKind.Other },
        };

        var result = MeasurementFilter.MainElements(measurements);

        Assert.Equal(new[] { "1", "3" }, result.Select(m => m.Id));
    }

    [Fact]
    public void MainElements_EmptyInput_ReturnsEmptyList()
    {
        var result = MeasurementFilter.MainElements(new List<Measurement>());

        Assert.Empty(result);
    }

    [Fact]
    public void MainElements_DoesNotModifyOriginalList()
    {
        // El filtro devuelve una lista nueva: los datos leídos no se pierden
        var measurements = new List<Measurement>
        {
            new() { Kind = ElementKind.Wall },
            new() { Kind = ElementKind.Other },
        };

        MeasurementFilter.MainElements(measurements);

        Assert.Equal(2, measurements.Count);
    }
}
