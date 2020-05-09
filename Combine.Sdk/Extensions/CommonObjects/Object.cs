using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for Primitive objects
  /// </summary>
  public static class ObjectExtensions
  {
    /// <summary>
    /// Checks if the given object value is not null and a representation of an Numeric object
    /// </summary>
    /// <param name="o">Object to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNumber(this object o)
    {
      return o != null && o is sbyte ||
        o is byte ||
        o is ushort ||
        o is short ||
        o is uint ||
        o is int ||
        o is ulong ||
        o is long ||
        o is float ||
        o is double ||
        o is decimal;
    }

    /// <summary>
    /// Checks if the given object value is not null and a representation of an Stirng object
    /// </summary>
    /// <param name="o">Object to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsString(this object o)
    {
      return o != null && o is string;
    }

    /// <summary>
    /// Checks if the given object value is not null and a representation of an DateTime object
    /// </summary>
    /// <param name="o">Object to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsDateTime(this object o)
    {
      return o != null && o is DateTime;
    }

    /// <summary>
    /// Checks if the given object value is not null and a representation of an TimeSpan object
    /// </summary>
    /// <param name="o">Object to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsTimeSpan(this object o)
    {
      return o != null && o is TimeSpan;
    }

    /// <summary>
    /// Checks if the given object value belongs to 
    /// the system set of primitive object types
    /// </summary>
    /// <param name="o">Object reference</param>
    /// <returns>True or False</returns>
    public static bool IsSystemBased(this object o)
    {
      string sb = o.GetType().Namespace;
      return !sb.IsNotValid() && sb.Equals("System");
    }    

    /// <summary>
    /// Gets the property names for the given object
    /// </summary>
    /// <param name="o">Object reference</param>
    /// <returns>String array</returns>
    public static string[] GetPropertyNames(this object o)
    {
      PropertyInfo[] info = o.GetType().GetProperties();
      string[] names = new string[info.Length];
      for (int x = 0; x < info.Length; x++)
      {
        names[x] = info[x].Name;
      }
      return names;
    }

    /// <summary>
    /// Gets the property names for the given object
    /// that are string type
    /// </summary>
    /// <param name="o">Object reference</param>
    /// <returns>String array</returns>
    public static string[] GetStringPropertyNames(this object o)
    {
      PropertyInfo[] info = o.GetType().GetProperties()
        .Where(i => i.PropertyType.Name.Equals(@"String"))
        .ToArray();
      string[] names = new string[info.Length];
      for (int x = 0; x < info.Length; x++)
      {
        names[x] = info[x].Name;
      }
      return names;
    }

    /// <summary>
    /// Checks if the given object value is not null and a representation of an string list
    /// or an string array
    /// </summary>
    /// <param name="list">Object to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsStringCollection(this object list)
    {
      return list != null && list is List<string> || list is string[];
    }

    /// <summary>
    /// Checks if the given object value is not null and a representation of a numeric value
    /// collection
    /// </summary>
    /// <param name="list">Object to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNumericCollection(this object list)
    {
      return list != null && list is IEnumerable<sbyte> ||
        list is IEnumerable<byte> ||
        list is IEnumerable<ushort> ||
        list is IEnumerable<short> ||
        list is IEnumerable<uint> ||
        list is IEnumerable<int> ||
        list is IEnumerable<ulong> ||
        list is IEnumerable<long> ||
        list is IEnumerable<float> ||
        list is IEnumerable<double> ||
        list is IEnumerable<decimal>;
    }

    /// <summary>
    /// Returns the byte array representation of the given object value
    /// </summary>
    /// <param name="o">Object Referente</param>
    /// <returns>Byte array</returns>
    public static byte[] GetBytes<T>(this T o)
    {
      if (o == null) return new byte[0] { };
      BinaryFormatter bf = new BinaryFormatter();
      MemoryStream ms = new MemoryStream();
      bf.Serialize(ms, o);
      ms.Position = 0;
      return ms.ToArray();
    }

    /// <summary>
    /// Saves the given object content as a .dat file
    /// </summary>
    /// <param name="content">Content to save</param>
    /// <param name="filePath">File path</param>
    /// <param name="fileName">File name without extension</param>    
    /// <returns>True or False</returns>
    public static async Task<bool> SaveAsDataFile(this object content, string filePath = null, string fileName = null)
    {
      //Validate the object content
      if (content == null) return false;
      //Initialize default path and name values
      filePath = filePath ?? AppDomain.CurrentDomain.BaseDirectory;
      fileName = fileName ?? $@"{Guid.NewGuid():N}";
      //Validate the path and name file values
      if (!filePath.IsDirectoryPath() || fileName.IsNotValid()) return false;
      string fileFullName = $@"{filePath}{fileName}.dat";
      bool saved = false;
      try
      {
        //Check for directory existance, if it doesnt exists then we've to create it
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        //Save all text
        await File.WriteAllBytesAsync(fileFullName, content.GetBytes());
        //Check for file existance
        saved = new FileInfo(fileFullName).Exists;
      }
      catch (Exception)
      {
        saved = false;
      }
      return saved;
    }
  }
}
