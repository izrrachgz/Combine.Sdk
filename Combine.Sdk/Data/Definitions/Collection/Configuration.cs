using System.Collections.Generic;

namespace Combine.Sdk.Data.Definitions.Collection
{
  /// <summary>
  /// Represent the most common data model
  /// for a configuration file
  /// </summary>
  public class Configuration
  {
    /// <summary>
    /// Key-valued-pair list of configurations
    /// </summary>
    public List<KeyValuePair<string, object>> Values { get; set; }

    /// <summary>
    /// Creates a new configuration instance
    /// </summary>
    public Configuration()
    {
      Values = new List<KeyValuePair<string, object>>();
    }
  }
}
