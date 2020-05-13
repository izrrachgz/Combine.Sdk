using Xunit;
using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Excel;
using DocumentFormat.OpenXml.Packaging;
using SystemColor = System.Drawing.Color;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Combine.Sdk.Tests.Extensions.Excel.Theories
{
  /// <summary>
  /// Provides a mechanism to test all the list
  /// extension methods availables
  /// </summary>
  public class TheorieSpreadSheetTests
  {
    /// <summary>
    /// Creates a new instance of List tests
    /// </summary>
    public TheorieSpreadSheetTests()
    {

    }

    /// <summary>
    /// Proves that the extension method ExcelDocument
    /// generates and saves all changes made in the 
    /// spread sheet documents correctly
    /// </summary>
    /// <param name="sheets">Number of sheets to add</param>
    /// <param name="records">Number of records to add for each sheet</param>
    /// <returns></returns>
    [Theory, InlineData(1, 1000, 1)]
    public async Task CreateReport(byte sheets = 1, int records = 1000, byte seconds = 1)
    {
      object[] h = new object[] { @"Name", @"BirthDate", @"Age", @"Email Address" };
      List<object[]> headers = new List<object[]>(1) { h };
      object[] i = new object[] { @"Israel Chavez Gamez", DateTime.Now.AddYears(-27), 27, @"izrra.ch@icloud.com" };
      List<object[]> data = Enumerable.Repeat(i, records).ToList();
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      for (byte x = 0; x < sheets; x++)
      {
        document.AddSheet($@"Sheet{x}");
        SheetData sheet = document.GetSheetData($@"Sheet{x}");
        await sheet.AddRows(headers);
        await sheet.AddRows(data);
        sheet.GetRow(0).SetColor(SystemColor.White, SystemColor.Purple);
      }
      stopwatch.Stop();
      document.AutoAdjustWidth();
      document.SaveAs($@"{AppDomain.CurrentDomain.BaseDirectory}HugeReport.xlsx");
      Assert.True(stopwatch.Elapsed.TotalSeconds <= seconds);
    }
  }
}
