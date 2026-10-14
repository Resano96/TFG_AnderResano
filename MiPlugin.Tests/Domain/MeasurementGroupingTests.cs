using MiPlugin.Domain.Models;
using MiPlugin.Domain.Services;

namespace MiPlugin.Tests.Domain;

public class MeasurementGroupingTests
{
    private static List<Measurement> SampleData() => new()
    {
        new Measurement { Id = "1", Category = "Suelos",  Family = "Suelo 30cm", Area = 12 },
        new Measurement { Id = "2", Category = "Puertas", Family = "Puerta simple" },
        new Measurement { Id = "3", Category = "Suelos",  Family = "Suelo 30cm", Area = 18 },
        new Measurement { Id = "4", Category = "Muros",   Family = "Muro básico", Area = 25 },
    };

    [Fact]
    public void ByCategory_CreatesOneGroupPerCategory()
    {
        var groups = MeasurementGrouping.ByCategory(SampleData());

        Assert.Equal(3, groups.Count);
    }

    [Fact]
    public void ByCategory_OrdersGroupsByName()
    {
        var groups = MeasurementGrouping.ByCategory(SampleData());

        Assert.Equal(new[] { "Muros", "Puertas", "Suelos" }, groups.Select(g => g.Name));
    }

    [Fact]
    public void ByCategory_PutsEachMeasurementInItsGroup()
    {
        var groups = MeasurementGrouping.ByCategory(SampleData());
        var floors = groups.Single(g => g.Name == "Suelos");

        Assert.Equal(2, floors.Count);
        Assert.All(floors.Items, m => Assert.Equal("Suelos", m.Category));
        Assert.Equal(30.0, floors.TotalArea);
    }

    [Fact]
    public void ByCategory_KeepsAllMeasurements()
    {
        var data = SampleData();

        var groups = MeasurementGrouping.ByCategory(data);

        Assert.Equal(data.Count, groups.Sum(g => g.Count));
    }

    [Fact]
    public void ByCategory_EmptyInput_ReturnsEmptyList()
    {
        var groups = MeasurementGrouping.ByCategory(new List<Measurement>());

        Assert.Empty(groups);
    }
}
