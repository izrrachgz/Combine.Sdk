using System;
using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for Guid type objects
  /// </summary>
  public static class GuidExtensions
  {
    /// <summary>
    /// Checks if the given Guid value is null or is equal to the min value
    /// </summary>
    /// <param name="date">Guid to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this Guid value)
    {
      return value == null || value.Equals(Guid.Empty);
    }

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of Guid to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this IEnumerable<Guid> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }
  }
}
