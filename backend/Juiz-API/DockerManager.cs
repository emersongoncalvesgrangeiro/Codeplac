using System.Diagnostics;

namespace Manager
{
  public class DockerManager
  {
    public DockerManager()
    {

    }
    private async Task MakeContainer(string type)
    {
      var processstart = new ProcessStartInfo();
      processstart.FileName = "/bin/bash";
      processstart.RedirectStandardOutput = true;
      processstart.RedirectStandardError = true;
      processstart.UseShellExecute = false;
      processstart.CreateNoWindow = true;
      try
      {
        if (type == "java")
        {
          processstart.Arguments = $"-c \"docker build -t javarun template/java/Dockerfile\"";
        }
        else if (type == "c")
        {
          processstart.Arguments = $"-c \"docker build -t javarun template/c/Dockerfile\"";
        }
        using var process = Process.Start(processstart);
        await process.WaitForExitAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine($"Error: ${e.Message}");
      }
    }
  }
}