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
  /// Provides a mechanism to test all the SheetData
  /// extension methods availables
  /// </summary>
  public class FactSheetDataTests
  {
    /// <summary>
    /// Creates a new sheet data test instance
    /// </summary>
    public FactSheetDataTests()
    {
    }

    /// <summary>
    /// Proves that the extension method IsNotValid
    /// evaluates a non-valid to work with sheet data
    /// reference correctly
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      SheetData sheet = null;
      Assert.True(sheet.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method GetRow
    /// gets an existing row from a sheet data 
    /// reference correctly
    /// </summary>
    [Fact]
    public void GetRow()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      Row row = sheet.GetRow(0);
      Assert.True(!row.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method SwapColumns
    /// replace two entire columns by index position
    /// in the sheet data correctly
    /// </summary>
    [Fact]
    public void SwapColumns()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      sheet.SwapColumns(0, 1);
      Assert.True(!document.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method AddRows
    /// adds the collection data as new rows
    /// inside the sheet data correctly
    /// </summary>
    [Fact]
    public async Task AddRows()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      List<object[]> data = new List<object[]>(1)
      {
        new object[]{@"Israel", DateTime.Now.AddYears(-27), 27, @"izrra@mail.com"}
      };
      SheetData sheet = document.GetSheetData(@"Report");
      await sheet.AddRows(data);
      Row row = sheet.GetRow(0);
      Assert.True(!row.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method AddRows
    /// adds the collection data as new rows
    /// inside the sheet data correctly
    /// </summary>
    [Fact]
    public void AddDataTableAsRows()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();

    }
  }
}
