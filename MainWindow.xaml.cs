using FontAwesome.WPF;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using WpfApp1.FunctionButtons;


namespace WpfApp1
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Functions functions = new Functions();

        public void SetVisability(ref ImageAwesome element, Visibility visibility)
        {
            element.Visibility = visibility; 
            OnPropertyChanged();
        }


        public Visibility GetVisibility(ref ImageAwesome element)
        {
            return element.Visibility;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public MainWindow()
        {
            InitializeComponent();
            Create create = new Create(this);
            create.CreateContent();
        }


        private async void FindVS_Click(object sender, RoutedEventArgs e)
        {

            SetVisability(ref LoadingImage, Visibility.Visible);

            var filePathList = new List<string>();

            var enumTask = Task.Run(async () =>
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        var files = await functions.GetFiles(drive.Name, "*devenv.exe");
                        foreach (var file in files)
                        {
                            filePathList.Add(file.FilePath);
                        }
                    }
                }
            });
            await enumTask.ConfigureAwait(true);
            AddCombbox(filePathList);
            SetVisability(ref LoadingImage, Visibility.Hidden);

        }
        public void AddCombbox(IList<string> files)
        {
            foreach (var f in files)
            {
                var fileParts = f.Split('\\');
                ComboBoxItem item = new ComboBoxItem();
                item.Name = fileParts[4]+fileParts[3];
                item.Tag = f;
                item.Content = fileParts[2]+" "+fileParts[3]+" "+fileParts[4];
                VisualStudio.Items.Add(item);
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

        private async void AddEverything_Click(object sender, RoutedEventArgs e)
        {
            SetVisability(ref AddEverythingLoading, Visibility.Visible);


            var create = new Create(this);
            var addAllSolutions = new AddAllSolutions();
            var enumTask = Task.Run(async () =>
            {
                addAllSolutions.addAllSolutions();             
            });
            await enumTask.ConfigureAwait(true);
            create.CreateContent();

            SetVisability(ref AddEverythingLoading, Visibility.Hidden);
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
