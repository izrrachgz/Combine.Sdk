using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for list type objects
  /// </summary>
  public static class ListExtensions
  {
    /// <summary>
    /// Checks if the given list is null or has any non-valid elements inside
    /// </summary>
    /// <typeparam name="T">Type of element retained on the list</typeparam>
    /// <param name="list">Collection of elements</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid<T>(this List<T> list)
    {
      return list == null || !list.Any() || list.Any(e => e == null);
    }    
  }
}
