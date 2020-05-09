using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for string type objects
  /// </summary>
  public static class StringExtensions
  {
    /// <summary>
    /// Checks wheter the given string value is null or has zero length when trimmed
    /// </summary>
    /// <param name="characters">String value to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this string characters)
    {
      return characters == null || characters.Trim().Length.Equals(0);
    }

    /// <summary>
    /// Checks if the given string array contains any non-valid value inside
    /// </summary>
    /// <param name="array">Array of strings to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this string[] array)
    {
      return array == null || !array.Any() || array.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Checks if the given string list contains any non-valid value inside
    /// </summary>
    /// <param name="list">List of strings to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this List<string> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Checks if the given string array list contains any non-valid value inside
    /// </summary>
    /// <param name="list">List of string arrays to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this List<string[]> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Checks if the given string represents an number
    /// </summary>
    /// <param name="characters">String value to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNumber(this string characters)
    {
      return !characters.IsNotValid() && new Regex(@"^\d").IsMatch(characters);
    }

    /// <summary>
    /// Checks if the given string represents an email
    /// </summary>
    /// <param name="characters">String value to evaluate</param>
    /// <param name="email">Instance of the email address value</param>
    /// <returns>True or False</returns>
    public static bool IsEmail(this string characters, out MailAddress email)
    {
      email = null;
      if (characters.IsNotValid()) return false;
      try
      {
        email = new MailAddress(characters);
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    /// <summary>
    /// Checks if the given string represents a DateTime
    /// </summary>
    /// <param name="characters">String value to evaluate</param>
    /// <param name="date">Instance of the date time value</param>
    /// <returns>True or False</returns>
    public static bool IsDateTime(this string characters, out DateTime date)
    {
      date = DateTime.MinValue;
      if (characters.IsNotValid()) return false;
      DateTime.TryParse(characters, out date);
      return !date.IsNotValid();
    }

    /// <summary>
    /// Checks if the given string represents a TimeSpan
    /// </summary>
    /// <param name="characters">String value to evaluate</param>
    /// <param name="time">Instance of the time span value</param>
    /// <returns>True or False</returns>
    public static bool IsTimeSpan(this string characters, out TimeSpan time)
    {
      time = TimeSpan.MinValue;
      if (characters.IsNotValid()) return false;
      TimeSpan.TryParse(characters, out time);
      return !time.IsNotValid();
    }

    /// <summary>
    /// Checks if the given string represents a Guid
    /// </summary>
    /// <param name="characters">String value to evaluate</param>
    /// <param name="guid">Instance of the guid value</param>
    /// <returns>True or False</returns>
    public static bool IsGuid(this string characters, out Guid guid)
    {
      guid = Guid.Empty;
      if (characters.IsNotValid()) return false;
      Guid.TryParse(characters, out guid);
      return !guid.IsNotValid();
    }

    /// <summary>
    /// Checks if the given string represents a Currency value
    /// </summary>
    /// <param name="characters">String value to evaluate</param>
    /// <param name="money">Instance of the decimal value</param>
    /// <returns>True or False</returns>
    public static bool IsCurrency(this string characters, out decimal money)
    {
      money = decimal.MinValue;
      if (characters.IsNotValid()) return false;
      decimal.TryParse(characters, NumberStyles.Currency, CultureInfo.CurrentCulture, out money);
      return !money.IsNotValid();
    }

    /// <summary>
    /// Checks if the given string is a valid directory path
    /// </summary>
    /// <param name="characters">String value to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsDirectoryPath(this string characters)
    {
      return !characters.IsNotValid() && Directory.Exists(characters);
    }

    /// <summary>
    /// Checks if the given string is a valid file path
    /// </summary>
    /// <param name="characters">String value to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsFilePath(this string characters, out Uri uri)
    {
      uri = null;
      if (characters.IsNotValid()) return false;
      bool result = false;
      try
      {
        uri = new Uri(characters);
        result = uri.IsFile;
      }
      catch (Exception)
      {
        result = false;
      }
      return result;
    }

    /// <summary>
    /// Checks if the given string is a valid web directory path
    /// </summary>
    /// <param name="characters">String value to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsWebDirectory(this string characters, out Uri uri)
    {
      uri = null;
      if (characters.IsNotValid()) return false;
      bool result;
      try
      {
        uri = new Uri(characters);
        result = uri.IsWellFormedOriginalString();
      }
      catch (Exception)
      {
        result = false;
      }
      return result;
    }

    /// <summary>
    /// Saves the given string content as a file
    /// </summary>
    /// <param name="content">Content to save</param>
    /// <param name="filePath">File path</param>
    /// <param name="fileName">File name without extension</param>
    /// <param name="fileExtension">File extension without dot (.)</param>
    /// <returns>True or False</returns>
    public static async Task<bool> SaveAsTextFile(this string content, string filePath = null, string fileName = null)
    {
      //Validate the object content
      if (content.IsNotValid()) return false;
      //Initialize default path and name values
      filePath = filePath ?? AppDomain.CurrentDomain.BaseDirectory;
      fileName = fileName ?? $@"{Guid.NewGuid():N}";
      //Validate the path and name file values
      if (!filePath.IsDirectoryPath() || fileName.IsNotValid()) return false;
      string fileFullName = $@"{filePath}{fileName}.txt";
      bool saved = false;
      try
      {
        //Check for directory existance, if it doesnt exists then we've to create it
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
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
