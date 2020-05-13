using Xunit;
using System;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Word;
using DocumentFormat.OpenXml.Packaging;
using Combine.Sdk.Extensions.Word.Models;

namespace Combine.Sdk.Tests.Extensions.Word.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the list
  /// extension methods availables
  /// </summary>
  public class FactListTests
  {
    /// <summary>
    /// Creates a new instance of List tests
    /// </summary>
    public FactListTests()
    {

    }

    /// <summary>
    /// Proves that the extension method List.ToWord
    /// generates the word processing document correctly
    /// </summary>
    [Fact]
    public void GetWordProcessingDocumentFromList()
    {
      List<string> names = new List<string>(3)
      {
        @"1 Israel Chavez Gamez",
        @"2 Israel Chavez Gamez",
        @"3 Israel Chavez Gamez"
      };
      WordprocessingDocument document = names.ToWord();      
      Assert.True(document != null);
    }

    /// <summary>
    /// Proves that the extension method List.ToWord
    /// generates and saves to disk the word processing
    /// document without errors
    /// </summary>
    [Fact]
    public void SaveWordDocumentFromList()
    {
      List<string> names = new List<string>(3)
      {
        @"1 Israel Chavez Gamez",
        @"2 Israel Chavez Gamez",
        @"3 Israel Chavez Gamez"
      };
      WordDocumentConfiguration configuration = new WordDocumentConfiguration()
      {
        Title = @"Documento Reporte",
        OutPutPath = AppDomain.CurrentDomain.BaseDirectory
      };
      WordprocessingDocument document = names.ToWord(configuration);
      Assert.True(document != null);
    }
  }
}
