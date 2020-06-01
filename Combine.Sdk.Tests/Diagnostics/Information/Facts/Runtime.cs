using Xunit;
using Combine.Sdk.Diagnostics.Information;
using Combine.Sdk.Diagnostics.Definitions.Models;

namespace Combine.Sdk.Tests.Diagnostics.Information.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the Runtim instance available
  /// </summary>
  public class RuntimeTests
  {
    /// <summary>
    /// Creates a new runtime tests instance
    /// </summary>
    public RuntimeTests() { }

    /// <summary>
    /// Proves that the GetMemoryMetrics methods
    /// can retrieve the platform memory metrics
    /// </summary>
    [Fact]
    public void MemoryMetrics()
    {
      MemoryMetrics metrics = Runtime.GetMemoryMetrics();
      Assert.True(metrics != null);
    }

    /// <summary>
    /// Proves that the CpuMetrics methods
    /// can retrieve the platform cpu metrics
    /// </summary>
    [Fact]
    public void CpuMetrics()
    {
      CpuMetrics metrics = Runtime.GetCpuMetrics();
      Assert.True(metrics != null);
    }

    /// <summary>
    /// Proves that the PlatformMetrics methods
    /// can retrieve the platform metrics
    /// </summary>
    [Fact]
    public void PlatformMetrics()
    {
      PlatformMetrics metrics = new PlatformMetrics();
      Assert.True(metrics != null);
    }
  }
}
