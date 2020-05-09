using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Combine.Sdk.Extensions.Json
{
  /// <summary>
  /// Provides extension methods for primitive object types
  /// </summary>
  public static class ObjectExtensions
  {
    /// <summary>
    /// Serialize the given object into an javascript object notation
    /// string
    /// </summary>
    /// <param name="o">Object to serialize</param>
    /// <returns>String in json format</returns>
    public static async Task<string> ToJson(this object o)
    {
      return await Task.Run(() => JsonConvert.SerializeObject(o));
    }
  }
}
