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

namespace xls_To_xlsx_Converter.ViewModel
{
    public class FileConverterViewModel : IFileDragDropTarget
    {
        private NavigationViewModel _navigationViewModel { get; set; }
        public AwaitableDelegateCommand ConvertFilesCommand { get; set; }
        public FileConverter fileConverter { get; set; }

        public FileConverterViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            ConvertFilesCommand = new AwaitableDelegateCommand(onConvertFilesCommand, canConvertFilesCommand);
            initFileCoverter();
        }

        public void initFileCoverter()
        {
            FileConverter tempFC = new FileConverter();

            fileConverter = tempFC;
        }

        public void OnFileDrop(string[] filePaths)
        {
            foreach(string path in filePaths)
            {
                if (Directory.Exists(path))
                {
                    
                    SearchOption SOption = SearchOption.AllDirectories;
                    string[] xlsFiles = Directory.GetFiles(path, "*.xls", SOption);
                    for(int i = 0; i < xlsFiles.Count(); i++)
                    {
                        fileConverter.FilesToConvert.Add(new FileData(xlsFiles[i]));
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
                                //maybe I'll make this a timed banner at the top of the interface. idk...
                                MessageBox.Show("File already added to list");
                            }
                        }
                    }
                    else
                    {
                        if (filePaths.Count() == 1)
                        {
                            //maybe I'll make this a timed banner at the top of the interface. idk...
                            MessageBox.Show("File or folder did not contain any xls files", "Nothing to do here", MessageBoxButton.OK);
                        }
                    }
                }
            }
        }

        public async Task onConvertFilesCommand()
        {
            //just testing this banner view switching stuff.
            fileConverter.testint = 1;
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
