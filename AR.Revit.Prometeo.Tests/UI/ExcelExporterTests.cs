using AR.Revit.Prometeo.Domain.Models;
using AR.Revit.Prometeo.UI.Services;
using ClosedXML.Excel;
using System.IO;

namespace AR.Revit.Prometeo.Tests.UI;

public class ExcelExporterTests
{
    private static List<Measurement> SampleData() => new()
    {
        new Measurement { Id = "101", Category = "Muros",   Family = "Muro básico", TypeName = "Genérico 200 mm", Area = 12.5 },
        new Measurement { Id = "202", Category = "Puertas", Family = "Puerta simple", TypeName = "80x210" },
    };

    [Fact]
    public void Export_WritesHeadersInFirstRow()
    {
        var path = TempExcelPath();
        try
        {
            ExcelExporter.Export(SampleData(), path);

            using var workbook = new XLWorkbook(path);
            var sheet = workbook.Worksheet(1);
            var headers = Enumerable.Range(1, 6).Select(c => sheet.Cell(1, c).GetString());

            Assert.Equal(new[] { "Categoría", "Familia", "Tipo", "Id", "Cantidad", "Unidad" }, headers);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Export_WritesOneRowPerMeasurement()
    {
        var path = TempExcelPath();
        try
        {
            ExcelExporter.Export(SampleData(), path);

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
    public void Export_WritesValuesInTheRightColumns()
    {
        var path = TempExcelPath();
        try
        {
            ExcelExporter.Export(SampleData(), path);

            using var workbook = new XLWorkbook(path);
            var sheet = workbook.Worksheet(1);

            Assert.Equal("Muros", sheet.Cell(2, 1).GetString());
            Assert.Equal("Muro básico", sheet.Cell(2, 2).GetString());
            Assert.Equal("Genérico 200 mm", sheet.Cell(2, 3).GetString());
            Assert.Equal("101", sheet.Cell(2, 4).GetString());
            Assert.Equal("m²", sheet.Cell(2, 6).GetString());
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Export_QuantityIsSavedAsNumber()
    {
        // Como número se puede sumar en Excel; como texto no
        var path = TempExcelPath();
        try
        {
            ExcelExporter.Export(SampleData(), path);

            using var workbook = new XLWorkbook(path);
            var sheet = workbook.Worksheet(1);

            Assert.Equal(12.5, sheet.Cell(2, 5).GetDouble());
            Assert.Equal(1.0, sheet.Cell(3, 5).GetDouble()); // la puerta: 1 ud
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Export_EmptyList_WritesOnlyHeaders()
    {
        var path = TempExcelPath();
        try
        {
            ExcelExporter.Export(new List<Measurement>(), path);

            using var workbook = new XLWorkbook(path);

            Assert.Equal(1, workbook.Worksheet(1).RowsUsed().Count());
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
