using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace xls_To_xlsx_Converter.CustomControls
{
    public class MyButton : Button
    {
        public static readonly DependencyProperty NonSelectedFilesExistProperty =
            DependencyProperty.Register("NonSelectedFilesExist", typeof(bool), typeof(MyButton), new PropertyMetadata(false));

        public bool NonSelectedFilesExist
        {
            get { return (bool)GetValue(NonSelectedFilesExistProperty); }
            set { SetValue(NonSelectedFilesExistProperty, value); }
        }
    }
}
