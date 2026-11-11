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
            FilterCommand = new RelayCommand(Filter);
            ExportCommand = new RelayCommand(() => { }, () => false);
        }
        private void Measure() => ShowGroups(_readModel());

        private void Filter() => ShowGroups(MeasurementFilter.MainElements(_readModel()));

        private void ShowGroups(IEnumerable<Measurement> measurements)
        {
            var groups = MeasurementGrouping.ByCategory(measurements);

            Groups.Clear();
            foreach (var group in groups)
                Groups.Add(group);
        }
    }
}