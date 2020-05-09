using System;
using System.Linq;
using System.IO.Packaging;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using Combine.Sdk.Extensions.Word.Models;
using Combine.Sdk.Extensions.CommonObjects;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace Combine.Sdk.Extensions.Word
{
  /// <summary>
  /// Provides extension methods for List type objects
  /// </summary>
  public static class ListExtensions
  {
    /// <summary>
    /// Gets an word processing document with the given list information
    /// as paragraphs
    /// </summary>    
    /// <param name="list">Reference to list</param>
    /// <param name="configuration">Word document configuration</param>
    /// <returns>WordProcessingDocument</returns>
    public static WordprocessingDocument ToWord(this IEnumerable<string> list, WordDocumentConfiguration configuration = null)
    {
      if (list.ToList().IsNotValid()) return null;
      string template = $@"{AppDomain.CurrentDomain.BaseDirectory}Extensions\Word\Templates\Document.docx";
      //Initialize via report template
      WordprocessingDocument document = WordprocessingDocument.CreateFromTemplate(template);
      try
      {
        //Compress using maximum value possible      
        document.CompressionOption = CompressionOption.Maximum;
        //Bond the body of the document or else create a new one
        Body body = document.MainDocumentPart.Document.Body;
        //Add text to the body via paragraphs
        for (int x = 0; x < list.ToList().Count; x++)
        {
          Paragraph paragraph = body.AppendChild(new Paragraph());
          Run run = paragraph.AppendChild(new Run());
          run.AppendChild(new Text(list.ElementAt(x)));
        }
        //Set default configuration
        configuration = configuration ?? new WordDocumentConfiguration();
        //Add document properties
        document.PackageProperties.Title = configuration.Title ?? @"Documento";
        document.PackageProperties.Creator = @"Israel Ch";
        document.PackageProperties.Category = @"Documento";
        document.PackageProperties.Description = $@"Documento informativo";
        document.PackageProperties.LastModifiedBy = @"Israel Ch";
        //Add extended document properties
        if (document.ExtendedFilePropertiesPart == null) document.AddExtendedFilePropertiesPart();
        document.ExtendedFilePropertiesPart.Properties = new Properties();
        document.ExtendedFilePropertiesPart.Properties.Company = new Company(@"izrra.ch");
        document.ExtendedFilePropertiesPart.Properties.Application = new Application("Extensions.Word");
        //Save all changes
        document.Save();
        //Save document in the specified output path
        if (configuration.OutPutPath.IsDirectoryPath())
        {
          document.SaveAs(configuration.OutPutPath + $@"{Guid.NewGuid():N}.docx");
          document.Close();
          document.Dispose();
        }
      }
      catch (Exception)
      {
        document = null;
      }
      return document;
    }
  }
}
