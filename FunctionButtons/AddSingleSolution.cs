using Microsoft.Win32;
using System.Windows;

namespace WpfApp1.FunctionButtons
{
    internal class AddSingleSolution : IFuctionButtons
    {
        public void addSingleSolution(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.FileName = ""; // Default file name
            dialog.DefaultExt = ".sln"; // Default file extension
            dialog.Filter = "Visual Studio Solution (.sln)|*.sln"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                Functions functions = new Functions();
                functions.AddToJson(dialog.SafeFileName, dialog.FileName);
            }
        }
    }
}
