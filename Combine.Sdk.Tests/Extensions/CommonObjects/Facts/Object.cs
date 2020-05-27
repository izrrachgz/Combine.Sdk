using Xunit;
using System;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for Primitive type objects
  /// </summary>
  public class ObjectTests
  {
    /// <summary>
    /// Creates a new instance of Fact Object Tests
    /// </summary>
    public ObjectTests()
    {

    }

    /// <summary>
    /// Proves that the IsSystemBased extension method
    /// evaluates correctly a System based C# object
    /// </summary>
    [Fact]
    public void IsSystemBased()
    {
      object o = 1;
      Assert.True(o.IsSystemBased());
    }

    /// <summary>
    /// Proves that the GetPropertyNames extension method
    /// can obtain all the properties names from an object
    /// </summary>
    [Fact]
    public void GetPropertyNames()
    {
      object o = new
      {
        Name = @"",
        LastName = @""
      };
      Assert.True(o.GetPropertyNames().Length.Equals(2));
    }

    /// <summary>
    /// Proves that the IsStringCollection extension method
    /// evaluates correctly a collection struct based string object
    /// </summary>
    [Fact]
    public void IsStringCollection()
    {
      object list = new List<string>(2)
      {
        @"Israel",
        @"Chavez"
      };
      Assert.True(list.IsStringCollection());
    }

    /// <summary>
    /// Proves that the GetBytes extension method
    /// can obtain the byte array representation data from an object
    /// </summary>
    [Fact]
    public void GetBytes()
    {
      object o = new Dictionary<string, string>
      {
        {@"Israel",@"Chavez" }
      };
      Assert.True(!o.GetBytes().IsNotValid());
    }

    /// <summary>
    /// Proves that the SaveAsDataFile extension method
    /// can save the byte array representation data from an object
    /// </summary>
    [Fact]
    public async void SaveAsDataFile()
    {
      object o = new Dictionary<string, string>
      {
        {@"Israel",@"Chavez" }
      };
      string path = $@"{AppDomain.CurrentDomain.BaseDirectory}TestResults\Extensions\CommonObjects\";
      Assert.True(await o.SaveAsDataFile(path, @"Names"));
    }
  }
}
