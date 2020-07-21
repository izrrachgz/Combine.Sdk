using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for Decimal type objects
  /// </summary>
  public static class DecimalExtensions
  {
    /// <summary>
    /// Checks if the given decimal value is null or is equal to the min value
    /// </summary>
    /// <param name="value">decimal to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this decimal value)
    {
      return value.Equals(decimal.MinValue);
    }
   
    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of decimal to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this IEnumerable<decimal> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }
  }
}
