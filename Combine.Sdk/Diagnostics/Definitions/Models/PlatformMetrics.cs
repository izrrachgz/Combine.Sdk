using System;
using System.Runtime.InteropServices;
using Combine.Sdk.Diagnostics.Information;

namespace Combine.Sdk.Diagnostics.Definitions.Models
{
  /// <summary>
  /// Provides a data model representation for a platform metric
  /// object entity
  /// </summary>
  public class PlatformMetrics
  {
    /// <summary>
    /// Os name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Os description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Indicates if the os is unix based
    /// </summary>
    public bool IsUnix { get; }

    /// <summary>
    /// Memory metrics at the moment
    /// </summary>
    public MemoryMetrics Memory { get; }

    /// <summary>
    /// Cpu metrics at the moment
    /// </summary>
    public CpuMetrics Cpu { get; }

    /// <summary>
    /// Platform read moment
    /// </summary>
    public DateTime Date { get; }

    /// <summary>
    /// Creates a new platform metrics instance
    /// initializing its values
    /// </summary>
    public PlatformMetrics()
    {
      Name = Runtime.IsUnix() ? @"Unix" : @"Windows";
      Description = RuntimeInformation.OSDescription;
      IsUnix = Runtime.IsUnix();
      Memory = Runtime.GetMemoryMetrics();
      Cpu = Runtime.GetCpuMetrics();
      Date = DateTime.Now;
    }
  }
}
