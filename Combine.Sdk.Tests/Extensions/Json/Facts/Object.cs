using Xunit;
using System.Threading.Tasks;
using Combine.Sdk.Extensions.Json;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.Json.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the json object
  /// extension methods availables
  /// </summary>
  public class ObjectTests
  {
    /// <summary>
    /// Creates a new object test instance
    /// </summary>
    public ObjectTests()
    {
    }

    /// <summary>
    /// Proves that the extension method ToJson
    /// can convert any serializable object into
    /// a javascript notation object
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ToJson()
    {
      object o = new
      {
        Name = @"Israel",
        LastName = @"Chavez"
      };
      string json = await o.ToJson();
      Assert.True(!json.IsNotValid());
    }
  }
}
