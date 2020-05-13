using Xunit;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for HttpResponseMessage type objects
  /// </summary>
  public class FactHttpResponseTests
  {
    /// <summary>
    /// Creates a new instance of Fact HttpResponseMessage Tests
    /// </summary>
    public FactHttpResponseTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with HttpResponseMessage object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.InternalServerError);
      Assert.True(message.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with HttpResponseMessage Array object
    /// </summary>
    [Fact]
    public void IsNotValidHttpResponseMessageArray()
    {
      HttpResponseMessage[] array = new HttpResponseMessage[1] {
        new HttpResponseMessage(HttpStatusCode.InternalServerError)
      };
      Assert.True(array.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with HttpResponseMessage List object
    /// </summary>
    [Fact]
    public void IsNotValidHttpResponseMessageList()
    {
      List<HttpResponseMessage> list = new List<HttpResponseMessage>(1){
        new HttpResponseMessage(HttpStatusCode.InternalServerError)
      };
      Assert.True(list.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with HttpResponseMessage Array List object
    /// </summary>
    [Fact]
    public void IsNotValidHttpResponseMessageArrayList()
    {
      List<HttpResponseMessage[]> list = new List<HttpResponseMessage[]>(1){
        new HttpResponseMessage[1]
        {
          new HttpResponseMessage(HttpStatusCode.InternalServerError)
        }
      };
      Assert.True(list.IsNotValid());
    }

    /// <summary>
    /// Proves that the AddStramContent extension method
    /// adds correctly an Stream object type to the HttpResponseMessage
    /// </summary>
    [Fact]
    public void AddStreamContent()
    {
      HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
      using (FileStream fs = File.OpenWrite($@"{AppDomain.CurrentDomain.BaseDirectory}Tools\Http\HttpConfig.json"))
      {
        Assert.True(message.AddStreamContent(fs));
      }
    }

    /// <summary>
    /// Proves that the AddStramContent extension method
    /// adds correctly an Stream object type to the HttpResponseMessage
    /// </summary>
    [Fact]
    public void AddAttachmentFromBytes()
    {
      byte[] bytes = new byte[3] { 1, 2, 3 };
      HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
      Assert.True(message.AddAttachment(bytes, @"application/text"));
    }

    /// <summary>
    /// Proves that the AddStramContent extension method
    /// adds correctly an Stream object type to the HttpResponseMessage
    /// </summary>
    [Fact]
    public void AddAttachmentFromFile()
    {
      HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
      using (FileStream fs = File.OpenWrite($@"{AppDomain.CurrentDomain.BaseDirectory}Tools\Http\HttpConfig.json"))
      {
        Assert.True(message.AddStreamContent(fs));
      }
    }

    /// <summary>
    /// Proves that the AddStramContent extension method
    /// adds correctly an Stream object type to the HttpResponseMessage
    /// </summary>
    [Fact]
    public void AddAttachmentFromString()
    {
      HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
      Assert.True(message.AddAttachment(@"Hello", @"application/text"));
    }
  }
}
