using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace xls_To_xlsx_Converter.CustomControls
{
    public class CustomDockPanel : DockPanel
    {
        public static readonly DependencyProperty IsIncludedProperty =
            DependencyProperty.Register("IsIncluded", typeof(bool), typeof(CustomDockPanel), new PropertyMetadata(false));

        public bool IsIncluded
        {
            get { return (bool)GetValue(IsIncludedProperty); }
            set { SetValue(IsIncludedProperty, value); }
        }

        public static readonly DependencyProperty IsHiddenProperty =
            DependencyProperty.Register("IsHidden", typeof(bool), typeof(CustomDockPanel), new PropertyMetadata(false));

        public bool IsHidden
        {
            get { return (bool)GetValue(IsHiddenProperty); }
            set { SetValue(IsHiddenProperty, value); }
        }
    }
}
