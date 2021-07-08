using System;
using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for TimeSpan type objects
  /// </summary>
  public static class TimeSpanExtensions
  {
    /// <summary>
    /// Checks if the given TimeSpan value is null or is equal to the min value
    /// </summary>
    /// <param name="time">TimeSpan to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this TimeSpan time)
    {
      return time.Equals(TimeSpan.MinValue);
    }

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of TimeSpan to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this IEnumerable<TimeSpan> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }
  }
}
