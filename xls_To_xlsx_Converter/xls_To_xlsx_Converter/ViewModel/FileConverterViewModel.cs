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

namespace xls_To_xlsx_Converter.ViewModel
{
    public class FileConverterViewModel : IFileDragDropTarget
    {
        private NavigationViewModel _navigationViewModel { get; set; }
        public AwaitableDelegateCommand ConvertFilesCommand { get; set; }
        public FileConverter fileConverter { get; set; }
        public DispatcherTimer InfoBannerTimer = new DispatcherTimer();

        public FileConverterViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            ConvertFilesCommand = new AwaitableDelegateCommand(onConvertFilesCommand, canConvertFilesCommand);
            InfoBannerTimer.Interval = TimeSpan.FromMilliseconds(50);
            InfoBannerTimer.Tick += InfoBannerTimer_Tick;
            initFileCoverter();
        }

        private void InfoBannerTimer_Tick(object sender, EventArgs e)
        {
            fileConverter.InfoBannerProgress += 1;

            if(fileConverter.InfoBannerProgress == 100)
            {
                InfoBannerTimer.Stop();
                fileConverter.InfoBannerProgress = 0;
                fileConverter.BannerExpanded = false;
            }
        }

        public void initFileCoverter()
        {
            FileConverter tempFC = new FileConverter();

            fileConverter = tempFC;
        }

        public void OnFileDrop(string[] filePaths)
        {
            int ExistingFileCount = 0;
            foreach(string path in filePaths)
            {
                if (Directory.Exists(path))
                {
                    //open the dialog banner to get info about recursion first


                    SearchOption SOption = SearchOption.AllDirectories;
                    string[] xlsFiles = Directory.GetFiles(path, "*.xls", SOption);
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
                                ExistingFileCount += 1;
                            }
                        }
                    }
                    else
                    {
                        if (filePaths.Count() == 1)
                        {
                            fileConverter.ShowInfoBanner(InfoBannerTimer, "Directory did not contain any XLS files");
                        }
                    }
                }
                else
                {
                    if (File.Exists(path) && path.EndsWith(".xls"))
                    {
                        if(fileConverter.FilesToConvert.Where(x => x.FileDetails.FullName == path).Count() == 0)
                        {
                            fileConverter.FilesToConvert.Add(new FileData(path));
                        }
                        else
                        {
                            if(filePaths.Count() == 1)
                            {
                                fileConverter.ShowInfoBanner(InfoBannerTimer, "File already added to list");
                            }
                            else
                            {
                                ExistingFileCount += 1;
                            }
                        }
                    }
                    else
                    {
                        if (filePaths.Count() == 1)
                        {
                            fileConverter.ShowInfoBanner(InfoBannerTimer, "File is not an XLS file");
                        }
                    }
                }
            }

            if (ExistingFileCount > 0)
            {
                fileConverter.ShowInfoBanner(InfoBannerTimer, $"{ExistingFileCount.ToString()} files were already found in the list and have not been added.");
            }
        }

        public async Task onConvertFilesCommand()
        {
            
        }

        public bool canConvertFilesCommand()
        {
            return true; //this is temporary
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
