using System;
using System.Linq;
using System.IO.Packaging;
using DocumentFormat.OpenXml;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Excel;
using DocumentFormat.OpenXml.Packaging;
using SystemColor = System.Drawing.Color;
using DocumentFormat.OpenXml.Spreadsheet;
using Combine.Sdk.Extensions.CommonObjects;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace Combine.Sdk.Extensions.Excel
{
  /// <summary>
  /// Provides extension methods for SpreadSheetDocument type objects
  /// </summary>
  public static class SpreadSheetExtensions
  {
    /// <summary>
    /// Checks if the given SpreadSheetDocument value is null or it does not contain
    /// any WorkbookPart in it
    /// </summary>
    /// <param name="document">SpreadSheetDocument to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this SpreadsheetDocument document)
    {
      return document == null || document.WorkbookPart == null || document.WorkbookPart.WorksheetParts.Count().Equals(0);
    }

    #region Private

    /// <summary>
    /// Workbook Style reference
    /// </summary>
    private static WorkbookStylesPart StylePart { get; set; }

    /// <summary>
    /// Workbook SharedString reference
    /// </summary>
    private static SharedStringTablePart StringPart { get; set; }

    /// <summary>
    /// Adds the font color to the current
    /// style sheet and creates a cell reference style
    /// </summary>
    /// <param name="color"></param>
    private static void AddFontStyle(SystemColor color)
    {
      HexBinaryValue hex = HexBinaryValue.FromString(color.ToHexArgb());
      //Search for style
      bool exists = StylePart.Stylesheet.Fonts
        .Select(f => f as Font)
        .Where(f => f.Color != null)
        .Where(f => f.Color.Rgb.Equals(hex))
        .Any();
      if (exists)
        return;
      //Create font element
      Font font = new Font();
      font.Append(new FontSize() { Val = 11D });
      font.Append(new Color() { Rgb = hex });
      font.Append(new FontName() { Val = "Calibri" });
      font.Append(new FontFamilyNumbering() { Val = 2 });
      font.Append(new FontScheme() { Val = FontSchemeValues.Minor });
      font.Append(new Bold());
      CellFormat cellFontFormat = new CellFormat()
      {
        FontId = StylePart.Stylesheet.Fonts.Count,
        ApplyFont = true
      };
      StylePart.Stylesheet.Fonts.Append(font);
      StylePart.Stylesheet.Fonts.Count++;
      //Add the format style reference
      StylePart.Stylesheet.CellFormats.Append(cellFontFormat);
      StylePart.Stylesheet.CellFormats.Count++;
      //Save all changes
      StylePart.Stylesheet.Save();
    }

    /// <summary>
    /// Adds the background color to the current
    /// style sheet and creates a cell reference style
    /// </summary>
    /// <param name="color"></param>
    private static void AddBackgroundStyle(SystemColor color)
    {
      HexBinaryValue hex = HexBinaryValue.FromString(color.ToHexArgb());
      //Search for style
      bool exists = StylePart.Stylesheet.Fills
        .Select(f => f as Fill)
        .Where(f => f.PatternFill.ForegroundColor != null)
        .Where(f => f.PatternFill.PatternType.Equals(PatternValues.Solid))
        .Where(f => f.PatternFill.ForegroundColor.Rgb.Equals(hex))
        .Any();
      if (exists)
        return;
      //Create fill element
      Fill fill = new Fill()
      {
        PatternFill = new PatternFill()
        {
          ForegroundColor = new ForegroundColor()
          {
            Rgb = hex,
          },
          PatternType = PatternValues.Solid
        }
      };
      //Add the fill (of a background)      
      CellFormat cellFontFormat = new CellFormat()
      {
        FillId = StylePart.Stylesheet.Fills.Count,
        ApplyFill = true
      };
      StylePart.Stylesheet.Fills.Append(fill);
      StylePart.Stylesheet.Fills.Count++;
      //Add the format style reference
      StylePart.Stylesheet.CellFormats.Append(cellFontFormat);
      StylePart.Stylesheet.CellFormats.Count++;
      //Save all changes
      StylePart.Stylesheet.Save();
    }

    /// <summary>
    /// Adds the font and background color to the current
    /// style sheet and creates a cell reference style
    /// </summary>
    /// <param name="fontColor">Font color</param>
    /// <param name="backgroundColor">Background color</param>
    private static void AddFontAndBackgroundStyle(SystemColor fontColor, SystemColor backgroundColor)
    {
      uint fontId = GetFontStyleIndex(fontColor);
      uint backgroundId = GetBackgroundStyleIndex(backgroundColor) + 1;
      bool exists = StylePart.Stylesheet.CellFormats
        .Select(f => f as CellFormat)
        .Where(f => f.FontId != null && f.FillId != null)
        .Where(f => f.FontId.Equals(fontId) && f.FillId.Equals(backgroundId))
        .Any();
      if (exists)
        return;
      CellFormat cellFontFormat = new CellFormat()
      {
        FontId = fontId,
        FillId = backgroundId,
        ApplyFont = true,
        ApplyFill = true
      };
      //Add the format style reference
      StylePart.Stylesheet.CellFormats.Append(cellFontFormat);
      StylePart.Stylesheet.CellFormats.Count++;
      //Save all changes
      StylePart.Stylesheet.Save();
    }

    /// <summary>
    /// Gets the font cell reference style index
    /// position using the given Color
    /// </summary>
    /// <param name="color">Font color</param>
    /// <returns>Index position</returns>
    private static uint GetFontStyleIndex(SystemColor color)
    {
      AddFontStyle(color);
      List<Font> fonts = StylePart.Stylesheet.Fonts
        .Select(f => f as Font)
        .ToList();
      Font font = fonts
        .Where(f => f.Color != null)
        .Where(f => f.Color.Rgb.Equals(color.ToHexArgb()))
        .First();
      return (uint)fonts.IndexOf(font);
    }

    /// <summary>
    /// Gets the background cell reference style index
    /// position using the given Color
    /// </summary>
    /// <param name="color">Background color</param>
    /// <returns>Index position</returns>
    private static uint GetBackgroundStyleIndex(SystemColor color)
    {
      AddBackgroundStyle(color);
      List<Fill> fills = StylePart.Stylesheet.Fills
        .Select(f => f as Fill)
        .ToList();
      Fill fill = fills
        .Where(f => f.PatternFill.ForegroundColor != null)
        .Where(f => f.PatternFill.ForegroundColor.Rgb.Equals(color.ToHexArgb()))
        .First();
      return (uint)fills.IndexOf(fill) - 1;
    }

    /// <summary>
    /// Gets the font and background cell reference style index
    /// position using the given Colors
    /// </summary>
    /// <param name="fontColor">Font color</param>
    /// <param name="backgroundColor">Background color</param>
    /// <returns>Index position</returns>
    private static uint GetFontAndBackgroundStyleIndex(SystemColor fontColor, SystemColor backgroundColor)
    {
      AddFontAndBackgroundStyle(fontColor, backgroundColor);
      uint fontIndex = GetFontStyleIndex(fontColor);
      uint backgroundIndex = GetBackgroundStyleIndex(backgroundColor) + 1;
      List<CellFormat> formats = StylePart.Stylesheet.CellFormats
        .Select(f => f as CellFormat)
        .ToList();
      CellFormat format = formats
        .Where(f => f.FontId != null && f.FillId != null)
        .First(f => f.FontId.Equals(fontIndex) && f.FillId.Equals(backgroundIndex));
      return (uint)formats.IndexOf(format);
    }

    #endregion

    /// <summary>
    /// Creates a SpreadSheetDocument from the specified
    /// template
    /// </summary>
    /// <param name="template">Template url</param>
    /// <returns>SpreadSheetDocument reference</returns>
    public static SpreadsheetDocument ExcelDocument(string template = null)
    {
      template = template ?? $@"{AppDomain.CurrentDomain.BaseDirectory}Templates\Report.xlsx";
      if (!template.IsFilePath(out Uri path))
        return null;
      //Initialize via report template
      SpreadsheetDocument document = SpreadsheetDocument.CreateFromTemplate(template);
      //Compress using maximum value possible
      document.CompressionOption = CompressionOption.Maximum;
      //Bond the workbook to the document or else create a new workspace
      WorkbookPart book = document.WorkbookPart ?? document.AddWorkbookPart();
      //Initialize workspace
      WorksheetPart workSpace = document.WorkbookPart.WorksheetParts.FirstOrDefault() ?? document.WorkbookPart.AddNewPart<WorksheetPart>();
      //Initialize worksheet
      if (workSpace.Worksheet == null)
        workSpace.Worksheet = new Worksheet();
      //Initialize worksheet stylesheet
      WorkbookStylesPart style = document.WorkbookPart.WorkbookStylesPart ?? document.WorkbookPart.AddNewPart<WorkbookStylesPart>();
      if (style.Stylesheet == null)
        style.Stylesheet = new Stylesheet();
      //Reserved fills Excel requires by default this two fills
      style.Stylesheet.Fills = new Fills();
      //Empty cell fill
      style.Stylesheet.Fills.AppendChild(new Fill
      {
        PatternFill = new PatternFill
        {
          PatternType = PatternValues.None
        }
      });
      //Weird grey cell fill
      style.Stylesheet.Fills.AppendChild(new Fill
      {
        PatternFill = new PatternFill
        {
          PatternType = PatternValues.Gray125
        }
      });
      style.Stylesheet.Fills.Count = 2;
      //Reserved Cell formats Excel requires by default this two formats
      style.Stylesheet.CellFormats = new CellFormats();
      style.Stylesheet.CellFormats.AppendChild(new CellFormat());
      style.Stylesheet.CellFormats.Count = 1;
      //Fonts
      style.Stylesheet.Fonts = new Fonts();
      Font font = new Font();
      font.Append(new FontSize() { Val = 11D });
      font.Append(new Color() { Rgb = SystemColor.Black.ToHexArgb() });
      font.Append(new FontName() { Val = "Calibri" });
      font.Append(new FontFamilyNumbering() { Val = 2 });
      font.Append(new FontScheme() { Val = FontSchemeValues.Minor });
      font.Append(new Bold());
      style.Stylesheet.Fonts.AppendChild(font);
      style.Stylesheet.Fonts.Count = 1;
      //Save all reserved formats
      style.Stylesheet.Save();
      StylePart = style;
      //Share string table part
      StringPart = book.GetPartsOfType<SharedStringTablePart>()
        .FirstOrDefault();
      //Add document properties
      document.PackageProperties.Title = @"Reporte de Listado";
      document.PackageProperties.Creator = @"Israel Ch";
      document.PackageProperties.Category = @"Reporte";
      document.PackageProperties.Description = $@"Listado de entidades";
      document.PackageProperties.LastModifiedBy = @"Israel Ch";
      //Add extended document properties
      if (document.ExtendedFilePropertiesPart == null)
        document.AddExtendedFilePropertiesPart();
      document.ExtendedFilePropertiesPart.Properties = new Properties
      {
        Company = new Company(@"izrra.ch"),
        Application = new Application("Extensions.Excel")
      };
      //Save all changes
      document.Save();
      return document;
    }

    /// <summary>
    /// Adds a new Sheet to the document
    /// </summary>
    /// <param name="document">SpreadSheetDocument reference</param>
    /// <param name="name">Sheet name</param>
    public static void AddSheet(this SpreadsheetDocument document, string name)
    {
      //Check whether the document or the sheet name is a non-valid to work with object value
      if (document.IsNotValid() || name.IsNotValid())
        return;
      //Search for sheet collection reference
      Sheets sheets = document
        .WorkbookPart
        .Workbook
        .GetFirstChild<Sheets>();
      //If there already exists a sheet by the given name, there is no need to create a new one.
      if (sheets.Descendants<Sheet>().Any(s => s.Name.Equals(name)))
        return;
      //Get the last sheet pushed into the sheets collection
      Sheet last = sheets
        .Descendants<Sheet>()
        .OrderByDescending(s => s.SheetId)
        .FirstOrDefault();
      //Set current workspace reference
      WorksheetPart workSpace = document.WorkbookPart.AddNewPart<WorksheetPart>();
      //Add new worksheet with data
      workSpace.Worksheet = new Worksheet(new SheetData());
      //Stablish index value
      uint index = last == null ? 1 : last.SheetId.Value;
      index = last == null ? 1 : index + 1;
      //Create the new sheet
      Sheet sheet = new Sheet()
      {
        Id = document.WorkbookPart.GetIdOfPart(workSpace),
        SheetId = index,
        Name = name
      };
      //Add new sheet to the collection
      sheets.Append(sheet);
    }

    /// <summary>
    /// Removes the sheet from the document
    /// using  the given sheet unique name
    /// </summary>
    /// <param name="document">SpreadSheetDocument reference</param>
    /// <param name="name">Sheet name</param>
    public static void RemoveSheet(this SpreadsheetDocument document, string name)
    {
      //Check whether the document or the sheet name is a non-valid to work with object value
      if (document.IsNotValid() || name.IsNotValid())
        return;
      //Search for sheet collection reference
      Sheets sheets = document
        .WorkbookPart
        .Workbook
        .GetFirstChild<Sheets>();
      //If there is not any existing sheet with the given name, there is nothing to remove.
      if (!sheets.Descendants<Sheet>().Any(s => s.Name.Equals(name)))
        return;
      Sheet sheet = sheets.Descendants<Sheet>()
        .First(s => s.Name.Equals(name));
      int index = sheets
        .ToList()
        .IndexOf(sheet);
      sheet.RemoveAllChildren();
      sheet.Remove();
    }

    /// <summary>
    /// Gets the sheet data from the document
    /// using the given sheet unique name
    /// </summary>
    /// <param name="document">SpreadSheetDocument reference</param>
    /// <param name="name">Sheet name</param>
    /// <returns>SheetData reference</returns>
    public static SheetData GetSheetData(this SpreadsheetDocument document, string name)
    {
      //Check whether the document or the sheet name is a non-valid to work with object value
      if (document.IsNotValid() || name.IsNotValid())
        return null;
      //Search the sheet by the unique name given
      Sheet sheet = document.WorkbookPart.Workbook
        .GetFirstChild<Sheets>()
        .Descendants<Sheet>()
        .Where(s => s.Name.Equals(name))
        .FirstOrDefault();
      if (sheet == null)
        return null;
      //Get the workspace corresponding to the sheet founded
      WorksheetPart workSpace = document.WorkbookPart.GetPartById(sheet.Id) as WorksheetPart;
      //return founded sheet data
      return workSpace.Worksheet
        .Descendants<SheetData>()
        .First();
    }

    /// <summary>
    /// Sets the font color to the cell
    /// </summary>
    /// <param name="cell">Cell reference</param>
    /// <param name="fontColor">Font color</param>
    public static void SetColor(this Cell cell, SystemColor fontColor)
    {
      if (cell.IsNotValid() || fontColor.IsNotValid())
        return;
      cell.StyleIndex = GetFontStyleIndex(fontColor);
    }

    /// <summary>
    /// Sets the background color to the cell
    /// </summary>
    /// <param name="cell">Cell reference</param>
    /// <param name="color">Background color</param>
    public static void SetBackgroundColor(this Cell cell, SystemColor color)
    {
      if (cell.IsNotValid() || color.IsNotValid())
        return;
      cell.StyleIndex = GetBackgroundStyleIndex(color);
    }

    /// <summary>
    /// Sets the font and background color to the cell
    /// </summary>
    /// <param name="cell">Cell reference</param>
    /// <param name="fontColor">Font color</param>
    /// <param name="backgroundColor">Background color</param>
    public static void SetColor(this Cell cell, SystemColor fontColor, SystemColor backgroundColor)
    {
      if (cell.IsNotValid() || fontColor.IsNotValid() || backgroundColor.IsNotValid()) return;
      cell.StyleIndex = GetFontAndBackgroundStyleIndex(fontColor, backgroundColor);
    }

    /// <summary>
    /// Sets the font color to all the
    /// cells in the row
    /// </summary>
    /// <param name="row">Row reference</param>
    /// <param name="fontColor">Font color</param>
    public static void SetColor(this Row row, SystemColor fontColor)
    {
      if (row.IsNotValid() || !row.Any()) return;
      row.Descendants<Cell>()
        .ToList()
        .ForEach(c => c.SetColor(fontColor));
    }

    /// <summary>
    /// Sets the background color to all the
    /// cells in the row
    /// </summary>
    /// <param name="row">Row reference</param>
    /// <param name="color">Background color</param>
    public static void SetBackgroundColor(this Row row, SystemColor color)
    {
      if (row.IsNotValid() || !row.Any()) return;
      row.Descendants<Cell>()
        .ToList()
        .ForEach(c => c.SetBackgroundColor(color));
    }

    /// <summary>
    /// Sets the font and background color to all the
    /// cells in the row
    /// </summary>
    /// <param name="row">Row reference</param>
    /// <param name="fontColor">Font color</param>
    /// <param name="backgroundColor">Background color</param>
    public static void SetColor(this Row row, SystemColor fontColor, SystemColor backgroundColor)
    {
      if (row.IsNotValid() || fontColor.IsNotValid() || backgroundColor.IsNotValid()) return;
      row.Descendants<Cell>()
        .ToList()
        .ForEach(c => c.SetColor(fontColor, backgroundColor));
    }

    /// <summary>
    /// Replaces all the key tags inside the sheet using
    /// the given values
    /// </summary>
    /// <param name="sheet">SheetData reference</param>
    /// <param name="values">Object values</param>
    /// <returns>Task State</returns>
    public static async Task SetTemplateValues(this SheetData sheet, IEnumerable<KeyValuePair<string, object>> values)
    {
      if (sheet.IsNotValid() || values == null || !values.Any())
        return;
      for (int x = 0; x < values.Count(); x++)
      {
        string name = values.ElementAt(x).Key;
        object value = values.ElementAt(x).Value;
        //Replace all cell values
        List<Cell> cells = sheet.Descendants<Cell>()
          .Where(c => c.CellValue.Text.Equals(name))
          .ToList();
        foreach (Cell c in cells)
          await c.SetValue(value);
        //Replace all shared string values
        List<OpenXmlElement> elements = StringPart.SharedStringTable
         .Where(e => e.InnerText.Equals(name))
         .ToList();
        elements.ForEach(e => e.InnerXml = e.InnerXml.Replace(name, value.ToString()));
      }
      StringPart.SharedStringTable.Save();
    }

    /// <summary>
    /// Adjust the column width to its best fit
    /// </summary>
    /// <param name="document">SpreadSheetDocument reference</param>
    public static void AutoAdjustWidth(this SpreadsheetDocument document)
    {
      if (document.IsNotValid()) return;
      //Maximum digit with per cell ~96dpi (most common display)
      double max = 8;
      //Add the columns for each worksheet that has any valid sheet data
      document.WorkbookPart.WorksheetParts
        .Select(wsp => wsp.Worksheet)
        .Where(wsp => wsp.Descendants<SheetData>().Any(sd => sd.Descendants<Row>().Any()))
        .ToList()
        .ForEach(ws =>
        {
          //Current SheetData reference
          SheetData sd = ws.GetFirstChild<SheetData>();
          //Find the widest row
          Row row = sd.Descendants<Row>().First();
          //Array of lengths (max length per cell per row)
          int[] lengths = new int[row.Descendants<Cell>().Count()];
          for (int i = 0; i < lengths.Length; i++)
            lengths[i] = 0;
          //Find the widest cell in all rows
          foreach (Row r in sd.Descendants<Row>())
          {
            for (int i = 0; i < lengths.Length; i++)
            {
              Cell cell = r.Descendants<Cell>().ElementAt(i);
              int length;
              //Take the cell length value from the shared string table if the cell has its value in the shared string table
              if (cell.DataType.Equals(CellValues.SharedString) && cell.CellValue.Text.IsNumber())
              {
                int index = Convert.ToInt32(cell.CellValue.Text);
                length = StringPart.SharedStringTable.ElementAt(index).InnerText.Length;
              }
              else
              {
                //Take the length from the cell value directly
                length = cell.CellValue.Text.Length;
              }
              //Set the current length for the cell at row if the length at the current position is less than the current
              if (lengths[i] < length)
                lengths[i] = length;
            }
          }
          //Get or create the custom columns reference
          Columns columns = ws.GetFirstChild<Columns>() ?? new Columns();
          //Adjust the columns width
          for (uint i = 1; i <= lengths.Length; i++)
          {
            double width = ((lengths[i - 1] * max + 5) / max * 256) / 256;
            columns.Append(new Column()
            {
              Min = i,
              Max = i,
              Width = width + 1,
              CustomWidth = true
            });
          }
          //Add the columns if they do not exists
          if (ws.GetFirstChild<Columns>() == null)
            ws.InsertBefore(columns, sd);
          //Save all changes made
          ws.Save();
        });
      document.Save();
    }
  }
}
