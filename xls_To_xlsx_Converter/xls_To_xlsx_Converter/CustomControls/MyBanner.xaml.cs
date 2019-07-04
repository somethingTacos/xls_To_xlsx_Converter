using System.Windows;
using System.Windows.Controls;

namespace xls_To_xlsx_Converter.CustomControls
{
    /// <summary>
    /// Interaction logic for MyBanner.xaml
    /// </summary>
    public partial class MyBanner : UserControl
    {
        public MyBanner()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(MyBanner), new PropertyMetadata(false));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty BannerTemplateIndexProperty =
            DependencyProperty.Register("BannerTemplateIndex", typeof(int), typeof(MyBanner), new PropertyMetadata(0));

        public int BannerTemplateIndex
        {
            get { return (int)GetValue(BannerTemplateIndexProperty); }
            set { SetValue(BannerTemplateIndexProperty, value); }
        }
    }
}
