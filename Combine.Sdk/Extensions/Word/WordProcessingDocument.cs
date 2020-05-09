using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using A = DocumentFormat.OpenXml.Drawing;
using Combine.Sdk.Extensions.CommonObjects;
using DocumentFormat.OpenXml.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;

namespace Combine.Sdk.Extensions.Word
{
  /// <summary>
  /// Provides extension methods for WordProcessingDocument type objects
  /// </summary>
  public static class WordProcessingDocumentExtensions
  {
    /// <summary>
    /// Checks wheter the given word document is null or its main and document
    /// part are null also
    /// </summary>
    /// <param name="word">Word document reference</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this WordprocessingDocument word)
    {
      return word == null || word.MainDocumentPart == null || word.MainDocumentPart.Document == null;
    }

    /// <summary>
    /// Adds an image element into the body of the given word document
    /// </summary>
    /// <param name="word">Word document reference</param>
    /// <param name="relationshipId">Unique identifier for image part id</param>
    private static void AddImageToBody(WordprocessingDocument word, string relationshipId)
    {
      Guid name = Guid.NewGuid();
      // Define the reference of the image.
      Drawing element = new Drawing(
        new DW.Inline(
          new DW.Extent()
          {
            Cx = 990000L,
            Cy = 792000L
          },
          new DW.EffectExtent()
          {
            LeftEdge = 0L,
            TopEdge = 0L,
            RightEdge = 0L,
            BottomEdge = 0L
          },
          new DW.DocProperties()
          {
            Id = 1U,
            Name = name.ToString(@"D")
          },
          new DW.NonVisualGraphicFrameDrawingProperties(
            new A.GraphicFrameLocks()
            {
              NoChangeAspect = true
            }
          ),
          new A.Graphic(
            new A.GraphicData(
              new PIC.Picture(
                new PIC.NonVisualPictureProperties(
                  new PIC.NonVisualDrawingProperties()
                  {
                    Id = 0U,
                    Name = $@"{name:D}.jpg"
                  },
                  new PIC.NonVisualPictureDrawingProperties()
                ),
                new PIC.BlipFill(
                  new A.Blip(
                    new A.BlipExtensionList(
                      new A.BlipExtension()
                      {
                        Uri = name.ToString(@"B")
                      }
                    )
                  )
                  {
                    Embed = relationshipId,
                    CompressionState = A.BlipCompressionValues.Print
                  },
                  new A.Stretch(
                    new A.FillRectangle()
                  )
                ),
                new PIC.ShapeProperties(
                  new A.Transform2D(
                    new A.Offset()
                    {
                      X = 0L,
                      Y = 0L
                    },
                    new A.Extents()
                    {
                      Cx = 990000L,
                      Cy = 792000L
                    }
                  ),
                  new A.PresetGeometry(
                    new A.AdjustValueList()
                  )
                  {
                    Preset = A.ShapeTypeValues.Rectangle
                  }
                )
              )
            )
            {
              Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture"
            }
           )
         )
        {
          DistanceFromTop = 0U,
          DistanceFromBottom = 0U,
          DistanceFromLeft = 0U,
          DistanceFromRight = 0U,
          EditId = "50D07946"
        });
      // Append the reference to body, the element should be in a Run.
      word.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
    }

    /// <summary>
    /// Adds an image element into the body of the given word document
    /// </summary>
    /// <param name="word">Word document reference</param>
    /// <param name="stream">Stream containing the image data</param>
    /// <param name="imageType">Type of image</param>
    /// <returns>True or False</returns>
    public static bool AddImage(this WordprocessingDocument word, Stream stream, ImagePartType imageType = ImagePartType.Jpeg)
    {
      if (word.IsNotValid() || stream.IsNotValid()) return false;
      try
      {
        MainDocumentPart main = word.MainDocumentPart;
        ImagePart imagePart = main.AddImagePart(imageType);
        using (Stream s = stream)
        {
          imagePart.FeedData(s);
        }
        AddImageToBody(word, main.GetIdOfPart(imagePart));
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    /// <summary>
    /// Adds a text value as paragraph into the body of the given word document
    /// </summary>
    /// <param name="word">Word document reference</param>
    /// <param name="text">Paragraph text</param>
    /// <returns>True or False</returns>
    public static bool AddParagraph(this WordprocessingDocument word, string text)
    {
      if (word.IsNotValid() || text.IsNotValid()) return false;
      try
      {
        //Bond the body of the document or else create a new one
        Body body = word.MainDocumentPart.Document.Body;
        //Add text to the body via paragraphs      
        Paragraph paragraph = body.AppendChild(new Paragraph());
        Run run = paragraph.AppendChild(new Run());
        run.AppendChild(new Text(text));
        word.Save();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    /// <summary>
    /// Adds a set of paragraphs into the body of the given word document
    /// </summary>
    /// <param name="word">Word document reference</param>
    /// <param name="list">Collection of paragraphs</param>
    /// <returns>True or False</returns>
    public static bool AddParagraphs(this WordprocessingDocument word, List<string> list)
    {
      if (word.IsNotValid() || list.IsNotValid()) return false;
      for (int x = 0; x < list.Count; x++)
      {
        word.AddParagraph(list.ElementAt(x));
      }
      return true;
    }
  }
}
