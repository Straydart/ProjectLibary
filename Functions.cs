using System.Text.Json;
using System.IO;
using System.Windows.Controls;
using FontAwesome.WPF;
using System.Windows;
using ProjectDirectory.Models;

namespace ProjectDirectory
{
    public class Functions
    {
        public string ReadFileContent(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            File.Create(filePath).Dispose();
            return string.Empty;
        }

        public static void AddToJson(string filename, string filepath, string targetJson)
        {
            var databaseJsonString = "";
            var Buttonsafe = new ButtonSafe()
            {
                ProjectName = filename.Replace(".sln", ""),
                FilePath = filepath,
            };

            databaseJsonString = File.ReadAllText(targetJson);

            List<ButtonSafe> databaseList;

            // Check if databaseJsonString is empty
            if (string.IsNullOrWhiteSpace(databaseJsonString))
            {
                // If empty, initialize an empty list
                databaseList = new List<ButtonSafe>();
            }
            else
            {
                // Deserialize the JSON array into a list of ButtonSafe objects
                databaseList = JsonSerializer.Deserialize<List<ButtonSafe>>(databaseJsonString);
            }

            // Add the newly created ButtonSafe object to the list
            if (!databaseJsonString.Contains(Buttonsafe.ProjectName) && !databaseJsonString.Contains(Buttonsafe.FilePath))
            {
                databaseList.Add(Buttonsafe);
            }

            // Serialize the list back to JSON
            var combinedJsonString = JsonSerializer.Serialize(databaseList);

            // Write the updated JSON back to database.json
            File.WriteAllText(targetJson, combinedJsonString);
        }
        public void SetVisibility(ImageAwesome element, Visibility visibility, View.MainWindow window)
        {
            element.Visibility = visibility;
            window.OnPropertyChanged();
        }

        public Visibility GetVisibility(ImageAwesome element)
        {
            return element.Visibility;
        }
        public void EnsureRequiredFilesExist()
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
        public void LoadExeJson(View.MainWindow window)
        {
            try
            {
                string jsonFilePath = "AutobuildJsons/ExecutionDatabase.json";
                if (File.Exists(jsonFilePath))
                {
                    string jsonContent = ReadFileContent(jsonFilePath);

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
                            window.VisualStudio.Items.Add(comboBoxItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static async Task<List<(string FileName, string FilePath)>> GetFiles(string path, string searchPattern)
        {
            List<(string FileName, string FilePath)> files = new List<(string FileName, string FilePath)>();
            try
            {

                try
                {
                    foreach (string file in Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly))
                    {
                        files.Add((Path.GetFileName(file), file));
                    }

                    foreach (string directory in Directory.GetDirectories(path))
                    {
                        try
                        {
                            files.AddRange(await GetFiles(directory, searchPattern));
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Console.WriteLine("Access denied to directory: " + directory);
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Access denied to path: " + path);
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory not found: " + path);
            }

            return files;
        }

    }
}
