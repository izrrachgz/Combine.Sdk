using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Models
{
  /// <summary>
  /// Provides a data model representation for a
  /// result table object entity
  /// </summary>
  public class ResultTable
  {
    /// <summary>
    /// Index position
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Columns collection
    /// </summary>
    public List<string> Columns { get; set; }

    /// <summary>
    /// Rows collection
    /// </summary>
    public List<ResultRow> Rows { get; set; }

    /// <summary>
    /// Creates a new result table empty instance
    /// </summary>
    public ResultTable()
    {
      Index = 0;
      Columns = new List<string>();
      Rows = new List<ResultRow>();
    }

    /// <summary>
    /// Creates a new result table instance
    /// using the supplied property values
    /// </summary>
    /// <param name="index">Table Index</param>
    public ResultTable(int index)
    {
      Index = index;
      Columns = new List<string>();
      Rows = new List<ResultRow>();
    }

    /// <summary>
    /// Creates a new result table instance
    /// using the supplied property values
    /// </summary>
    /// <param name="index">Table index</param>
    /// <param name="columns">Columns collection</param>
    /// <param name="rows">Rows collection</param>
    public ResultTable(int index, List<string> columns, List<ResultRow> rows)
    {
      Index = index;
      Columns = columns;
      Rows = rows;
    }

    /// <summary>
    /// Creates a new result table instance
    /// using the supplied property values
    /// </summary>
    /// <param name="index">Table index</param>
    /// <param name="columns">Columns collection</param>
    /// <param name="rows">Rows collection</param>
    public ResultTable(int index, int[] columns, List<ResultRow> rows)
    {
      Index = index;
      Columns = columns
        .Select(e => e.ToString())
        .ToList();
      Rows = rows;
    }

    /// <summary>
    /// Gets the first result from the
    /// first row
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="column"></param>
    /// <returns>T object</returns>
    public T GetFirstResult<T>(string column) where T : struct
    => Rows.FirstOrDefault().GetCellValue<T>(column);

    /// <summary>
    /// Gets the first result from the
    /// specified row
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index">Row index position</param>
    /// <param name="column">Column name</param>
    /// <returns>T object</returns>
    public T GetFirstResult<T>(int index, string column) where T : struct
    => Rows.Count - 1 >= index ? Rows.ElementAt(index).GetCellValue<T>(column) : default(T);
  }
}
