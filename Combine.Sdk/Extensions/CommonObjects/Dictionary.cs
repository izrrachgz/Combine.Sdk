using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for Dictionary type objects
  /// </summary>
  public static class DictionaryExtensions
  {
    /// <summary>
    /// Checks if the given dictionary is null or doesnt contain any element
    /// </summary>
    /// <typeparam name="T">Type of element retained on the dictionary</typeparam>
    /// <param name="dictionary">Dictionary of elements</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid<T>(this Dictionary<T, T> dictionary)
    {
      return dictionary == null || dictionary.Count.Equals(0);
    }    

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <typeparam name="T">Type of element retained on the dictionary</typeparam>
    /// <param name="list">Collection of Dictionary elements</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid<T>(this IEnumerable<Dictionary<T, T>> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }
  }
}
