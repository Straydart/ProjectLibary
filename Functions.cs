using System.Text.Json;
using System.IO;
using System.Windows.Controls;

namespace WpfApp1
{
    public class Functions
    {
        public void AddToJson(string filename, string filepath)
        {
            var Buttonsafe = new ButtonSafe()
            {
                ProjectName = filename.Replace(".sln", ""),
                FilePath = filepath,
            };

            // Read the existing JSON data from database.json
            var databaseJsonString = File.ReadAllText(@"database.json");

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
            File.WriteAllText(@"database.json", combinedJsonString);
        }

        public async Task<List<(string FileName, string FilePath)>> GetFiles(string path, string searchPattern)
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
