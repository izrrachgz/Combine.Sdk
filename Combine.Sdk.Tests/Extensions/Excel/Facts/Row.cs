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
  /// Provides a mechanism to test all the Row
  /// extension methods availables
  /// </summary>
  public class RowTests
  {
    /// <summary>
    /// Creates a new row test instance
    /// </summary>
    public RowTests() { }

    /// <summary>
    /// Proves that the extension method IsNotValid
    /// evaluates a non-valid to work with row
    /// reference correctly
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      Row row = null;
      Assert.True(row.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method GetCell
    /// gets an existing cell from a row 
    /// reference correctly
    /// </summary>
    [Fact]
    public void GetCell()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      Row row = sheet.GetRow(0);
      Cell cell = row.GetCell(0);
      Assert.True(!cell.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method SwapColumns
    /// replace two entire columns by index position
    /// in the row correctly
    /// </summary>
    [Fact]
    public void SwapColumns()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      Row row = sheet.GetRow(0);
      row.SwapColumns(0, 1);
      Assert.True(!document.IsNotValid());
    }
  }
}
