using Xunit;
using Combine.Sdk.Data.Representation.Response;

namespace Combine.Sdk.Tests.Data.Representation.Facts.Response
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the BasicResponse instance available
  /// </summary>
  public class FactBasicResponseTests
  {
    /// <summary>
    /// Creates a new basic response tests instance
    /// </summary>
    public FactBasicResponseTests()
    {

    }

    /// <summary>
    /// Proves that the public constructor
    /// initializes the instance variables
    /// correctly
    /// </summary>
    [Fact]
    public void BasicResponse()
    {
      BasicResponse response = new BasicResponse();
      Assert.False(response.Correct);
    }
  }
}
