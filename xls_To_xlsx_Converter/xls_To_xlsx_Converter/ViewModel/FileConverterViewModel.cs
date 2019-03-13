using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xls_To_xlsx_Converter.ViewModel;
using xls_To_xlsx_Converter.Model;
using xls_To_xlsx_Converter.Helpers;
using System.Windows;

namespace xls_To_xlsx_Converter.ViewModel
{
    public class FileConverterViewModel : IFileDragDropTarget
    {
        private NavigationViewModel _navigationViewModel { get; set; }

        public FileConverter fileConverter { get; set; }

        public FileConverterViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            initFileCoverter();
        }

        public void initFileCoverter()
        {
            FileConverter tempFC = new FileConverter();

            fileConverter = tempFC;
        }

        public void OnFileDrop(string[] filePaths)
        {
            string message = string.Empty;
            foreach(string path in filePaths)
            {
                message += path.ToString() + "\n";
            }

            MessageBox.Show(message);
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
