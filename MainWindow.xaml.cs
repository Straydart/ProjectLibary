using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.FunctionButtons;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        Functions functions = new Functions();
        public MainWindow()
        {
            InitializeComponent();
            Create create = new Create(this);
            create.CreateContent();
        }
        private void FindVS_Click(object sender, RoutedEventArgs e)
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    var file = functions.GetFiles(drive.Name, "*devenv.exe");
                    foreach (var f in file)
                    {
                        ListBoxItem listBoxItem = new ListBoxItem();
                        listBoxItem.Name = f.FilePath.Split('\\')[4]+f.FilePath.Split('\\')[3];
                        listBoxItem.Tag = f.FilePath;
                        listBoxItem.Content = f.FilePath.Split('\\')[2]+" "+f.FilePath.Split('\\')[3]+" "+f.FilePath.Split('\\')[4];
                        VisualStudio.Items.Add(listBoxItem);
                    }
                }
            }
        }
        
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in MainGrid.Children)
            {
                Button button = new Button();
                if (item.GetType() == typeof(Button))
                {
                    button = (Button)item;
                    if (button.Name.ToLower() == "slnbutton")
                    {
                        if (button.Content.ToString().Contains(Searchbar.Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            button.Background = Brushes.Yellow;
                        }
                        else
                        {
                            button.Background = Brushes.LightGray;
                        }
                    }

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var addSingleSolution = new AddSingleSolution();
            var create = new Create(this);
            addSingleSolution.addSingleSolution(sender, e);
            create.CreateContent();
        }
        private void AddEverything_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            var create = new Create(this);
            var addAllSolutions = new AddAllSolutions();
            addAllSolutions.addAllSolutions();
            create.CreateContent();
            this.IsEnabled = true;
        }
        public void Open_Sln(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (VisualStudio.SelectedItem is ListBoxItem selectedItem)
            {
                string filePath = selectedItem.Tag.ToString();
                string arguments = button.Tag.ToString();

                using var process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = filePath,
                        Arguments = arguments,
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
            if (VisualStudio.SelectedItem is ListBoxItem selectedItem)
            {
                var tag = selectedItem.Tag;
                MessageBox.Show($"Selected Tag: {tag}");
            }
        }

    }
}
