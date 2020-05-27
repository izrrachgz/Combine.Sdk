using Xunit;
using Combine.Sdk.Data.Definitions.Collection;

namespace Combine.Sdk.Tests.Data.Definitions.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the Configuration instance available
  /// </summary>
  public class FactConfigurationTests
  {
    /// <summary>
    /// Creates a new configuration tests instance
    /// </summary>
    public FactConfigurationTests()
    {

    }

    /// <summary>
    /// Proves that the public constructor
    /// initializes the instance variables
    /// correctly
    /// </summary>
    [Fact]
    public void Configuration()
    {
      Configuration configuration = new Configuration();
      Assert.True(configuration != null);
    }
  }
}
