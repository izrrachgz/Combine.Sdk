using Xunit;
using Combine.Sdk.Data.Definitions.Response;

namespace Combine.Sdk.Tests.Data.Definitions.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the ComplexResponse instance available
  /// </summary>
  public class ComplexResponseTests
  {
    /// <summary>
    /// Creates a new complex response tests instance
    /// </summary>
    public ComplexResponseTests()
    {

    }
    
    /// <summary>
    /// Proves that the public constructor
    /// initializes the instance variables
    /// supplied through parameters correctly
    /// </summary>
    [Fact]
    public void ComplexResponse()
    {
      ModelResponse<int> response = new ModelResponse<int>(1);
      Assert.True(response.Correct);
    }
  }
}
