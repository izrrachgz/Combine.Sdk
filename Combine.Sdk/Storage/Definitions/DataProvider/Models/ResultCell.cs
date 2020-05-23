using System;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Models
{
  /// <summary>
  /// Provides a data model representation for a 
  /// result cell object entity
  /// </summary>
  public class ResultCell
  {
    /// <summary>
    /// Row Index position
    /// </summary>
    public int RowIndex { get; set; }

    /// <summary>
    /// Column Index position
    /// </summary>
    public int ColumnIndex { get; set; }

    /// <summary>
    /// Column Name
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// Cell value
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Creates a new result cell empty instance
    /// </summary>
    public ResultCell() { }

    /// <summary>
    /// Creates a new result cell instance
    /// using the supplied property values
    /// </summary>
    /// <param name="rowIndex">Row Index</param>
    /// <param name="columnIndex">Column Index</param>
    /// <param name="value">Cell value</param>
    public ResultCell(int rowIndex, int columnIndex, object value)
    {
      RowIndex = rowIndex;
      ColumnIndex = columnIndex;
      ColumnName = columnIndex.ToString();
      Value = value == DBNull.Value ? null : value;
    }

    /// <summary>
    /// Creates a new result cell instance
    /// using the supplied property values
    /// </summary>
    /// <param name="rowIndex">Row Index</param>
    /// <param name="columnIndex">Column Index</param>
    /// <param name="columnName">Column Name</param>
    /// <param name="value">Cell value</param>
    public ResultCell(int rowIndex, int columnIndex, string columnName, object value)
    {
      RowIndex = rowIndex;
      ColumnIndex = columnIndex;
      ColumnName = columnName;
      Value = value == DBNull.Value ? null : value;
    }
  }
}
