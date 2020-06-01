using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
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
    public static MemoryMetrics GetMemoryMetrics()
      => IsUnix() ? GetUnixMemoryMetrics() : GetWindowsMemoryMetrics();

    /// <summary>
    /// Gets the os system cpu usage information
    /// for the current platform
    /// </summary>
    /// <returns>CpuMetrics</returns>
    public static CpuMetrics GetCpuMetrics()
      => IsUnix() ? new CpuMetrics() : GetWindowsCpuMetrics();

    /// <summary>
    /// Gets the os system memory usage information
    /// for unix platform
    /// </summary>
    /// <returns>MemoryMetrics</returns>
    private static MemoryMetrics GetUnixMemoryMetrics()
    {
      string output = "";
      ProcessStartInfo info = new ProcessStartInfo("free -m")
      {
        FileName = "/bin/bash",
        Arguments = "-c \"free -m\"",
        RedirectStandardOutput = true
      };
      using (Process process = Process.Start(info))
      {
        output = process.StandardOutput.ReadToEnd();
        Console.WriteLine(output);
      }
      string[] lines = output.Split("\n");
      string[] memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
      MemoryMetrics metrics = new MemoryMetrics
      {
        Date = DateTime.Now,
        Total = double.Parse(memory[1]),
        Free = double.Parse(memory[3])
      };
      return metrics;
    }

    /// <summary>
    /// Gets the os system memory usage information
    /// for windows platform
    /// </summary>
    /// <returns>MemoryMetrics</returns>
    private static MemoryMetrics GetWindowsMemoryMetrics()
    {
      string output = "";
      ProcessStartInfo info = new ProcessStartInfo
      {
        FileName = "wmic",
        Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value",
        RedirectStandardOutput = true
      };
      using (Process process = Process.Start(info))
      {
        output = process.StandardOutput.ReadToEnd();
      }
      string[] lines = output.Trim().Split("\n");
      List<KeyValuePair<string, string>> results = lines
        .Select(l =>
        {
          string[] line = l.Split("=", StringSplitOptions.RemoveEmptyEntries);
          return new KeyValuePair<string, string>(line[0], line[1]);
        })
        .ToList();
      MemoryMetrics metrics = new MemoryMetrics()
      {
        Total = Math.Round(double.Parse(results.FirstOrDefault(r => r.Key.Equals(@"TotalVisibleMemorySize")).Value) / 1024, 0),
        Free = Math.Round(double.Parse(results.FirstOrDefault(r => r.Key.Equals(@"FreePhysicalMemory")).Value) / 1024, 0),
        Date = DateTime.Now
      };
      return metrics;
    }

    /// <summary>
    /// Gets the os system cpu usage information
    /// for windows platform
    /// </summary>
    /// <returns>MemoryMetrics</returns>
    private static CpuMetrics GetWindowsCpuMetrics()
    {
      string output = "";
      ProcessStartInfo info = new ProcessStartInfo
      {
        FileName = "wmic",
        Arguments = "Cpu Get Name, Description, Manufacturer, NumberOfCores, NumberOfEnabledCore, ThreadCount, MaxClockSpeed, LoadPercentage /Value",
        RedirectStandardOutput = true
      };
      using (Process process = Process.Start(info))
      {
        output = process.StandardOutput.ReadToEnd();
      }
      string[] lines = output.Trim().Split("\n");
      List<KeyValuePair<string, string>> results = lines
        .Select(l =>
        {
          string[] line = l.Split("=", StringSplitOptions.RemoveEmptyEntries);
          return new KeyValuePair<string, string>(line[0], line[1]);
        })
        .ToList();
      CpuMetrics metrics = new CpuMetrics()
      {
        Name = results.FirstOrDefault(r => r.Key.Equals(@"Name")).Value,
        Description = results.FirstOrDefault(r => r.Key.Equals(@"Description")).Value,
        Manufacturer = results.FirstOrDefault(r => r.Key.Equals(@"Manufacturer")).Value,
        NumberOfCores = byte.Parse(results.FirstOrDefault(r => r.Key.Equals(@"NumberOfCores")).Value),
        NumberOfEnabledCore = byte.Parse(results.FirstOrDefault(r => r.Key.Equals(@"NumberOfEnabledCore")).Value),
        ThreadCount = byte.Parse(results.FirstOrDefault(r => r.Key.Equals(@"ThreadCount")).Value),
        MaxClockSpeed = int.Parse(results.FirstOrDefault(r => r.Key.Equals(@"MaxClockSpeed")).Value),
        LoadPercentage = double.Parse(results.FirstOrDefault(r => r.Key.Equals(@"LoadPercentage")).Value),
        Date = DateTime.Now
      };
      return metrics;
    }
  }
}
