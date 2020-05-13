using Xunit;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for List type objects
  /// </summary>
  public class FactListTests
  {
    /// <summary>
    /// Creates a new instance of Fact List Tests
    /// </summary>
    public FactListTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with List object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      List<int> list = new List<int>();
      Assert.True(list.IsNotValid());
    }
  }
}
