using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using PropertyChanged;
using xls_To_xlsx_Converter.ViewModel;

namespace xls_To_xlsx_Converter.Model
{
    public class FileConverterModel { }

    [AddINotifyPropertyChangedInterface]
    public class FileConverter
    {
        public ObservableCollection<FileData> FilesToConvert { get; set; } = new ObservableCollection<FileData>();
        public int SelectedBannerIndex { get; set; } = 1;
        public string InfoBannerText { get; set; } = "Info Banner Text";
        public int InfoBannerProgress { get; set; } = 0;
        public bool IsExpandedInfoBanner { get; set; } = false;
        public bool IsExpandedDialogBanner { get; set; } = false;
        public bool AltBannerExpanded { get; set; } = false;
        public string DialogBannerText { get; set; } = "Dialog Banner Text";
        public bool IsRecursiveSearch { get; set; } = false;

        public bool FilesNotSelected { get; set; } = false;

        public void ShowInfoBanner(DispatcherTimer infoBannerTimer, string NewText)
        {
            infoBannerTimer.Stop();
            if(IsExpandedDialogBanner) { AltBannerExpanded = true; }
            SelectedBannerIndex = 0;
            InfoBannerText = NewText;
            InfoBannerProgress = 0;
            IsExpandedDialogBanner = false;
            IsExpandedInfoBanner = true;
            infoBannerTimer.Start();
        }

        public void ShowDialogBanner(string NewText)
        {
            if(IsExpandedInfoBanner) { AltBannerExpanded = true; }
            SelectedBannerIndex = 1;
            DialogBannerText = NewText;
            IsExpandedInfoBanner = false;
            IsExpandedDialogBanner = true;
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class FileData : INotifyPropertyChanged
    {
        public string ConversionStatus { get; set; } = "";
        public FileInfo FileDetails { get; set; }
        public bool MarkedForRemoval { get; set; } = false;

        private bool _IsIncluded;
        public bool IsIncluded
        {
            get { return _IsIncluded; }
            set
            {
                _IsIncluded = value;
                
                if (_IsIncluded)
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
    public class ConvertionData
    {
        public bool IsConvertingFiles { get; set; } = false;
        public bool EnableControls { get; set; } = true;
        public int TotalFilesToConvert { get; set; } = 0;
        public int FilesConverted { get; set; } = 0;
        public int ConversionProgress { get; set; } = 0;
    }

    [AddINotifyPropertyChangedInterface]
    public static class AdditionalStaticData
    {
        public static int BannerIndex { get; set; } = 1;
        public static ObservableCollection<string> UnprocessedPaths { get; set; } = new ObservableCollection<string>();
        public static int ProcessedPathsCount { get; set; } = 0;
        public static int ExistingFileCount { get; set; } = 0;
        public static int NonXLSFileCount { get; set; } = 0;
        public static string SearchDir { get; set; } = "";
    }


}
