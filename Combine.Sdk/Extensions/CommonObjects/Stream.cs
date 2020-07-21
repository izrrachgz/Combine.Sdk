using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for Stream type objects
  /// </summary>
  public static class StreamExtensions
  {
    /// <summary>
    /// Checks if the given Stream value is null or its lengths is zero
    /// or its position of read is not zero
    /// </summary>
    /// <param name="stream">Stream to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this Stream stream)
    {
      return stream == null || stream.Length.Equals(0) || !stream.Position.Equals(0);
    }    

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of Stream to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this IEnumerable<Stream> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }
  }
}
