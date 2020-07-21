using System;
using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for Uri type objects
  /// </summary>
  public static class UriExtensions
  {
    /// <summary>
    /// Checks if the given Uri value is null or is equal to the min value
    /// </summary>
    /// <param name="uri">Uri to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this Uri uri)
    {
      return uri == null || uri.AbsolutePath.IsNotValid();
    }

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of Uri to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this IEnumerable<Uri> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }
  }
}
