using Xunit;
using System;
using System.IO;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Word;
using DocumentFormat.OpenXml.Packaging;

namespace Combine.Sdk.Tests.Extensions.Word.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the WordProcessingDocument
  /// extension methods availables
  /// </summary>
  public class FactWordProcessingDocumentTests
  {
    /// <summary>
    /// Creates a new instance of WordProcessingDocument tests
    /// </summary>
    public FactWordProcessingDocumentTests()
    {

    }

    /// <summary>
    /// Proves that the extension method Word.AddImage
    /// adds to the word processing document an image correctly
    /// </summary>
    [Fact]
    public void AddImage()
    {
      List<string> paragraphs = new List<string>(3)
      {
        @"Parrafo 1",
        @"Parrafo 2",
        @"Parrafo 3"
      };
      WordprocessingDocument word = paragraphs.ToWord();
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
      List<string> paragraphs = new List<string>(3)
      {
        @"Parrafo 1",
        @"Parrafo 2",
        @"Parrafo 3"
      };
      WordprocessingDocument word = paragraphs.ToWord();
      Assert.True(word.AddParagraph(@"Parrafo 4"));
    }
  }
}
