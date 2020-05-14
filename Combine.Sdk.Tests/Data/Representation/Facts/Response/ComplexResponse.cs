using Xunit;
using Combine.Sdk.Data.Representation.Response;

namespace Combine.Sdk.Tests.Data.Representation.Facts.Response
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the ComplexResponse instance available
  /// </summary>
  public class FactComplexResponseTests
  {
    /// <summary>
    /// Creates a new complex response tests instance
    /// </summary>
    public FactComplexResponseTests()
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
      ComplexResponse<int> response = new ComplexResponse<int>(1);
      Assert.True(response.Correct);
    }
  }
}
