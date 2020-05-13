using Xunit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Json;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.Json.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the json
  /// extension methods availables
  /// </summary>
  public class FactHttpResponseTests
  {
    /// <summary>
    /// Creates a new instance of HttpResponseMessage tests
    /// </summary>
    public FactHttpResponseTests()
    {

    }

    /// <summary>
    /// Proves that the extension method HttpResponseMessage.AddJsonAttachment
    /// adds the given object to the response message content in json format correctly
    /// </summary>
    [Fact]
    public async Task AddJsonAttachment()
    {
      object json = new
      {
        Name = @"Israel",
        LastName = @"Chavez"
      };
      HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
      Assert.True(await message.AddJsonAttachment(json));
    }

    /// <summary>
    /// Proves that the extension method HttpResponseMessage.InterpretFromJsonContent
    /// Interprets correctly the http response message content as the specified
    /// instance object type
    /// </summary>
    [Fact]
    public async Task InterpretFromJsonContent()
    {
      List<string> list = new List<string>(3)
      {
        @"Israel",
        @"Chavez",
        @"Gamez"
      };
      HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
      await message.AddJsonAttachment(list);
      List<string> deserialized = await message.InterpretFromJsonContent<List<string>>();
      Assert.True(!deserialized.IsNotValid());
    }
  }
}
