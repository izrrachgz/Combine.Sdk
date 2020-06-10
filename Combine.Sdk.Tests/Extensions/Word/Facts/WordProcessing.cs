using Xunit;
using System;
using System.IO;
using Combine.Sdk.Extensions.Word;
using DocumentFormat.OpenXml.Packaging;

namespace Combine.Sdk.Tests.Extensions.Word.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the WordProcessingDocument
  /// extension methods availables
  /// </summary>
  public class FactWordProcessingTests
  {
    /// <summary>
    /// Creates a new instance of WordProcessingDocument tests
    /// </summary>
    public FactWordProcessingTests()
    {

    }

    /// <summary>
    /// Proves that the extension method Word.AddImage
    /// adds to the word processing document an image correctly
    /// </summary>
    [Fact]
    public void AddImage()
    {
      WordprocessingDocument word = WordProcessingExtensions.WordDocument();
      using (FileStream fs = File.OpenRead($@"{AppDomain.CurrentDomain.BaseDirectory}\Extensions\Word\Templates\Example.JPG"))
      {
        Assert.True(word.AddImage(fs));
        word.SaveAs($@"{AppDomain.CurrentDomain.BaseDirectory}\TestResults\Extensions\Word\ImageDocument.docx");
      }
    }

    /// <summary>
    /// Proves that the extension method Word.AddParagraph
    /// adds to the word processing document an text paragraph correctly
    /// </summary>
    [Fact]
    public void AddParagraph()
    {
      WordprocessingDocument word = WordProcessingExtensions.WordDocument();
      word.AddParagraph(@"Parrado 1");
      Assert.True(!word.IsNotValid());
    }
  }
}
