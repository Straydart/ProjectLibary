using System.IO;

namespace WpfApp1.FunctionButtons
{
    internal class AddAllSolutions
    {
        Functions functions = new Functions();
        public async void addAllSolutions()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    List<(string FileName, string FilePath)> slnFiles = new List<(string FileName, string FilePath)>();
                    try
                    {
                        slnFiles.AddRange(await functions.GetFiles(drive.ToString(), "*.sln"));
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }

                    foreach (var file in slnFiles)
                    {
                        await functions.AddToJson(file.FileName, file.FilePath, "AutobuildJsons/SolutionDatabase.json");
                    }
                }
            }
        }
    }
}
