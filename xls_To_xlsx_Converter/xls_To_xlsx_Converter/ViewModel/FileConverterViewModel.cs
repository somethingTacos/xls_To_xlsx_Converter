using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xls_To_xlsx_Converter.ViewModel;
using xls_To_xlsx_Converter.Model;
using xls_To_xlsx_Converter.Helpers;
using System.Windows;
using System.IO;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace xls_To_xlsx_Converter.ViewModel
{
    public class FileConverterViewModel : IFileDragDropTarget
    {
        private NavigationViewModel _navigationViewModel { get; set; }
        public AwaitableDelegateCommand ConvertFilesCommand { get; set; }
        public MyICommand StartDirectorySerachCommand { get; set; }
        public MyICommand CancelSearchCommand { get; set; }
        public MyICommand CloseInfoBannerCommand { get; set; }
        public MyICommand RemoveListedFileCommand { get; set; }
        public MyICommand RemoveAllNonSelectedListedFilesCommand { get; set; }
        public MyICommand UpdateFilesNotSelectedCommand { get; set; }

        public FileConverter fileConverter { get; set; }
        public DispatcherTimer InfoBannerTimer = new DispatcherTimer();
        public DispatcherTimer RemovalAnimationTimer = new DispatcherTimer();
        public ConvertionData convertionData { get; set; }

        public FileConverterViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            ConvertFilesCommand = new AwaitableDelegateCommand(onConvertFilesCommand, canConvertFilesCommand);
            StartDirectorySerachCommand = new MyICommand(onStartDirectorySerachCommand);
            CloseInfoBannerCommand = new MyICommand(onCloseInfoBannerCommand);
            CancelSearchCommand = new MyICommand(onCancelSearchCommand);
            RemoveListedFileCommand = new MyICommand(onRemoveListedFileCommand);
            RemoveAllNonSelectedListedFilesCommand = new MyICommand(onRemoveAllNonSelectedListedFilesCommand);
            UpdateFilesNotSelectedCommand = new MyICommand(onUpdateFilesNotSelectedCommand);
            RemovalAnimationTimer.Interval = TimeSpan.FromMilliseconds(50);
            RemovalAnimationTimer.Tick += RemovalAnimationTimer_Tick;
            InfoBannerTimer.Interval = TimeSpan.FromMilliseconds(50);
            InfoBannerTimer.Tick += InfoBannerTimer_Tick;
            initFileCoverter();
        }

        private void RemovalAnimationTimer_Tick(object sender, EventArgs e)
        {
            if (fileConverter.FilesToConvert.Where(x => !x.IsIncluded && !x.MarkedForRemoval).Count() > 0)
            {
                FileData fd = fileConverter.FilesToConvert.Where(x => !x.IsIncluded && !x.MarkedForRemoval).FirstOrDefault();
                fd.MarkedForRemoval = true;
            }
            else
            {
                fileConverter.FilesNotSelected = false;
                List<FileData> tempFD = new List<FileData>(fileConverter.FilesToConvert);
                foreach(FileData fd in tempFD)
                {
                    if(fd.MarkedForRemoval)
                    {
                        fileConverter.FilesToConvert.Remove(fd);
                    }
                }
                RemovalAnimationTimer.Stop();
            }

        }

        private void InfoBannerTimer_Tick(object sender, EventArgs e)
        {
            fileConverter.InfoBannerProgress += 1;

            if(fileConverter.InfoBannerProgress == 100)
            {
                CloseInfoBanner();
            }
        }

        public void initFileCoverter()
        {
            FileConverter tempFC = new FileConverter();
            ConvertionData tempCD = new ConvertionData();


            fileConverter = tempFC;
            convertionData = tempCD;
        }

        private void ProcessPaths(ObservableCollection<string> filePaths)
        {
            foreach (string path in filePaths)
            {
                if(AdditionalStaticData.UnprocessedPaths.Contains(path))
                {
                   AdditionalStaticData.UnprocessedPaths.Remove(path);
                }

                if (Directory.Exists(path))
                {
                    AdditionalStaticData.SearchDir = path;

                    string[] pathList = path.Split('\\');
                    string dirName = pathList.Last<string>();
                    InfoBannerTimer.Stop();
                    fileConverter.ShowDialogBanner($"Choose how to search directory '{dirName}' for files.");
                    return;
                }
                else
                {
                    if (File.Exists(path) && path.EndsWith(".xls"))
                    {
                        if (fileConverter.FilesToConvert.Where(x => x.FileDetails.FullName == path).Count() == 0)
                        {
                            fileConverter.FilesToConvert.Add(new FileData(path));
                        }
                        else
                        {
                            if (filePaths.Count() == 1 && AdditionalStaticData.ExistingFileCount == 1)
                            {
                                fileConverter.ShowInfoBanner(InfoBannerTimer, "File already added to list");
                            }
                            else
                            {
                                AdditionalStaticData.ExistingFileCount += 1;
                            }
                        }
                    }
                    else
                    {
                        if (filePaths.Count() == 1 && AdditionalStaticData.ExistingFileCount == 0)
                        {
                            fileConverter.ShowInfoBanner(InfoBannerTimer, "File is not an XLS file");
                        }
                    }
                }
            }

            if (AdditionalStaticData.UnprocessedPaths.Count() == 0)
            {
                if (AdditionalStaticData.ExistingFileCount > 0)
                {
                    fileConverter.ShowInfoBanner(InfoBannerTimer, $"{AdditionalStaticData.ExistingFileCount.ToString()} files were already found in the list and have not been added.");
                    AdditionalStaticData.ExistingFileCount = 0;
                }
            }
        }

        public void OnFileDrop(string[] filePaths)
        {
            ObservableCollection<string> paths = new ObservableCollection<string>();

            for(int i = 0; i < filePaths.Count(); i++)
            {
                paths.Add(filePaths[i]);
            }

            AdditionalStaticData.UnprocessedPaths = new ObservableCollection<string>(paths);
            ProcessPaths(paths);
        }

        public void onRemoveListedFileCommand(object parameter)
        {
            if (parameter is FileData fileData)
            {
                if(fileConverter.FilesToConvert.Contains(fileData))
                {
                    fileConverter.FilesToConvert.Remove(fileData);
                }
            }
        }

        private void onRemoveAllNonSelectedListedFilesCommand(object parameter)
        {
            RemovalAnimationTimer.Start();
        }

        public void onCloseInfoBannerCommand(object parameter)
        {
            CloseInfoBanner();
        }

        private void CloseInfoBanner()
        {
            InfoBannerTimer.Stop();
            fileConverter.InfoBannerProgress = 0;
            fileConverter.IsExpandedInfoBanner = false;
            fileConverter.IsExpandedDialogBanner = false;
            fileConverter.AltBannerExpanded = false;
        }

        public void onUpdateFilesNotSelectedCommand(object parameter)
        {
            fileConverter.FilesNotSelected = fileConverter.FilesToConvert.Where(x => x.IsIncluded == false).Count() > 0;
        }

        public async Task onConvertFilesCommand()
        {
            convertionData.IsNotConvertingFiles = false;
            RemovalAnimationTimer.Start();
            //want to close removal buttons and selection checkboxes as soon as the conversion starts.
        }

        public bool canConvertFilesCommand()
        {
            return true; //this is temporary
        }

        public void onStartDirectorySerachCommand(object parameter)
        {
            fileConverter.IsExpandedDialogBanner = false;
            fileConverter.AltBannerExpanded = false;
            fileConverter.IsExpandedInfoBanner = false;

            SearchOption SOption = SearchOption.TopDirectoryOnly;

            if (fileConverter.IsRecursiveSearch)
            {
               SOption = SearchOption.AllDirectories;
            }

            string[] xlsFiles = Directory.GetFiles(AdditionalStaticData.SearchDir, "*.xls", SOption);
            if (xlsFiles.Count() > 0)
            {
                for (int i = 0; i < xlsFiles.Count(); i++)
                {
                    if (fileConverter.FilesToConvert.Where(x => x.FileDetails.FullName == xlsFiles[i]).Count() == 0)
                    {
                        fileConverter.FilesToConvert.Add(new FileData(xlsFiles[i]));
                    }
                    else
                    {
                        AdditionalStaticData.ExistingFileCount += 1;
                    }
                }
            }
            else
            {
                if (AdditionalStaticData.UnprocessedPaths.Count() == 0)
                {
                    fileConverter.ShowInfoBanner(InfoBannerTimer, "Directory did not contain any XLS files");
                }
            }

            if (AdditionalStaticData.UnprocessedPaths.Contains(AdditionalStaticData.SearchDir))
            {
                AdditionalStaticData.UnprocessedPaths.Remove(AdditionalStaticData.SearchDir);
            }

            if (AdditionalStaticData.UnprocessedPaths.Count() > 0)
            {
                ProcessPaths(new ObservableCollection<string>(AdditionalStaticData.UnprocessedPaths));
            }
            else
            {
                if (AdditionalStaticData.ExistingFileCount > 0)
                {
                    fileConverter.ShowInfoBanner(InfoBannerTimer, $"{AdditionalStaticData.ExistingFileCount.ToString()} files were already found in the list and have not been added.");
                    AdditionalStaticData.ExistingFileCount = 0;
                }
            }
        }

        public void onCancelSearchCommand(object parameter)
        {
            fileConverter.IsExpandedDialogBanner = false;

            if (AdditionalStaticData.UnprocessedPaths.Contains(AdditionalStaticData.SearchDir))
            {
                AdditionalStaticData.UnprocessedPaths.Remove(AdditionalStaticData.SearchDir);
            }

            if (AdditionalStaticData.UnprocessedPaths.Count() > 0)
            {
                ProcessPaths(new ObservableCollection<string>(AdditionalStaticData.UnprocessedPaths));
            }
        }
    }
}


// I'm being lazy, I'll deal with this later...
//static void Main(string[] args)
//{
//    Console.WriteLine("Enter a file path:");
//    string path = Console.ReadLine();

//    FileInfo file = new FileInfo(path);

//    ConvertXLS_XLSX(file);
//}

//public static string ConvertXLS_XLSX(FileInfo file)
//{
//    var app = new Microsoft.Office.Interop.Excel.Application();
//    var xlsFile = file.FullName;
//    var wb = app.Workbooks.Open(xlsFile);
//    var xlsxFile = xlsFile + "x";
//    wb.SaveAs(Filename: xlsxFile, FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
//    wb.Close();
//    app.Quit();
//    return xlsxFile;
//}
