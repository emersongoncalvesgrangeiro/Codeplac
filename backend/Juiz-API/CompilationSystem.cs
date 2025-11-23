using System.Diagnostics;

namespace CompilationSystem
{
  public class CompilationSystem
  {
    public string output = "", error = "";
    public CompilationSystem()
    {

    }
    public async Task CreateANewDiretory(string name, int reg)
    {
      try
      {
        string path = $"/CodeData/${name}";
        Console.WriteLine(path + "\n");
        if (!Directory.Exists(path))
        {
          Directory.CreateDirectory(path);
          await Checker(path);
        }
        else
        {
          await Checker(path);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine($"Exeption Error: ${e.Message}");
      }
    }
    private async Task Checker(string path)
    {
      string[] archives = Directory.GetFiles(path);
      string compilationinfo = "";
      switch (Path.GetExtension(archives[0]))
      {
        case ".java":
          compilationinfo = "java";
          break;
        case ".c":
          compilationinfo = "c";
          break;
        case ".h":
          compilationinfo = "c";
          break;
        default:
          break;
      }
      await Compiler(archives, path, compilationinfo);
    }
    private async Task Compiler(string[] archives, string path, string type)
    {
      var process = new ProcessStartInfo();
      string archivelist = "";
      for (int i = 0; i < archives.Length; i++)
      {
        archivelist = archivelist + " " + archives[i];
      }
      process.FileName = "/bin/bash";
      process.RedirectStandardOutput = true;
      process.RedirectStandardError = true;
      process.UseShellExecute = false;
      process.CreateNoWindow = true;
      try
      {
        if (type == "java")
        {
          process.Arguments = $"-c \"cd /${path}/ && javac *.java -d out\"";
        }
        else if (type == "c")
        {
          process.Arguments = $"-c \"mv main.c /${path} && cd /${path}/ && gcc ${archivelist} -o out\"";
        }
        using var processstart = Process.Start(process);
        output = processstart.StandardOutput.ReadToEnd();
        error = processstart.StandardError.ReadToEnd();
        await processstart.WaitForExitAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine($"Error: ${e.Message}");
      }
    }
  }
}