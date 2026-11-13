using Microsoft.Win32;

namespace AR.Revit.Prometeo.UI.Services
{
    public static class FileDialogs
    {

        /// <summary>Abre el dialogo "Guardar como" para que el usuario elija donde guardar el Excel </summary>
        /// <returns>Devuelve la ruta si le da a guardar o null si no le da</returns>
        public static string? AskExcelPath()
        {
            var dialog = new SaveFileDialog
            {
                //Filter hace que solo puedas guardar .xlsx
                Filter = "Libro de Excel (*.xlsx)|*.xlsx",
                FileName = "Mediciones.xlsx"
            };
            // ShowDialog es true si se le da a guardar, false o null si le da a cancelar
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}
