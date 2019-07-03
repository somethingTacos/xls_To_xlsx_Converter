using System.Windows;
using System.Windows.Controls;

namespace xls_To_xlsx_Converter.Views
{
    /// <summary>
    /// Interaction logic for FileConverterView.xaml
    /// </summary>
    public partial class FileConverterView : UserControl
    {
        public FileConverterView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta / 10);
            e.Handled = true;
        }
    }
}
