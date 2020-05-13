using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Excel;
using DocumentFormat.OpenXml.Packaging;
using SystemColor = System.Drawing.Color;
using DocumentFormat.OpenXml.Spreadsheet;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Tests.Extensions.Excel.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the SpreadSheet
  /// extension methods availables
  /// </summary>
  public class FactSpreadSheetTests
  {
    /// <summary>
    /// Creates a new instance of List tests
    /// </summary>
    public FactSpreadSheetTests()
    {

    }

    /// <summary>
    /// Proves that the extension method IsNotValid
    /// evaluates a non-valid to work with
    /// spread sheet document correctly
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      SpreadsheetDocument document = null;
      Assert.True(document.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method ExcelDocument
    /// generates a spread sheet document correctly
    /// </summary>
    [Fact]
    public void ExcelDocument()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      Assert.True(!document.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method AddSheet
    /// generates and saves all changes made in the 
    /// spread sheet documents correctly
    /// </summary>
    [Fact]
    public void AddSheet()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      document.AddSheet(@"NewSheet");
      SheetData sheet = document.GetSheetData(@"NewSheet");
      Assert.True(!sheet.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method RemoveSheet
    /// removes a sheet data from the 
    /// spread sheet document correctly
    /// </summary>
    [Fact]
    public void RemoveSheet()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      document.RemoveSheet(@"Report");
      SheetData sheet = document.GetSheetData(@"Report");
      Assert.True(sheet.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method GetSheetData
    /// gets an existing sheet data from the 
    /// spread sheet document correctly
    /// </summary>
    [Fact]
    public void GetSheetData()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      Assert.True(!sheet.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method SetColor
    /// sets the specified font color to the 
    /// cell reference in the spread sheet document
    /// correctly
    /// </summary>
    [Fact]
    public void SetCellFontColor()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      Cell cell = sheet.GetRow(0)
        .GetCell(0);
      cell.SetColor(SystemColor.Purple);
      Assert.True(cell.StyleIndex >= 0);
    }

    /// <summary>
    /// Proves that the extension method SetColor
    /// sets the specified background color to the 
    /// cell reference in the spread sheet document
    /// correctly
    /// </summary>
    [Fact]
    public void SetCellBackgroundColor()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      Cell cell = sheet.GetRow(0)
        .GetCell(0);
      cell.SetBackgroundColor(SystemColor.Purple);
      Assert.True(cell.StyleIndex >= 0);
    }

    /// <summary>
    /// Proves that the extension method SetColor
    /// sets the specified font and background color to the 
    /// cell reference in the spread sheet document
    /// correctly
    /// </summary>
    [Fact]
    public void SetCellFontAndBackgroundColor()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      Cell cell = sheet.GetRow(0)
        .GetCell(0);
      cell.SetColor(SystemColor.White, SystemColor.Purple);
      Assert.True(cell.StyleIndex >= 0);
    }

    /// <summary>
    /// Proves that the extension method SetColor
    /// sets the specified font color to the 
    /// row reference in the spread sheet document
    /// correctly
    /// </summary>
    [Fact]
    public void SetRowFontColor()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      Row row = sheet.GetRow(0);
      row.SetColor(SystemColor.Purple);
      Assert.True(row.Descendants<Cell>().ToList().TrueForAll(c => c.StyleIndex >= 0));
    }

    /// <summary>
    /// Proves that the extension method SetColor
    /// sets the specified background color to the 
    /// row reference in the spread sheet document
    /// correctly
    /// </summary>
    [Fact]
    public void SetRowBackgroundColor()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      Row row = sheet.GetRow(0);
      row.SetBackgroundColor(SystemColor.Purple);
      Assert.True(row.Descendants<Cell>().ToList().TrueForAll(c => c.StyleIndex >= 0));
    }

    /// <summary>
    /// Proves that the extension method SetColor
    /// sets the specified font and background color to the 
    /// row reference in the spread sheet document
    /// correctly
    /// </summary>
    [Fact]
    public void SetRowFontAndBackgroundColor()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      Row row = sheet.GetRow(0);
      row.SetColor(SystemColor.White, SystemColor.Purple);
      Assert.True(row.Descendants<Cell>().ToList().TrueForAll(c => c.StyleIndex >= 0));
    }

    /// <summary>
    /// Proves that the extension method SetTemplateValues
    /// replaces all the key specified cell values for its
    /// corresponding values in the sheet data correctly
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task SetTemplateValues()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Entity");
      List<KeyValuePair<string, object>> data = new List<KeyValuePair<string, object>>(3)
      {
        new KeyValuePair<string, object>(@"{{Name}}", @"Israel"),
        new KeyValuePair<string, object>(@"{{Age}}", DateTime.Now.AddYears(-27)),
        new KeyValuePair<string, object>(@"{{Email}}", @"izrra.ch@mail.com")
      };
      await sheet.SetTemplateValues(data);
      Assert.True(!document.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method AutoAdjustWidth
    /// adjusts automatically all the columns in the
    /// spread sheet documents without corrupting it
    /// </summary>
    [Fact]
    public void AutoAdjustWidth()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      document.AutoAdjustWidth();
      Assert.True(!document.IsNotValid());
    }
  }
}
