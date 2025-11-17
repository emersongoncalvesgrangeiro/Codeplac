using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CompilationSystem
{
  public class CompilationSystem
  {
    public string output, error;
    public CompilationSystem()
    {
      string path = "./CodeData/";
      try
      {
        if (!Directory.Exists(path))
        {
          Directory.CreateDirectory(path);
          //CreateANewDiretory();
        }
        else
        {

        }
      }
      catch (Exception e)
      {
        Console.WriteLine($"Exeption Error: ${e.Message}");
      }
    }
    private void CreateANewDiretory(string name, int reg)
    {
      try
      {
        string path = name + reg;
        if (!Directory.Exists(path))
        {
          Directory.CreateDirectory(path);
        }
        else
        {
          Checker(path);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine($"Exeption Error: ${e.Message}");
      }
    }
    private void Checker(string path)
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
      Compiler(archives, path, compilationinfo);
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
          process.Arguments = $"-c cd /${path}/ && javac -d out ${archivelist}";
        }
        else if (type == "c")
        {
          process.Arguments = $"-c cd /${path}/ && gcc ${archivelist} -o out";
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