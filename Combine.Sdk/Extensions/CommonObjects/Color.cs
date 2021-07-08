using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for Color type objects
  /// </summary>
  public static class ColorExtensions
  {
    /// <summary>
    /// Checks if the given Color value is null or is equal to the min value
    /// </summary>
    /// <param name="color">Color to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this Color color)
    {
      return color.Equals(Color.Empty) || color.Equals(Color.Transparent);
    }

    /// <summary>
    /// Checks if the given Color array is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="array">Array of Color to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this Color[] array)
    {
      return array == null || !array.Any() || array.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of Color to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this List<Color> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of Color to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this List<Color[]> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Converts the system color into the hex argb
    /// specification string format
    /// </summary>
    /// <param name="color">Color reference</param>
    /// <returns>String</returns>
    public static string ToHexArgb(this Color color)
    {
      return $@"{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }
  }
}
