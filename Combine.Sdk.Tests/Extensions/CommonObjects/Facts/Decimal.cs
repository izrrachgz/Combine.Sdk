using Xunit;
using System;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for Decimal type objects
  /// </summary>
  public class FactDecimalTests
  {
    /// <summary>
    /// Creates a new instance of Fact Decimal Tests
    /// </summary>
    public FactDecimalTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Decimal object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      Assert.True(decimal.MinValue.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Decimal Array object
    /// </summary>
    [Fact]
    public void IsNotValidDecimalArray()
    {
      Decimal[] array = new Decimal[1]
      {
        decimal.MinValue
      };
      Assert.True(array.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Decimal List object
    /// </summary>
    [Fact]
    public void IsNotValidDecimalList()
    {
      List<Decimal> dates = new List<Decimal>(1)
      {
        decimal.MinValue
      };
      Assert.True(dates.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Decimal Array List object
    /// </summary>
    [Fact]
    public void IsNotValidDecimalArrayList()
    {
      List<Decimal[]> dates = new List<Decimal[]>(1)
      {
        new Decimal[1]
        {
          decimal.MinValue
        }
      };
      Assert.True(dates.IsNotValid());
    }
  }
}
