using AR.Revit.Prometeo.UI.ViewModels;
using System.Windows;


namespace AR.Revit.Prometeo.UI.Views
{
    /// <summary>
    /// Lógica de interacción para MeasurementsWindow.xaml
    /// </summary>
    public partial class MeasurementsWindow : Window
    {
        public MeasurementsWindow(MeasurementsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
