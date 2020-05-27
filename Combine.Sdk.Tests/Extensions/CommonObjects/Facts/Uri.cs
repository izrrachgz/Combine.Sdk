using Xunit;
using System;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for Uri type objects
  /// </summary>
  public class UriTests
  {
    /// <summary>
    /// Creates a new instance of Fact Uri Tests
    /// </summary>
    public UriTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Uri object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      Uri.TryCreate(@"non-valid-format", UriKind.Absolute, out Uri uri);
      Assert.True(uri.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Uri Array object
    /// </summary>
    [Fact]
    public void IsNotValidUriArray()
    {
      Uri[] array = new Uri[0];
      Assert.True(array.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Uri List object
    /// </summary>
    [Fact]
    public void IsNotValidUriList()
    {
      List<Uri> dates = new List<Uri>(0);
      Assert.True(dates.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Uri Array List object
    /// </summary>
    [Fact]
    public void IsNotValidUriArrayList()
    {
      List<Uri[]> dates = new List<Uri[]>(0);
      Assert.True(dates.IsNotValid());
    }
  }
}
