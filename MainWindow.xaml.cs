using FontAwesome.WPF;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.FunctionButtons;

namespace WpfApp1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly Functions _functions = new Functions();


        public MainWindow()
        {
            EnsureRequiredFilesExist();
            InitializeComponent();
            InitializeContent();
        }

        private void EnsureRequiredFilesExist()
        {
            string[] requiredFiles =
            {
                "AutobuildJsons/SolutionDatabase.json",
                "AutobuildJsons/ExecutionDatabase.json"
            };

            foreach (var file in requiredFiles)
            {
                if (!File.Exists(file))
                {
                    if (!Directory.Exists(file))
                    {
                        Directory.CreateDirectory(file.Split('/')[0]);
                    }
                    File.Create(file).Dispose();

                }
            }
        }

        private async void InitializeContent()
        {
            var create = new Create(this);
            await create.CreateContentAsync();
            LoadExeJson();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetVisibility(ImageAwesome element, Visibility visibility)
        {
            element.Visibility = visibility;
            OnPropertyChanged();
        }

        public Visibility GetVisibility(ImageAwesome element)
        {
            return element.Visibility;
        }

        private async void FindVS_Click(object sender, RoutedEventArgs e)
        {
            SetVisibility(LoadingImage, Visibility.Visible);

            var filePathList = await Task.Run(async () =>
            {
                var filePaths = new List<string>();
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        var files = await _functions.GetFiles(drive.Name, "*devenv.exe");
                        filePaths.AddRange(files.Select(file => file.FilePath));
                    }
                }
                return filePaths;
            });

            AddToComboBox(filePathList);
            SetVisibility(LoadingImage, Visibility.Hidden);
        }

        private void AddToComboBox(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var fileParts = file.Split('\\');
                var item = new ComboBoxItem
                {
                    Name = $"{fileParts[4]}{fileParts[3]}",
                    Tag = file,
                    Content = $"{fileParts[2]} {fileParts[3]} {fileParts[4]}"
                };
                _functions.AddToJson(item.Content.ToString(), item.Tag.ToString(), "AutobuildJsons/ExecutionDatabase.json");
            }
            LoadExeJson();
        }
        public void LoadExeJson()
        {
            try
            {

                string jsonFilePath = "AutobuildJsons/ExecutionDatabase.json";
                if (File.Exists(jsonFilePath))
                {
                    string jsonContent = _functions.ReadFileContent(jsonFilePath);

                    if (!string.IsNullOrEmpty(jsonContent))
                    {

                        var executablesList = JsonSerializer.Deserialize<List<ButtonSafe>>(jsonContent);

                        foreach (var executable in executablesList)
                        {
                            var comboBoxItem = new ComboBoxItem
                            {
                                Content = executable.ProjectName,
                                Tag = executable.FilePath,
                            };
                            VisualStudio.Items.Add(comboBoxItem);
                        }
                    }
                }
            }catch (Exception ex)
            {

            }
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var addSingleSolution = new AddSingleSolution();
            addSingleSolution.addSingleSolution(sender, e);

            var create = new Create(this);
            await create.CreateContentAsync();
        }

        private async void AddEverything_Click(object sender, RoutedEventArgs e)
        {
            SetVisibility(AddEverythingLoading, Visibility.Visible);

            var addAllSolutions = new AddAllSolutions();
            await Task.Run(() => addAllSolutions.addAllSolutions());

            var create = new Create(this);
            await create.CreateContentAsync();

            SetVisibility(AddEverythingLoading, Visibility.Hidden);
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
