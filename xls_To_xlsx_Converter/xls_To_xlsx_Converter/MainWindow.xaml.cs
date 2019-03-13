using System.Windows;
using xls_To_xlsx_Converter.ViewModel;

namespace xls_To_xlsx_Converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewmodel = new NavigationViewModel();
            viewmodel.SelectedViewModel = new FileConverterViewModel(viewmodel);
            this.DataContext = viewmodel;
        }
    }
}
