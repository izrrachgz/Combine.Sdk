using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for KeyValuePair type objects
  /// </summary>
  public static class KeyValuePairExtensions
  {
    /// <summary>
    /// Gets the first element from the collection
    /// that matches the key name supplied
    /// </summary>
    /// <typeparam name="T">Type to cast to</typeparam>
    /// <param name="list">Collection reference</param>
    /// <param name="name">Key name</param>
    /// <returns>Object</returns>
    public static T GetFirst<T>(this List<KeyValuePair<string, object>> list, string name)
    {
      if (list.IsNotValid() || name.IsNotValid())
        return default;
      return (T)list
        .Where(e => e.Key.Equals(name))
        .Select(e => e.Value)
        .FirstOrDefault();
    }
  }
}
