using System.Data;
using System.Linq;
using DocumentFormat.OpenXml;
using System.Threading.Tasks;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Combine.Sdk.Extensions.Excel
{
  /// <summary>
  /// Provides extension methods for SheetData type objects
  /// </summary>
  public static class SheetDataExtensions
  {
    /// <summary>
    /// Checks if the given SheetData value is null
    /// </summary>
    /// <param name="sheet">SheetData to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this SheetData sheet)
    {
      return sheet == null;
    }

    /// <summary>
    /// Gets the row at the given index from the
    /// sheet data
    /// </summary>
    /// <param name="sheet">SheetData reference</param>
    /// <param name="index">Index position</param>
    /// <returns>Row</returns>
    public static Row GetRow(this SheetData sheet, uint index = 0)
    {
      if (sheet.IsNotValid() || sheet.Descendants<Row>().Count() - 1 < index) return null;
      return sheet.Descendants<Row>().ElementAt((int)index);
    }

    /// <summary>
    /// Changes the two specified columns in the
    /// sheet data
    /// </summary>
    /// <param name="sheet">SheetData reference</param>
    /// <param name="first">Index position column to swap</param>
    /// <param name="swap">Index position column to swap for</param>
    public static void SwapColumns(this SheetData sheet, uint first, uint swap)
    {
      if (sheet.IsNotValid() || first.Equals(swap)) return;
      sheet.Descendants<Row>()
        .Select(r => r as Row)
        .ToList()
        .ForEach(r => r.SwapColumns(first, swap));
    }

    /// <summary>
    /// Changes the two specified columns in the
    /// sheet data
    /// </summary>
    /// <param name="sheet">SheetData reference</param>
    /// <param name="first">Reference position column to swap</param>
    /// <param name="swap">Reference position column to swap for</param>
    public static void SwapColumns(this SheetData sheet, string first, string swap)
    {
      if (sheet.IsNotValid() || first.Equals(swap)) return;
      sheet.Descendants<Row>()
        .Select(r => r as Row)
        .ToList()
        .ForEach(r => r.SwapColumns(first, swap));
    }

    /// <summary>
    /// Adds all the specified values as new rows
    /// </summary>
    /// <param name="sheet">SheetData reference</param>
    /// <param name="data">Object values</param>
    /// <returns>Task State</returns>
    public static async Task AddRows(this SheetData sheet, IEnumerable<IEnumerable<object>> data)
    {
      //Check whether the sheet reference or the sheet data is a non-valid to work with object value
      if (sheet.IsNotValid() || data == null || !data.Any())
        return;
      //stablish wich is going to be the start index for data to set
      uint startIndex = sheet
        .Descendants<Row>()
        .LastOrDefault()?.RowIndex.Value ?? 0;
      startIndex++;
      //Add all the entries
      for (uint r = 0; r < data.Count(); r++)
      {
        //Create a new row
        Row row = new Row() { RowIndex = startIndex + r };
        IEnumerable<object> d = data.ElementAt((int)r);
        //Add all the entities as a row
        for (uint c = 0; c < d.Count(); c++)
        {
          //Get the entity in turn
          object e = d.ElementAt((int)c);
          Cell cell = new Cell();
          await cell.SetValue(e);
          row.AppendChild(cell);
        }
        //Add the row to the data sheet
        sheet.Append(row);
      }
    }

    /// <summary>
    /// Adds all the specified data table values as new rows
    /// </summary>
    /// <param name="sheet">SheetData reference</param>
    /// <param name="table">DataTable reference</param>
    /// <param name="includeColumns">Include the table columnas as first row</param>
    /// <returns>Task State</returns>
    public static async Task AddRows(this SheetData sheet, DataTable table, bool includeColumns = true)
    {
      //Check whether the sheet reference or the sheet data is a non-valid to work with object value
      if (sheet.IsNotValid() || table == null || !table.Rows.Count.Equals(0) || table.HasErrors)
        return;
      //Include the columns name as first row
      if (includeColumns)
      {
        string[] columns = new string[table.Columns.Count];
        for (int x = 0; x < table.Columns.Count; x++)
          columns[x] = table.Columns[x].ColumnName;
        await sheet.AddRows(new List<string[]>(1) { columns });
      }
      //Include all the rows inside the table
      List<object[]> rows = new List<object[]>(table.Rows.Count);
      for (int r = 0; r < table.Rows.Count; r++)
        rows.Add(table.Rows[r].ItemArray);
      await sheet.AddRows(rows);
    }
  }
}