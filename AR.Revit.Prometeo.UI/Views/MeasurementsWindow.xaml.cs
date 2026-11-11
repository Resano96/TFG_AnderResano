using AR.Revit.Prometeo.UI.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;


namespace AR.Revit.Prometeo.UI.Views
{
    /// <summary>
    /// Lógica de interacción para MeasurementsWindow.xaml
    /// </summary>
    public partial class MeasurementsWindow : Window
    {
        public MeasurementsWindow(MeasurementsViewModel viewModel)
        {
            Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
