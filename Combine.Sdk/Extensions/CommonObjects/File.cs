using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for FileInfo type objects
  /// </summary>
  public static class FileInfoExtensions
  {
    /// <summary>
    /// Checks if the given value is null or doesnt exists
    /// </summary>
    /// <param name="info">File information</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this FileInfo info)
    {      
      return info == null || !info.FullName.IsNotValid() || !info.Exists;
    }

    /// <summary>
    /// Checks if the given FileInfo array is no valid or contains any non-valid value inside
    /// </summary>
    /// <param name="array">Array of FileInfo to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this FileInfo[] array)
    {
      return array == null || !array.Any() || array.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Checks if the given list is not valid or contains non-valid elements inside
    /// </summary>
    /// <param name="list">Collection of file information</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this List<FileInfo> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list"></param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this List<FileInfo[]> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }
  }
}
