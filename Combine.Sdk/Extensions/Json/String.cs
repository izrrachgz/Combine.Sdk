using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Extensions.Json
{
  /// <summary>
  /// Provides extension methods for string type objects
  /// </summary>
  public static class StringExtensions
  {    
    /// <summary>
    /// Gets the object representation from a javascript object notation
    /// string object
    /// </summary>
    /// <typeparam name="T">Type of instance</typeparam>
    /// <param name="o">Reference to the json string object</param>
    /// <returns>Object instance or Null</returns>
    public static async Task<T> FromJson<T>(this string o)
    {
      if (o.IsNotValid()) return default(T);
      return await Task.Run(() => JsonConvert.DeserializeObject<T>(o));
    }

    /// <summary>
    /// Saves the given string content as a file
    /// </summary>
    /// <param name="content">Content to save</param>
    /// <param name="idented">Specifies if the content must be saved in an indented format</param>
    /// <param name="filePath">File path</param>
    /// <param name="fileName">File name without extension</param>
    /// <param name="fileExtension">File extension without dot (.)</param>
    /// <returns>True or False</returns>
    public static async Task<bool> SaveAsJsonFile(this object o, bool indented = true, string filePath = null, string fileName = null)
    {
      //Validate the object content
      if (o == null) return false;
      //Initialize default path and name values
      filePath = filePath ?? AppDomain.CurrentDomain.BaseDirectory;
      fileName = fileName ?? $@"{Guid.NewGuid():N}";
      //Validate the path and name file values
      if (!filePath.IsDirectoryPath() || fileName.IsNotValid()) return false;
      string fileFullName = $@"{filePath}{fileName}.json";
      bool saved = false;
      try
      {
        //Check for directory existance, if it doesnt exists then we've to create it
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        Formatting format = indented ? Formatting.Indented : Formatting.None;
        //Get the json string content
        string content = await Task.Run(() => JsonConvert.SerializeObject(o, format));
        //Save all text
        await File.WriteAllTextAsync(fileFullName, content, Encoding.UTF8);
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
