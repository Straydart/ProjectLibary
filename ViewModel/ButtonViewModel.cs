using CommunityToolkit.Mvvm.ComponentModel;
using ProjectDirectory.Model;
using ProjectDirectory.Models;
using ProjectDirectory.PluginCore;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace ProjectDirectory.ViewModel
{
    public class ButtonViewModel : ObservableObject
    {
        public ObservableCollection<ButtonData> buttonDatas { get; set; } = new ObservableCollection<ButtonData>();
        public ObservableCollection<ButtonSafe> ExecutionDatabase { get; set; } = new ObservableCollection<ButtonSafe>();
        public ObservableCollection<ButtonSafe> SolutionDatabase { get; set; } = new ObservableCollection<ButtonSafe>();

        private PluginManager pluginManager = null;
        private Functions Functions = new Functions();

        public ButtonViewModel()
        {
            pluginManager = new PluginManager("plugins");
            foreach (var ele in pluginManager.CurrentPlugins)
            {
                ButtonData data = new ButtonData(ele);
                buttonDatas.Add(data);
            }

            try
            {
                string[] requiredFiles = {
                "AutobuildJsons/SolutionDatabase.json",
                "AutobuildJsons/ExecutionDatabase.json" };
                foreach (var jsonFilePath in requiredFiles)
                {
                    if (System.IO.File.Exists(jsonFilePath))
                    {
                        string jsonContent = Functions.ReadFileContent(jsonFilePath);

                        if (!string.IsNullOrEmpty(jsonContent))
                        {
                            var executablesList = JsonSerializer.Deserialize<List<ButtonSafe>>(jsonContent);

                            if (jsonFilePath.Contains("ExecutionDatabase"))
                            {
                                foreach (var executable in executablesList)
                                {
                                    ExecutionDatabase.Add(executable);
                                }
                            }
                            else if (jsonFilePath.Contains("SolutionDatabase"))
                            {
                                int row = 1;
                                int column = 0;
                                foreach (var executable in executablesList)
                                {
                                    if (column >= 6)
                                    {
                                        column = 0;
                                        row++;
                                    } 
                                    executable.Column = column;
                                    executable.Row = row;
                                    SolutionDatabase.Add(executable);
                                    column++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
