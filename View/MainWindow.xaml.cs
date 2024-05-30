using ProjectDirectory.PluginCore;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectDirectory.View
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly Functions _functions = new Functions();


        public MainWindow()
        {
            _functions.EnsureRequiredFilesExist();
            InitializeComponent();
        }

    
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {            
            foreach (var item in MainGrid.Children.OfType<Button>())
            {
                if (item.Name.Equals("slnbutton", StringComparison.OrdinalIgnoreCase))
                {
                    item.Background = item.Content.ToString().Contains(Searchbar.Text, StringComparison.CurrentCultureIgnoreCase)
                        ? Brushes.Yellow
                        : Brushes.LightGray;
                }
            }
        }

        public void Open_Sln(object sender, RoutedEventArgs e)
        {
            if (VisualStudio.SelectedItem is ComboBoxItem selectedItem)
            {
                string filePath = selectedItem.Tag.ToString();
                string arguments = ((Button)sender).Tag.ToString();

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = filePath,
                        Arguments = arguments
                    }
                };
                process.Start();
            }
            else
            {
                MessageBox.Show("No solution selected.");
            }
        }

        private void VisualStudio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VisualStudio.SelectedItem is ComboBoxItem selectedItem)
            {
                MessageBox.Show($"Selected Tag: {selectedItem.Tag}");
            }
        }
    }
}
