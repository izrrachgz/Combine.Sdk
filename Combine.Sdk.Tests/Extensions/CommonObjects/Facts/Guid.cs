using Xunit;
using System;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for Guid type objects
  /// </summary>
  public class GuidTests
  {
    /// <summary>
    /// Creates a new instance of Fact Guid Tests
    /// </summary>
    public GuidTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Guid object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      Assert.True(Guid.Empty.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Guid Array object
    /// </summary>
    [Fact]
    public void IsNotValidGuidArray()
    {
      Guid[] array = new Guid[1]
      {
        Guid.Empty
      };
      Assert.True(array.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Guid List object
    /// </summary>
    [Fact]
    public void IsNotValidGuidList()
    {
      List<Guid> dates = new List<Guid>(1)
      {
        Guid.Empty
      };
      Assert.True(dates.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Guid Array List object
    /// </summary>
    [Fact]
    public void IsNotValidGuidArrayList()
    {
      List<Guid[]> dates = new List<Guid[]>(1)
      {
        new Guid[1]
        {
          Guid.Empty
        }
      };
      Assert.True(dates.IsNotValid());
    }
  }
}
