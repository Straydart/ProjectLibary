using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class Create
    {
        private readonly MainWindow _window;

        public Create(MainWindow window)
        {
            _window = window;
        }

        public async Task CreateContentAsync()
        {
            var functions = new Functions();
          
                string jsonContent = functions.ReadFileContent("AutobuildJsons/SolutionDatabase.json");

                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    return;
                }

                var buttonsData = JsonSerializer.Deserialize<List<ButtonSafe>>(jsonContent);
                GenerateButtons(buttonsData);
            
        }

        private void GenerateButtons(List<ButtonSafe> buttonsData)
        {
            int row = 1;
            int column = 0;

            foreach (var buttonData in buttonsData)
            {
                CreateButton(buttonData.ProjectName, buttonData.FilePath, row, column);

                column++;
                if (column == 6)
                {
                    AddNewRow();
                    column = 0;
                    row++;
                }
            }
        }

        private void CreateButton(string projectName, string filePath, int row, int column)
        {
            var button = new Button
            {
                Name = "SlnButton",
                Content = projectName,
                Tag = filePath
            };
            button.Click += _window.Open_Sln;

            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);
            _window.MainGrid.Children.Add(button);
        }

        private void AddNewRow()
        {
            var newRow = new RowDefinition
            {
                Height = new GridLength(100)
            };
            _window.MainGrid.RowDefinitions.Add(newRow);
        }
    }
}
