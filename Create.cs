using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using WpfApp1;

namespace WpfApp1
{
    public class Create
    {
        
        public MainWindow window;
        public Create(MainWindow window) {
            this.window = window;
        }
        public void CreateContent()
        {
            string databaseJsonString = "";
            if (File.Exists(@"database.json"))
            {
                databaseJsonString = File.ReadAllText(@"database.json");
            }
            else 
            {
                File.Create("database.json");
            }
            // Check if databaseJsonString is empty
            if (string.IsNullOrWhiteSpace(databaseJsonString))
            {
                // If empty, there are no buttons to load
                return;
            }

            // Deserialize the JSON array into a list of ButtonSafe objects
            var databaseList = JsonSerializer.Deserialize<List<ButtonSafe>>(databaseJsonString);
            var row = 1;
            var column = 0;
            // Iterate over each ButtonSafe object and create a button for it
            foreach (var buttonSafe in databaseList)
            {
                CreateSlnButton(buttonSafe.ProjectName, buttonSafe.FilePath, row, column);
                column++;
                if (column/6 == 1)
                {
                    RowDefinition newRow = new RowDefinition();
                    newRow.Height = new GridLength(100);
                    window.MainGrid.RowDefinitions.Add(newRow);
                    column = 0;
                    row++;
                }
            }
        }
        private void CreateSlnButton(string projectName, string filePath, int row, int column)
        {
            // Create a button for the selected solution
            Button button = new Button();
            button.Name = "SlnButton";
            button.Content = projectName;
            button.Tag = filePath;
            button.Click += window.Open_Sln;
            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);
            
            // Add the button to the main grid
            window.MainGrid.Children.Add(button);
        }
    }
}
