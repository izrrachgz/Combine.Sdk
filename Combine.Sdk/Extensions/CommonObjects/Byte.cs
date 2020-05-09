using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for Byte type objects
  /// </summary>
  public static class ByteExtensions
  {
    /// <summary>
    /// Checks if the given byte array is null or its length is zero
    /// </summary>
    /// <param name="bytes">Byte array to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this byte[] bytes)
    {
      return bytes == null || bytes.Length.Equals(0);
    }

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of byte arrays to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this List<byte[]> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }
  }
}
