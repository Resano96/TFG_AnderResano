using AR.Revit.Prometeo.Domain.Models;
using ClosedXML.Excel;

namespace AR.Revit.Prometeo.UI.Services
{
    public static class ExcelExporter
    {
        private static readonly string[] Headers =
            ["Categoría", "Familia", "Tipo", "Id", "Cantidad", "Unidad"];

        /// <summary>
        /// Exporta la lista de mediciones a excel
        /// </summary>
        /// <param name="measurements">Lista elementos medibles</param>
        /// <param name="path">Ubicacion donde se guarda el excel</param>
        public static void Export(IEnumerable<Measurement> measurements, string path)
        {
            //Creamos una nueva hoja llamada "Mediciones"
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Mediciones");

            //escribimos las cabeceras.
            //Se pone col +1 porque los array empiezan de 0 pero las columnas de excel desde el 1.
            for (int col = 0; col < Headers.Length; col++)
                sheet.Cell(1, col + 1).Value = Headers[col];

            int row = 2;
            foreach (var m in measurements)
            {
                sheet.Cell(row, 1).Value = m.Category;
                sheet.Cell(row, 2).Value = m.Family;
                sheet.Cell(row, 3).Value = m.TypeName;
                sheet.Cell(row, 4).Value = m.Id;
                sheet.Cell(row, 5).Value = m.Quantity;
                sheet.Cell(row, 6).Value = m.Unit;
                row++;
            }
            //ponemos en negrita la fila de la cabecera
            sheet.Row(1).Style.Font.Bold = true;
            //damos formato a la columna de las cantidades
            sheet.Column(5).Style.NumberFormat.Format = "0.00";
            //ajustamos al tamaño del contenido
            sheet.Columns().AdjustToContents();

            workbook.SaveAs(path);
        }
    }
}
