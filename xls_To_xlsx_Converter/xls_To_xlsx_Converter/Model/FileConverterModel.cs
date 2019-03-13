using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace xls_To_xlsx_Converter.Model
{
    public class FileConverterModel { }

    [AddINotifyPropertyChangedInterface]
    public class FileConverter
    {
        public int TotalFilesToConvert { get; set; } = 0;
        public int FilesConverted { get; set; } = 0;

        public ObservableCollection<FileInfo> FilesToConvert { get; set; } = new ObservableCollection<FileInfo>();
    }
}
