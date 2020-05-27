using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Combine.Sdk.ToolBox.Html;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Json;
using Combine.Sdk.Extensions.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Definitions.Response;
using Combine.Sdk.Data.Definitions.Collection;
using Combine.Sdk.Storage.DataProvider.SqlServer.Commands;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;

namespace Combine.Sdk.Tests.ToolBox.Html.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the GridBuilder instance available
  /// </summary>
  public class GridBuilderTests
  {
    /// <summary>
    /// Grid builder reference
    /// </summary>
    private GridBuilder GridBuilder { get; }

    /// <summary>
    /// Sql connection string
    /// </summary>
    private string ConnectionString { get; }

    /// <summary>
    /// Creates a new grid builder tests instance
    /// </summary>
    public GridBuilderTests()
    {
      GridBuilder = new GridBuilder();
      JsonLoader<Configuration> config = new JsonLoader<Configuration>($@"{AppDomain.CurrentDomain.BaseDirectory}Storage\SqlServer\DataProvider.json");
      config.Load()
        .Wait();
      ConnectionString = config.Instance.Values
        .GetFirst<string>(@"ConnectionString");
    }

    /// <summary>
    /// Proves that the HtmlTable method
    /// converts a ResultTable object instance
    /// into an html table string
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ResultTableHtmlTable()
    {
      SqlServerCommand command = new SqlServerCommand(ConnectionString);
      ComplexResponse<List<ResultTable>> result = await command.Query(@"SELECT Name = 'Israel', Age=27;");
      string table = GridBuilder.HtmlTable(result.Model.ElementAt(0));
      Assert.True(result.Correct && !table.IsNotValid());
    }

    /// <summary>
    /// Proves that the HtmlTable method
    /// converts a Sheetdata object instance
    /// into an html table string 
    /// </summary>
    [Fact]
    public void SheetDataHtmlTable()
    {
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      SheetData sheet = document.GetSheetData(@"Report");
      string table = GridBuilder.HtmlTable(sheet);
      Assert.True(!table.IsNotValid());
    }
  }
}
