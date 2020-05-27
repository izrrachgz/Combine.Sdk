using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.ToolBox.Repalcer;

namespace Combine.Sdk.Tests.ToolBox.Replaces.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// availables in the file replacer class
  /// </summary>
  public class FileReplacerTests
  {
    /// <summary>
    /// Creates a new file replacer test instance
    /// </summary>
    public FileReplacerTests()
    {

    }

    /// <summary>
    /// Proves that the file replacer method
    /// ReplaceKeys can replace the supplied
    /// value key list in the file content
    /// correctly
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task ReplaceKeys()
    {
      string url = $@"{AppDomain.CurrentDomain.BaseDirectory}Tools\Replacer\Templates\TextFile.txt";
      FileReplacer fileReplacer = new FileReplacer(url);
      List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>(3)
      {
        new KeyValuePair<string, string>(@"{{Name}}",@"Israel Chavezz"),
        new KeyValuePair<string, string>(@"{{Age}}",@"27"),
        new KeyValuePair<string, string>(@"{{Email}}",@"izrra@mail.com"),
      };
      await fileReplacer.ReplaceKeys(values);
      List<string> keys = values
        .Select(v => v.Key)
        .ToList();
      Assert.True(keys.TrueForAll(k => !fileReplacer.Content.Contains(k)));
    }

    /// <summary>
    /// Proves that the file replacer method
    /// Save can save the file content correctly    
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task Save()
    {
      string url = $@"{AppDomain.CurrentDomain.BaseDirectory}Tools\Replacer\Templates\TextFile.txt";
      FileReplacer fileReplacer = new FileReplacer(url);
      List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>(3)
      {
        new KeyValuePair<string, string>(@"{{Name}}",@"Israel Chavezz"),
        new KeyValuePair<string, string>(@"{{Age}}",@"27"),
        new KeyValuePair<string, string>(@"{{Email}}",@"izrra@mail.com"),
      };
      await fileReplacer.ReplaceKeys(values);
      string urlCopy = $@"{AppDomain.CurrentDomain.BaseDirectory}TestResults\Tools\Replacer\TextFile.txt";
      Assert.True(await fileReplacer.Save(urlCopy));
    }

    /// <summary>
    /// Proves that the file replacer method
    /// ReplaceAndSave can replace the supplied
    /// value key list and save the file content
    /// correctly
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task ReplaceAndSave()
    {
      string url = $@"{AppDomain.CurrentDomain.BaseDirectory}Tools\Replacer\Templates\TextFile.txt";
      FileReplacer fileReplacer = new FileReplacer(url);
      List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>(3)
      {
        new KeyValuePair<string, string>(@"{{Name}}",@"Israel Chavezz"),
        new KeyValuePair<string, string>(@"{{Age}}",@"27"),
        new KeyValuePair<string, string>(@"{{Email}}",@"izrra@mail.com"),
      };
      string urlCopy = $@"{AppDomain.CurrentDomain.BaseDirectory}TestResults\Tools\Replacer\TextFile.txt";
      Assert.True(await fileReplacer.ReplaceAndSave(values, urlCopy));
    }
  }
}
