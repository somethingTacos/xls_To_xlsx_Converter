using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<FileData> FilesToConvert { get; set; } = new ObservableCollection<FileData>();
    }

    [AddINotifyPropertyChangedInterface]
    public class FileData : INotifyPropertyChanged
    {
        public string ConversionStatus { get; set; } = "";
        public FileInfo FileDetails { get; set; }

        private bool _IsIncluded;
        public bool IsIncluded
        {
            get { return _IsIncluded; }
            set
            {
                _IsIncluded = value;
                if(_IsIncluded)
                {
                    ConversionStatus = "Selected";
                }
                else
                {
                    ConversionStatus = "Not Selected";
                }

                RaisePropertyChanged("IsIncluded");
            }
        }

        public FileData(string path)
        {
            FileDetails = new FileInfo(path);
            ConversionStatus = "Selected";
            IsIncluded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    public static class FileConverionInfo
    {
        public static bool IsConvertingFiles { get; set; } = false;
        public static int TotalFilesToConvert { get; set; } = 0;
        public static int FilesConverted { get; set; } = 0;
    }
}
