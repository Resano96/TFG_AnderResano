using MiPlugin.Domain.Models;

namespace MiPlugin.Tests.Domain;

public class MeasurementTests
{
    [Fact]
    public void Quantity_WithArea_ReturnsArea()
    {
        var measurement = new Measurement { Area = 12.5 };

        Assert.Equal(12.5, measurement.Quantity);
    }

    [Fact]
    public void Quantity_WithoutArea_ReturnsOne()
    {
        var measurement = new Measurement { Area = null };

        Assert.Equal(1, measurement.Quantity);
    }

    [Fact]
    public void Unit_WithArea_IsSquareMeters()
    {
        var measurement = new Measurement { Area = 3 };

        Assert.Equal("m²", measurement.Unit);
    }

    [Fact]
    public void Unit_WithoutArea_IsUnits()
    {
        var measurement = new Measurement();

        Assert.Equal("ud", measurement.Unit);
    }

    [Fact]
    public void Quantity_WithZeroArea_ReturnsZeroAndSquareMeters()
    {
        // 0 m² sigue siendo "tiene área": no debe convertirse en 1 ud
        var measurement = new Measurement { Area = 0 };

        Assert.Equal(0, measurement.Quantity);
        Assert.Equal("m²", measurement.Unit);
    }
}
