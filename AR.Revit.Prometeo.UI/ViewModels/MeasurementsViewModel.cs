using AR.Revit.Prometeo.Domain.Models;
using AR.Revit.Prometeo.Domain.Services;
using AR.Revit.Prometeo.UI.Commands;

using System.Collections.ObjectModel;

using System.Windows.Input;

namespace AR.Revit.Prometeo.UI.ViewModels
{
    public class MeasurementsViewModel : ViewModelBase
    {
        private readonly Func<List<Measurement>> _readModel;
        public ObservableCollection<MeasurementGroup> Groups { get; } = [];
        public ICommand MeasureCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand ExportCommand { get; }

        public MeasurementsViewModel(Func<List<Measurement>> readModel)
        {
            _readModel = readModel;
            MeasureCommand = new RelayCommand(Measure);
            FilterCommand = new RelayCommand(() => { }, () => false);
            ExportCommand = new RelayCommand(() => { }, () => false);

        }
        private void Measure()
        {
            var measurements = _readModel();
            var groups = MeasurementGrouping.ByCategory(measurements);

            Groups.Clear();
            foreach (var group in groups)
            {
                Groups.Add(group);
            }
        }
    }
    
}
