using Xunit;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for Byte type objects
  /// </summary>
  public class FactByteTests
  {
    /// <summary>
    /// Creates a new instance of Fact Byte Tests
    /// </summary>
    public FactByteTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with byte array object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      byte[] bytes = new byte[0] { };
      Assert.True(bytes.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with byte array object
    /// </summary>
    [Fact]
    public void IsNotValidByteList()
    {
      List<byte[]> bytes = new List<byte[]>(1)
      {
        new byte[0]{}
      };
      Assert.True(bytes.IsNotValid());
    }
  }
}
