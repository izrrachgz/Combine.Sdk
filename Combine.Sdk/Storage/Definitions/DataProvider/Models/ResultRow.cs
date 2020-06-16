using System;
using System.Linq;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Models
{
  /// <summary>
  /// Provides a data model representation for a
  /// result row object entity
  /// </summary>
  public class ResultRow
  {
    /// <summary>
    /// Row Index position
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Cell collection
    /// </summary>
    public List<ResultCell> Cells { get; set; }

    /// <summary>
    /// Creates a new result row empty instance
    /// </summary>
    public ResultRow()
    {
      Index = 0;
      Cells = new List<ResultCell>();
    }

    /// <summary>
    /// Creates a new result row instance
    /// using the supplied property values
    /// </summary>
    /// <param name="index">Row Index</param>
    public ResultRow(int index)
    {
      Index = index;
      Cells = new List<ResultCell>();
    }

    /// <summary>
    /// Creates a new result row instance
    /// using the supplied property values
    /// </summary>
    /// <param name="index">Row Index</param>
    /// <param name="cells">Cell collection</param>
    public ResultRow(int index, List<ResultCell> cells)
    {
      Index = index;
      Cells = cells;
    }

    /// <summary>
    /// Gets the first cell value in the
    /// specified column
    /// </summary>
    /// <typeparam name="T">Object Type</typeparam>
    /// <param name="column">Column Name</param>
    /// <returns>Casted cell value</returns>
    public T GetCellValue<T>(string column)
    => (T)Cells.FirstOrDefault(c => c.ColumnName.Equals(column))?.Value;

    /// <summary>
    /// Gets the first cell value in the
    /// specified column
    /// </summary>
    /// <typeparam name="T">Object Type</typeparam>
    /// <param name="index">Index cell position</param>
    /// <returns>Casted cell value</returns>
    public T GetCellValue<T>(int index)
    => Cells.Count - 1 >= index ? (T)Cells.ElementAt(index).Value : default(T);

    /// <summary>
    /// Converts a result row into a entity
    /// class instance
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <param name="row">Row reference</param>
    /// <returns>Entity</returns>
    public T ToEntity<T>() where T : class, IEntity, new()
    {
      if (Cells.IsNotValid())
        return default(T);
      T entity = new T();
      Type type = entity.GetType();
      string[] properties = entity.GetPropertyNames();
      string[] columns = Cells
        .Select(c => c.ColumnName)
        .ToArray();
      foreach (string column in properties)
      {
        if (!columns.Contains(column))
          continue;
        object value = Cells
          .Where(c => c.ColumnName.Equals(column))
          .Select(c => c.Value)
          .FirstOrDefault();
        value = value == DBNull.Value ? null : value;
        type.GetProperty(column).SetValue(entity, value);
      }
      return entity;
    }
  }
}
