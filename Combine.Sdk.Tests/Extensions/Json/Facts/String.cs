using Xunit;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Json;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.Json.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the json string
  /// extension methods availables
  /// </summary>
  public class FactStringTests
  {
    /// <summary>
    /// Creates a new string tests instance
    /// </summary>
    public FactStringTests()
    {

    }

    /// <summary>
    /// Proves that the extension method FromJson
    /// Deserializes a string json object value
    /// into a specified object instance correctly
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task FromJson()
    {
      List<string> list = new List<string>(3)
      {
        @"Israel",
        @"Chavez",
        @"Gamez"
      };
      string json = await list.ToJson();
      List<string> deserialized = await json.FromJson<List<string>>();
      Assert.True(!deserialized.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method SaveAsJsonFile
    /// can save a serialized json object into a file
    /// correctly
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SaveAsJsonFile()
    {
      List<string> list = new List<string>(3)
      {
        @"Israel",
        @"Chavez",
        @"Gamez"
      };
      await list.SaveAsJsonFile(true, $@"{AppDomain.CurrentDomain.BaseDirectory}", @"Listita");
      $@"{AppDomain.CurrentDomain.BaseDirectory}Listita.json".IsFilePath(out Uri uri);
      Assert.True(!uri.IsNotValid());
    }
  }
}
