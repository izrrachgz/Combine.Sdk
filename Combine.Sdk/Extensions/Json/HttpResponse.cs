using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Extensions.Json
{
  /// <summary>
  /// Provides extension methods for HttpResponseMessaje type objects
  /// </summary>
  public static class HttpResponseExtensions
  {
    /// <summary>
    /// Adds the given objects to the http response message as json string content
    /// </summary>
    /// <param name="message">HttpResponseMessage reference</param>
    /// <param name="json">Object content</param>
    /// <param name="name">File name</param>
    /// <returns>True or False</returns>
    public static async Task<bool> AddJsonAttachment(this HttpResponseMessage message, object json, string name = null)
    {
      if (message.IsNotValid() || json == null) return false;
      message.Content = new StringContent(await Task.Run(() => JsonConvert.SerializeObject(json)));
      message.Content.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
      message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(@"attachment")
      {
        FileName = name ?? $@"{DateTime.Now:s}"
      };
      return true;
    }

    /// <summary>
    /// Interprets the string content as an object
    /// </summary>
    /// <typeparam name="T">Type of object contained in the response message</typeparam>
    /// <param name="message">HttpResponseMessage reference</param>
    /// <returns>T object interpreted or default value</returns>
    public static async Task<T> InterpretFromJsonContent<T>(this HttpResponseMessage message) where T : class
    {
      if (message == null || !(message.Content is StringContent)) return null;
      return await Task.Run(async () => JsonConvert.DeserializeObject<T>(await message.Content.ReadAsStringAsync()));
    }
  }
}
