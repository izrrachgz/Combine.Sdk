using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.ToolBox.Repalcer
{
  /// <summary>
  /// Provides a mechanism to replace the contents
  /// of an specified file
  /// </summary>
  public class FileReplacer
  {
    /// <summary>
    /// Internal access file url address
    /// </summary>
    private string Url { get; }

    /// <summary>
    /// Internal access file content
    /// </summary>
    private string FileContent { get; set; }

    /// <summary>
    /// File content
    /// </summary>
    public string Content => FileContent;

    /// <summary>
    /// Creates a new file replacer instance
    /// </summary>
    /// <param name="url">File address</param>
    public FileReplacer(string url)
    {
      Url = url;
    }

    /// <summary>
    /// Saves all the changes made to the
    /// file content in the specified address
    /// </summary>
    /// <param name="url">File address</param>
    /// <returns>Bool</returns>
    public async Task<bool> Save(string url = null)
    {
      string saveUrl = url ?? Url;
      if (!saveUrl.IsFilePath(out Uri uri))
        return false;
      await File.WriteAllTextAsync(saveUrl, FileContent);
      return true;
    }

    /// <summary>
    /// Replaces the file content using
    /// the supplied key-value list
    /// </summary>
    /// <param name="values">Key-value values to replace with</param>
    /// <returns>Bool</returns>
    public async Task<bool> ReplaceKeys(List<KeyValuePair<string, string>> values)
    {
      if (!FileContent.IsNotValid() || !Url.IsFilePath(out Uri uri) || values.IsNotValid())
        return false;
      using (FileStream fs = File.OpenRead(Url))
      {
        using (StreamReader sr = new StreamReader(fs))
        {
          StringBuilder sb = new StringBuilder();
          while (!sr.EndOfStream)
            sb.AppendLine(await sr.ReadLineAsync());
          FileContent = sb.ToString();
          sr.Dispose();
        }
        fs.Dispose();
      }
      if (FileContent.IsNotValid())
        return false;
      values.ForEach(v => FileContent = FileContent.Replace(v.Key, v.Value));
      return true;
    }

    /// <summary>
    /// Replaces and saves the file content in
    /// the specified address
    /// </summary>
    /// <param name="values">Key-value values to replace with</param>
    /// <param name="url">Save address</param>
    /// <returns>Bool</returns>
    public async Task<bool> ReplaceAndSave(List<KeyValuePair<string, string>> values, string url = null)
    {
      if (!await ReplaceKeys(values))
        return false;
      return await Save(url);
    }
  }
}
