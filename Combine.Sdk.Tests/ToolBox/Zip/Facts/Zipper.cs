using Xunit;
using System;
using System.IO;
using System.Threading.Tasks;
using Combine.Sdk.ToolBox.Zip;
using Combine.Sdk.Data.Definitions.Response;

namespace Combine.Sdk.Tests.ToolBox.Zip.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the Zipper instance available
  /// </summary>
  public class ZipperTests
  {
    /// <summary>
    /// Zipper toolbox instance
    /// </summary>
    private Zipper Zipper { get; }

    /// <summary>
    /// Creates a new Zipper tests instance
    /// </summary>
    public ZipperTests()
    {
      Zipper = new Zipper();
    }

    /// <summary>
    /// Proves that the ZipFile method 
    /// compress an existing file correctly
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task ZipFile()
    {
      FileInfo info = new FileInfo($@"{AppDomain.CurrentDomain.BaseDirectory}ToolBox\Http\HttpConfig.json");
      BasicResponse result = await Zipper.Zip(info);
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the ZipFiles method
    /// compress a collection of existing
    /// files correctly
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task ZipFiles()
    {
      string[] files = new string[3]{
        $@"{AppDomain.CurrentDomain.BaseDirectory}ToolBox\Http\HttpConfig.json",
        $@"{AppDomain.CurrentDomain.BaseDirectory}ToolBox\Replacer\Templates\TextFile.txt",
        $@"{AppDomain.CurrentDomain.BaseDirectory}Extensions\Excel\Templates\Report.xlsx"
      };
      BasicResponse result = await Zipper.ZipFiles(files);
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the ZipDirectory method
    /// compress a directory and its files
    /// in a recursively way correctly
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task ZipDirectory()
    {
      BasicResponse result = await Zipper.ZipDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}ToolBox");
      Assert.True(result.Correct);
    }
  }
}
