namespace Combine.Sdk.Diagnostics.Definitions.Models
{
  /// <summary>
  /// Provides a data model representation for a memory metric
  /// object entity
  /// </summary>
  public class MemoryMetrics
  {
    /// <summary>
    /// Usable memory
    /// </summary>
    public double Total { get; set; }

    /// <summary>
    /// Allocated memory
    /// </summary>
    public double Used { get; set; }

    /// <summary>
    /// Unallocated memory
    /// </summary>
    public double Free { get; set; }

    /// <summary>
    /// Creates a new memory metrics empty instance
    /// </summary>
    public MemoryMetrics()
    {
    }

    /// <summary>
    /// Creates a new memory metrics instance
    /// using the supplied values
    /// </summary>
    /// <param name="total">Total memory</param>
    /// <param name="used">Used memory</param>
    /// <param name="free">Free memory</param>
    public MemoryMetrics(double total, double used, double free)
    {
      Total = total;
      Used = used;
      Free = free;
    }
  }
}
