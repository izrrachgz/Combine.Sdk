using Xunit;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for Dictionary type objects
  /// </summary>
  public class DictionaryTests
  {
    /// <summary>
    /// Creates a new instance of Fact Dictionary Tests
    /// </summary>
    public DictionaryTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Dictionary object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>(1);
      Assert.True(dictionary.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Dictionary Array object
    /// </summary>
    [Fact]
    public void IsNotValidDictionaryArray()
    {
      Dictionary<string, string>[] array = new Dictionary<string, string>[1];
      Assert.True(array.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Dictionary List object
    /// </summary>
    [Fact]
    public void IsNotValidDictionaryList()
    {
      List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
      Assert.True(list.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with Dictionary Array List object
    /// </summary>
    [Fact]
    public void IsNotValidDictionaryArrayList()
    {
      List<Dictionary<string, string>[]> list = new List<Dictionary<string, string>[]>();
      Assert.True(list.IsNotValid());
    }
  }
}
