using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Extensions.Json
{
  /// <summary>
  /// Provides the mechanism to load object instances
  /// using the file contend specified by url
  /// </summary>
  /// <typeparam name="T">Instance type</typeparam>
  public class JsonLoader<T> where T : class
  {
    /// <summary>
    /// Private access instance
    /// </summary>
    private T @Class { get; set; }

    /// <summary>
    /// Instance
    /// </summary>
    public T Instance => @Class;

    /// <summary>
    /// Url pointer to json file
    /// </summary>
    public string Url { get; }

    /// <summary>
    /// Creates a new json file loader instance
    /// </summary>
    /// <param name="url"></param>
    public JsonLoader(string url)
    {
      Url = url;
    }

    /// <summary>
    /// Loads the instance specified
    /// using the file content
    /// </summary>
    /// <returns></returns>
    public async Task Load()
    {
      //Verify previous load or url
      if (@Class != null || Url.IsNotValid() || !Url.IsFilePath(out Uri uri) || !Url.EndsWith(@".json"))
        return;
      //Open file stream
      using (FileStream fs = File.OpenRead(Url))
      {
        //Read file contend
        using (StreamReader sr = new StreamReader(fs))
        {
          //Append file contend
          StringBuilder sb = new StringBuilder();
          while (!sr.EndOfStream)
            sb.AppendLine(await sr.ReadLineAsync());
          //Create class instance
          @Class = await Task.Run(() => JsonConvert.DeserializeObject<T>(sb.ToString()));
          sr.Dispose();
        }
        fs.Dispose();
      }
    }
  }
}
