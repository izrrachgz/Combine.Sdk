using Xunit;
using System;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for TimeSpan type objects
  /// </summary>
  public class TimeSpanTests
  {
    /// <summary>
    /// Creates a new instance of Fact TimeSpan Tests
    /// </summary>
    public TimeSpanTests()
    {
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with TimeSpan object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      TimeSpan time = TimeSpan.MinValue;
      Assert.True(time.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with TimeSpan Array object
    /// </summary>
    [Fact]
    public void IsNotValidTimeSpanArray()
    {
      TimeSpan[] array = new TimeSpan[1]{
          TimeSpan.MinValue
      };
      Assert.True(array.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with TimeSpan List object
    /// </summary>
    [Fact]
    public void IsNotValidTimeSpanList()
    {
      List<TimeSpan> list = new List<TimeSpan>(1){
        TimeSpan.MinValue
      };
      Assert.True(list.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with TimeSpan Array List object
    /// </summary>
    [Fact]
    public void IsNotValidTimeSpanArrayList()
    {
      List<TimeSpan[]> list = new List<TimeSpan[]>(1){
        new TimeSpan[1]
        {
          TimeSpan.MinValue
        }
      };
      Assert.True(list.IsNotValid());
    }
  }
}
