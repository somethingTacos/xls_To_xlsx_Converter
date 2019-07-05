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

        public static readonly DependencyProperty IsExpandedInfoBannerProperty =
            DependencyProperty.Register("IsExpandedInfoBanner", typeof(bool), typeof(MyBanner), new PropertyMetadata(false));

        public bool IsExpandedInfoBanner
        {
            get { return (bool)GetValue(IsExpandedInfoBannerProperty); }
            set { SetValue(IsExpandedInfoBannerProperty, value); }
        }

        public static readonly DependencyProperty BannerTemplateIndexProperty =
            DependencyProperty.Register("BannerTemplateIndex", typeof(int), typeof(MyBanner), new PropertyMetadata(0));

        public int BannerTemplateIndex
        {
            get { return (int)GetValue(BannerTemplateIndexProperty); }
            set { SetValue(BannerTemplateIndexProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedDialogBannerProperty =
            DependencyProperty.Register("IsExpandedDialogBanner", typeof(bool), typeof(MyBanner), new PropertyMetadata(false));

        public bool IsExpandedDialogBanner
        {
            get { return (bool)GetValue(IsExpandedDialogBannerProperty); }
            set { SetValue(IsExpandedDialogBannerProperty, value); }
        }
    }
}
