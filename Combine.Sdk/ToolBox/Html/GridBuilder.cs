using System.Linq;
using System.Text;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;

namespace Combine.Sdk.ToolBox.Html
{
  /// <summary>
  /// Provides a mechanism to build html grid tables
  /// </summary>
  public class GridBuilder
  {
    /// <summary>
    /// Creates a new GridBuilder instace
    /// </summary>
    public GridBuilder()
    {
    }

    /// <summary>
    /// Creates the header table element
    /// </summary>
    /// <param name="columns">Columns reference</param>
    /// <returns>Thead</returns>
    private string Headers(IEnumerable<string> columns)
    {
      StringBuilder sb = new StringBuilder(@"<thead class='table-header'>");
      sb.Append(@"<tr class='table-header-row'>");
      int index = 0;
      foreach (string column in columns)
      {
        sb.Append($@"<th data-key='{index}' class='table-header-cell'>")
          .Append(column)
          .Append(@"</th>");
        index++;
      }
      sb.Append(@"</tr>")
        .Append(@"</thead>");
      return sb.ToString();
    }

    /// <summary>
    /// Creates the body table element
    /// </summary>
    /// <param name="rows">Rows reference</param>
    /// <returns>Tbody</returns>
    private string Body(List<object[]> rows)
    {
      StringBuilder sb = new StringBuilder(@"<tbody class='table-body'>");
      int index = 0;
      foreach (object[] row in rows)
      {
        sb.Append($@"<tr class='table-body-row' data-key='{index}'>");
        foreach (object value in row)
        {
          sb.Append(@"<td class='table-body-cell'>")
            .Append(value)
            .Append(@"</td>");
        }
        sb.Append(@"</tr>");
        index++;
      }
      sb.Append(@"</tbody>");
      return sb.ToString();
    }

    /// <summary>
    /// Converts a ResultTable reference into
    /// an Html Table
    /// </summary>
    /// <param name="table">ResultTable reference</param>
    /// <returns>Html Table</returns>
    public string HtmlTable(ResultTable table, bool includeHeaders = true)
    {
      StringBuilder grid = new StringBuilder(@"<table class='grid-table'>");
      List<object[]> rows = table.ToGrid();
      if (includeHeaders)
      {
        rows.RemoveAt(0);
        grid.Append(Headers(table.Columns));
      }
      grid.Append(Body(rows))
      .Append(@"</table>");
      return grid.ToString();
    }

    /// <summary>
    /// Converts a SheetData reference into
    /// an Html Table
    /// </summary>
    /// <param name="sheet">SheetData reference</param>
    /// <param name="firstRowAsHeaders">Specify that the first row are the headers</param>
    /// <returns>Html Table</returns>
    public string HtmlTable(SheetData sheet, bool firstRowAsHeaders = true)
    {
      StringBuilder grid = new StringBuilder(@"<table class='grid-table'>");
      List<object[]> rows = sheet.GetData();
      if (firstRowAsHeaders)
      {
        string[] columns = rows.ElementAt(0)
             .Select(c => c.ToString())
             .ToArray();
        rows.RemoveAt(0);
        grid.Append(Headers(columns));
      }
      grid.Append(Body(rows))
      .Append(@"</table>");
      return grid.ToString();
    }
  }
}
