using Xunit;
using System;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for DateTime type objects
  /// </summary>
  public class DateTimeTests
  {
    /// <summary>
    /// Creates a new instance of Fact DateTime Tests
    /// </summary>
    public DateTimeTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with DateTime object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      Assert.True(DateTime.MinValue.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with DateTime Array object
    /// </summary>
    [Fact]
    public void IsNotValidDateTimeArray()
    {
      DateTime[] array = new DateTime[1]
      {
        DateTime.MinValue
      };
      Assert.True(array.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with DateTime List object
    /// </summary>
    [Fact]
    public void IsNotValidDateTimeList()
    {
      List<DateTime> dates = new List<DateTime>(1)
      {
        DateTime.MinValue
      };
      Assert.True(dates.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with DateTime Array List object
    /// </summary>
    [Fact]
    public void IsNotValidDateTimeArrayList()
    {
      List<DateTime[]> dates = new List<DateTime[]>(1)
      {
        new DateTime[1]
        {
          DateTime.MinValue
        }
      };
      Assert.True(dates.IsNotValid());
    }
  }
}
