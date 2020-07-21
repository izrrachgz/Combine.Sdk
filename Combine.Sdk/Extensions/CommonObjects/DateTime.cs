using System;
using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for DateTime type objects
  /// </summary>
  public static class DateTimeExtensions
  {
    /// <summary>
    /// Checks if the given DateTime value is null or is equal to the min value
    /// </summary>
    /// <param name="date">DateTime to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this DateTime date)
    {
      return date == null || date.Equals(DateTime.MinValue);
    }   

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of DateTime to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this IEnumerable<DateTime> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }
  }
}
