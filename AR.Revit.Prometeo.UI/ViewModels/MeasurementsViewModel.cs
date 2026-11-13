using AR.Revit.Prometeo.Domain.Models;
using AR.Revit.Prometeo.Domain.Services;
using AR.Revit.Prometeo.UI.Commands;
using AR.Revit.Prometeo.UI.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace AR.Revit.Prometeo.UI.ViewModels
{
    /// <summary>
    /// ViewModel de la ventana de mediciones. Hace de intermediario entre la vista (XAML)
    /// y los datos: la vista solo se enlaza a sus propiedades y comandos, sin lógica propia.
    /// </summary>
    public class MeasurementsViewModel : ViewModelBase
    {
        // Función que devuelve las mediciones del modelo. Se recibe desde fuera (inyección de dependencias):
        // en Revit es RevitMeasurementReader.ReadModel y en los tests una función con datos de prueba.
        // Así el ViewModel no conoce la API de Revit.
        private readonly Func<List<Measurement>> _readModel;

        // Mediciones que se están mostrando ahora mismo (todas o filtradas).
        // Se guardan para exportar exactamente lo que se ve. Empieza vacía, nunca null.
        private List<Measurement> _shown = [];

        // Funcion que pregunta la ruta donde vas a guardar el excel
        private readonly Func<string?> _askSavePath;

        // Mensaje que se muestra al pie de la ventana (resultado de exportar).
        private string _status = string.Empty;

        /// <summary>Mensaje de estado para el usuario (exportación correcta o error).</summary>
        public string Status
        {
            get => _status;
            private set
            {
                _status = value;
                OnPropertyChanged(); // Avisa a la vista para que actualice el texto
            }
        }

        /// <summary>Grupos por categoría que muestra la ventana. Es una ObservableCollection:
        /// avisa a la vista cada vez que se añade o se quita un grupo, y la tabla se refresca sola.</summary>
        public ObservableCollection<MeasurementGroup> Groups { get; } = [];

        /// <summary>Comando del botón Medir: muestra todos los elementos del modelo.</summary>
        public ICommand MeasureCommand { get; }

        /// <summary>Comando del botón Filtrar: muestra solo muros, suelos, ventanas y puertas.</summary>
        public ICommand FilterCommand { get; }

        /// <summary>Comando del botón Exportar: guarda en Excel las mediciones mostradas.</summary>
        public ICommand ExportCommand { get; }

        /// <summary>Crea el ViewModel con la función que lee el modelo.</summary>
        /// <param name="readModel">Función que devuelve las mediciones del modelo.</param>
        /// <param name="askSavePath">Funcion que devuelve la ruta si se guarda o null si no</param>
        public MeasurementsViewModel(Func<List<Measurement>> readModel, Func<string?> askSavePath)
        {
            _readModel = readModel;
            _askSavePath = askSavePath;

            // Cada RelayCommand conecta un botón con un método.
            // El segundo parámetro (opcional) dice si el botón está activo.
            MeasureCommand = new RelayCommand(Measure);
            FilterCommand = new RelayCommand(Filter);
            ExportCommand = new RelayCommand(Export, () => _shown.Count > 0);
        }

        // Lee el modelo completo y lo muestra.
        private void Measure() => ShowGroups(_readModel());

        // Lee el modelo, se queda solo con los elementos principales y los muestra.
        // Vuelve a leer en lugar de filtrar lo que hay en pantalla: funciona sin pulsar Medir antes
        // y refleja los cambios hechos en Revit.
        private void Filter() => ShowGroups(MeasurementFilter.MainElements(_readModel()));

        // Parte común de Medir y Filtrar (DRY): guarda la lista, agrupa por categoría y rellena la tabla.
        // Como todo lo que se muestra pasa por aquí, _shown siempre coincide con lo que se ve.
        private void ShowGroups(IEnumerable<Measurement> measurements)
        {
            // Copia propia de la lista, para exportar después lo que se está viendo.
            _shown = measurements.ToList();

            // La agrupación y los totales están en el dominio, no en el ViewModel.
            var groups = MeasurementGrouping.ByCategory(_shown);

            // Se vacía y se rellena la colección existente en lugar de crear una nueva:
            // la vista está enlazada a esta instancia y se entera de cada cambio.
            Groups.Clear();
            foreach (var group in groups)
                Groups.Add(group);
        }

        /// <summary>Exporta la medicion hecha a excel.</summary>
        /// <remarks> Primero consigue la ruta donde buscamos guardar el excel. Despues, exporta la lista.</remarks>
        private void Export()
        {
            string? path = _askSavePath();
            if (path == null)
                return; // El usuario ha cancelado

            try
            {
                ExcelExporter.Export(_shown, path);
                Status = $"Exportado: {path}";
            }
            catch (IOException)
            {
                Status = "No se pudo guardar. ¿Está el fichero abierto en Excel?";
            }
        }
    }
}