﻿using System;
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

        public string DialogBannerText { get; set; } = "Dialog Banner Text";
        public bool IsRecursiveSearch { get; set; } = false;

        public void ShowInfoBanner(DispatcherTimer infoBannerTimer, string NewText)
        {
            infoBannerTimer.Stop();
            SelectedBannerIndex = 0;
            InfoBannerText = NewText;
            InfoBannerProgress = 0;
            IsExpandedInfoBanner = true;
            infoBannerTimer.Start();
        }

        public void ShowDialogBanner(string NewText)
        {
            SelectedBannerIndex = 1;
            DialogBannerText = NewText;
            IsExpandedDialogBanner = true;
        }
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
    public static class AdditionalStaticData
    {
        public static bool IsConvertingFiles { get; set; } = false;
        public static int TotalFilesToConvert { get; set; } = 0;
        public static int FilesConverted { get; set; } = 0;
        public static int BannerIndex { get; set; } = 1;
        public static ObservableCollection<string> UnprocessedPaths { get; set; } = new ObservableCollection<string>();
        public static int ExistingFileCount { get; set; } = 0;
        public static string SearchDir { get; set; } = "";
    }


}
