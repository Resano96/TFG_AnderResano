using AR.Revit.Prometeo.Domain.Models;
using AR.Revit.Prometeo.Domain.Services;

namespace AR.Revit.Prometeo.Tests.Domain;

public class ParameterDiffTests
{
    // Crea una fila con un solo parámetro, para no repetir código en cada test
    private static ParameterRow Row(string id, string param, string value) => new()
    {
        Id = id,
        Values = new() { [param] = value }
    };

    [Fact]
    public void Compare_SameValues_ReturnsNoChanges()
    {
        var model = new[] { Row("1", "Marca", "M-01") };
        var excel = new[] { Row("1", "Marca", "M-01") };

        var changes = ParameterDiff.Compare(model, excel);

        Assert.Empty(changes);
    }

    [Fact]
    public void Compare_ChangedValue_ReturnsOneChangeWithAllData()
    {
        var model = new[] { Row("1", "Marca", "M-01") };
        var excel = new[] { Row("1", "Marca", "M-02") };

        var changes = ParameterDiff.Compare(model, excel);

        var change = Assert.Single(changes);
        Assert.Equal("1", change.ElementId);
        Assert.Equal("Marca", change.Parameter);
        Assert.Equal("M-01", change.OldValue);
        Assert.Equal("M-02", change.NewValue);
    }

    [Fact]
    public void Compare_OnlyWhitespaceDiffers_ReturnsNoChanges()
    {
        // Un espacio de más al editar en Excel no debe contar como cambio
        var model = new[] { Row("1", "Marca", "M-01") };
        var excel = new[] { Row("1", "Marca", "  M-01 ") };

        var changes = ParameterDiff.Compare(model, excel);

        Assert.Empty(changes);
    }

    [Fact]
    public void Compare_UnknownElementId_IsIgnored()
    {
        // Si el usuario cambia el Id en el Excel, esa fila no puede aplicarse a ningún elemento
        var model = new[] { Row("1", "Marca", "M-01") };
        var excel = new[] { Row("999", "Marca", "M-02") };

        var changes = ParameterDiff.Compare(model, excel);

        Assert.Empty(changes);
    }

    [Fact]
    public void Compare_UnknownParameter_IsIgnored()
    {
        // Una columna inventada en el Excel no corresponde a ningún parámetro del elemento
        var model = new[] { Row("1", "Marca", "M-01") };
        var excel = new[] { Row("1", "Inventado", "X") };

        var changes = ParameterDiff.Compare(model, excel);

        Assert.Empty(changes);
    }

    [Fact]
    public void Compare_TwoElementsOnlyOneChanged_ReturnsChangeOfTheRightElement()
    {
        var model = new[] { Row("1", "Marca", "M-01"), Row("2", "Marca", "M-02") };
        var excel = new[] { Row("1", "Marca", "M-01"), Row("2", "Marca", "NUEVA") };

        var changes = ParameterDiff.Compare(model, excel);

        var change = Assert.Single(changes);
        Assert.Equal("2", change.ElementId);
        Assert.Equal("NUEVA", change.NewValue);
    }

    [Fact]
    public void Compare_SeveralParametersChangedInOneElement_ReturnsOneChangePerParameter()
    {
        var model = new[]
        {
            new ParameterRow { Id = "1", Values = new() { ["Marca"] = "M-01", ["Comentarios"] = "", ["Altura"] = "2.10" } }
        };
        var excel = new[]
        {
            new ParameterRow { Id = "1", Values = new() { ["Marca"] = "M-09", ["Comentarios"] = "Revisar", ["Altura"] = "2.10" } }
        };

        var changes = ParameterDiff.Compare(model, excel);

        Assert.Equal(2, changes.Count);
        Assert.Contains(changes, c => c.Parameter == "Marca" && c.NewValue == "M-09");
        Assert.Contains(changes, c => c.Parameter == "Comentarios" && c.NewValue == "Revisar");
    }

    [Fact]
    public void Compare_ParameterMissingInExcel_IsNotAChange()
    {
        // Si el usuario borra una columna del Excel, el parámetro no se toca (no se vacía)
        var model = new[]
        {
            new ParameterRow { Id = "1", Values = new() { ["Marca"] = "M-01", ["Comentarios"] = "Original" } }
        };
        var excel = new[] { Row("1", "Marca", "M-01") };

        var changes = ParameterDiff.Compare(model, excel);

        Assert.Empty(changes);
    }

    [Fact]
    public void Compare_ValueClearedInExcel_IsAChange()
    {
        // Vaciar una celda sí es un cambio: el usuario quiere dejar el parámetro en blanco
        var model = new[] { Row("1", "Comentarios", "Revisar") };
        var excel = new[] { Row("1", "Comentarios", "") };

        var changes = ParameterDiff.Compare(model, excel);

        var change = Assert.Single(changes);
        Assert.Equal(string.Empty, change.NewValue);
    }

    [Fact]
    public void Compare_EmptyExcel_ReturnsNoChanges()
    {
        var model = new[] { Row("1", "Marca", "M-01") };

        var changes = ParameterDiff.Compare(model, Array.Empty<ParameterRow>());

        Assert.Empty(changes);
    }
}
