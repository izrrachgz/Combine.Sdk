using Xunit;
using System.IO;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for Stream type objects
  /// </summary>
  public class FactStreamTests
  {
    /// <summary>
    /// Creates a new instance of Fact Stream Tests
    /// </summary>
    public FactStreamTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Stream object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      Stream stream = new MemoryStream();
      Assert.True(stream.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Stream Array object
    /// </summary>
    [Fact]
    public void IsNotValidSreamArray()
    {
      Stream[] stream = new Stream[0];
      Assert.True(stream.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Stream List object
    /// </summary>
    [Fact]
    public void IsNotValidStreamList()
    {
      List<Stream> list = new List<Stream>(0);
      Assert.True(list.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Stream Array List object
    /// </summary>
    [Fact]
    public void IsNotValidStreamArrayList()
    {
      List<Stream[]> arrayList = new List<Stream[]>(0);
      Assert.True(arrayList.IsNotValid());
    }
  }
}
