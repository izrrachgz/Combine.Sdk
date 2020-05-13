using Xunit;
using System.IO;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.CommonObjects.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the extension
  /// methods availables for File type objects
  /// </summary>
  public class FactFileTests
  {
    /// <summary>
    /// Creates a new instance of Fact File Tests
    /// </summary>
    public FactFileTests()
    {

    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with FileInfo object
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      FileInfo info = new FileInfo(@"invalid-path");
      Assert.True(info.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with FileInfo Array object
    /// </summary>
    [Fact]
    public void IsNotValidFileInfoArray()
    {
      FileInfo[] array = new FileInfo[1]{
        new FileInfo(@"invalid-path")
      };
      Assert.True(array.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with FileInfo List object
    /// </summary>
    [Fact]
    public void IsNotValidFileInfoList()
    {
      List<FileInfo> list = new List<FileInfo>(1){
        new FileInfo(@"invalid-path")
      };
      Assert.True(list.IsNotValid());
    }

    /// <summary>
    /// Proves that the IsNotValid extension method
    /// evaluates correctly an non-valid to work with FileInfo Array List object
    /// </summary>
    [Fact]
    public void IsNotValidFileInfoArrayList()
    {
      List<FileInfo[]> list = new List<FileInfo[]>(1){
        new FileInfo[1]
        {
          new FileInfo(@"invalid-path")
        }
      };
      Assert.True(list.IsNotValid());
    }
  }
}
