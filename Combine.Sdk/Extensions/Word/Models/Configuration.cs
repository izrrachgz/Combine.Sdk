namespace Combine.Sdk.Extensions.Word.Models
{
  /// <summary>
  /// Provides a data model representation for a word document configuration
  /// </summary>
  public class WordDocumentConfiguration
  {
    /// <summary>
    /// Document title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Output filepath
    /// </summary>
    public string OutPutPath { get; set; }

    /// <summary>
    /// Creates a new instance of word document configuration
    /// </summary>
    public WordDocumentConfiguration()
    {
      Title = @"Documento";
      OutPutPath = null;
    }
  }
}
