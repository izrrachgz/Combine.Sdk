using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Combine.Sdk.Diagnostics.Definitions.Models;

namespace Combine.Sdk.Diagnostics.Information
{
  /// <summary>
  /// Provides a mechanism to gather useful
  /// run time platform information
  /// </summary>
  public class Runtime
  {
    /// <summary>
    /// Indicates if the current OS platform is Unix based
    /// </summary>
    /// <returns>Boolean</returns>
    public static bool IsUnix()    
      => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);    

    /// <summary>
    /// Gets the os system memory usage information
    /// for the current platform
    /// </summary>
    /// <returns>MemoryMetrics</returns>
    public MemoryMetrics GetMetrics()
      => IsUnix() ? GetUnixMetrics() : GetWindowsMetrics();

    /// <summary>
    /// Gets the os system memory usage information
    /// for unix platform
    /// </summary>
    /// <returns>MemoryMetrics</returns>
    private MemoryMetrics GetUnixMetrics()
    {
      string output = "";
      ProcessStartInfo info = new ProcessStartInfo("free -m");
      info.FileName = "/bin/bash";
      info.Arguments = "-c \"free -m\"";
      info.RedirectStandardOutput = true;
      using (Process process = Process.Start(info))
      {
        output = process.StandardOutput.ReadToEnd();
        Console.WriteLine(output);
      }
      string[] lines = output.Split("\n");
      string[] memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
      MemoryMetrics metrics = new MemoryMetrics();
      metrics.Total = double.Parse(memory[1]);
      metrics.Used = double.Parse(memory[2]);
      metrics.Free = double.Parse(memory[3]);
      return metrics;
    }

    /// <summary>
    /// Gets the os system memory usage information
    /// for windows platform
    /// </summary>
    /// <returns>MemoryMetrics</returns>
    private MemoryMetrics GetWindowsMetrics()
    {
      string output = "";
      ProcessStartInfo info = new ProcessStartInfo();
      info.FileName = "wmic";
      info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
      info.RedirectStandardOutput = true;
      using (Process process = Process.Start(info))
      {
        output = process.StandardOutput.ReadToEnd();
      }
      string[] lines = output.Trim().Split("\n");
      string[] freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
      string[] totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);
      var metrics = new MemoryMetrics();
      metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
      metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
      metrics.Used = metrics.Total - metrics.Free;
      return metrics;
    }
  }
}
