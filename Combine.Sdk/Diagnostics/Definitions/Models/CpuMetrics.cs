using System;

namespace Combine.Sdk.Diagnostics.Definitions.Models
{
  /// <summary>
  /// Provides a data model representation for a cpu platform metric
  /// object entity
  /// </summary>
  public class CpuMetrics
  {
    /// <summary>
    /// Manufacturer name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Manufacturer description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Hardware manufacturer
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// Number of cores
    /// </summary>
    public byte NumberOfCores { get; set; }

    /// <summary>
    /// Number of enabled cores
    /// </summary>
    public byte NumberOfEnabledCores { get; set; }

    /// <summary>
    /// Number of threads
    /// </summary>
    public byte ThreadCount { get; set; }

    /// <summary>
    /// Max clock speed in Mhz
    /// </summary>
    public int MaxClockSpeed { get; set; }

    /// <summary>
    /// Current load percentage
    /// </summary>
    public double LoadPercentage { get; set; }

    /// <summary>
    /// Platform read moment
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Creates a new cpu metrics empty instance
    /// </summary>
    public CpuMetrics()
    {

    }
  }
}
