using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace Combine.Sdk.Extensions.CommonObjects
{
  /// <summary>
  /// Provides extension methods for HttpResponseMessage type objects
  /// </summary>
  public static class HttpResponseExtensions
  {
    /// <summary>
    /// Checks if the given response message is null or is not a success 
    /// status code
    /// </summary>
    /// <param name="message">HttpResponseMessage to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this HttpResponseMessage message)
    {
      return message == null || !message.IsSuccessStatusCode;
    }

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list"></param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this IEnumerable<HttpResponseMessage> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Adds the given stream as stream content into the http response message
    /// </summary>
    /// <param name="message">HttpResponseMessage reference</param>
    /// <param name="stream">Stream content to add</param>
    /// <param name="contentType">Type of media content (default is 'application/octet-stream')</param>
    /// <param name="name">File attachment name</param>
    /// <returns>True or False</returns>
    public static bool AddStreamContent(this HttpResponseMessage message, Stream stream, string contentType = null, string name = null)
    {
      if (message.IsNotValid() || stream.IsNotValid()) return false;
      message.Content = new StreamContent(stream);
      message.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType ?? @"application/octet-stream");
      message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(@"attachment")
      {
        FileName = name ?? $@"{DateTime.Now:s}"
      };
      return true;
    }

    /// <summary>
    /// Adds the given byte array as byte array content into the http response message
    /// </summary>
    /// <param name="message">HttpResponseMessage reference</param>
    /// <param name="bytes">Byte array content</param>
    /// <param name="contentType">Content type</param>
    /// <param name="name">Name of file</param>
    /// <returns>True or False</returns>
    public static bool AddAttachment(this HttpResponseMessage message, byte[] bytes, string contentType, string name = null)
    {
      if (message.IsNotValid() || bytes.IsNotValid() || contentType.IsNotValid()) return false;
      message.Content = new ByteArrayContent(bytes);
      message.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
      message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(@"attachment")
      {
        FileName = name ?? $@"{DateTime.Now:s}"
      };
      return true;
    }

    /// <summary>
    /// Adds the given file info as file stream content into the http response message
    /// </summary>
    /// <param name="message">HttpResponseMessage reference</param>
    /// <param name="info">File content</param>
    /// <param name="contentType">Content type</param>
    /// <param name="name">Name of file</param>
    /// <returns>True or False</returns>
    public static bool AddAttachment(this HttpResponseMessage message, FileInfo info, string contentType, string name = null)
    {
      if (message.IsNotValid() || info.IsNotValid() || contentType.IsNotValid()) return false;
      return message.AddStreamContent(new FileStream(info.FullName, FileMode.Open, FileAccess.Read), contentType, name ?? info.Name);
    }

    /// <summary>
    /// Adds the given value as string content into the http response message
    /// </summary>
    /// <param name="message">HttpResponseMessage reference</param>
    /// <param name="content">String content (default is 'application/json')</param>
    /// <param name="contentType">Content type</param>
    /// <param name="name">Name of file</param>
    /// <returns>True or False</returns>
    public static bool AddAttachment(this HttpResponseMessage message, string content, string contentType = @"application/json", string name = null)
    {
      if (message.IsNotValid() || content.IsNotValid() || contentType.IsNotValid()) return false;
      message.Content = new StringContent(content);
      message.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
      message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(@"attachment")
      {
        FileName = name ?? $@"{DateTime.Now:s}"
      };
      return true;
    }
  }
}
