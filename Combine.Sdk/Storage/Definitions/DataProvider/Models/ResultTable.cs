using System;
using System.Linq;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

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

    /// <summary>
    /// Converts the result table into
    /// a grid list
    /// </summary>
    /// <param name="table">Result Table reference</param>
    /// <returns>Grid list</returns>
    public List<object[]> ToGrid()
    {
      if (Rows.IsNotValid())
        return new List<object[]>(0);
      List<object[]> grid = new List<object[]>(Rows.Count);
      grid.Add(Columns.ToArray());
      for (int r = 0; r < Rows.Count; r++)
      {
        List<object> row = new List<object>(Columns.Count);
        for (int c = 0; c < Columns.Count; c++)
          row.Add(Rows.ElementAt(r).Cells.ElementAt(c).Value);
        grid.Add(row.ToArray());
      }
      return grid;
    }

    /// <summary>
    /// Converts a result table into an
    /// collection class instance
    /// </summary>
    /// <typeparam name="T">Class Type</typeparam>
    /// <param name="table">Table reference</param>
    /// <returns>Class list</returns>
    public List<T> ToList<T>() where T : class, new()
    {
      if (Rows.IsNotValid())
        return new List<T>(0);
      T entity = new T();
      Type type = entity.GetType();
      string[] properties = entity.GetPropertyNames();
      string[] columns = Columns.ToArray();
      List<T> entities = new List<T>(Rows.Count);
      Rows.ForEach(r =>
      {
        T model = new T();
        foreach (string column in properties)
        {
          if (!columns.Contains(column))
            continue;
          object value = r.Cells
            .Where(c => c.ColumnName.Equals(column))
            .Select(c => c.Value)
            .FirstOrDefault();
          value = value == DBNull.Value ? null : value;
          type.GetProperty(column).SetValue(model, value);
        }
        entities.Add(model);
      });
      return entities;
    }

    /// <summary>
    /// Converts a result table into an
    /// entity collection class instance
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <param name="table">Table reference</param>
    /// <returns>Entity list</returns>
    public List<T> ToEntities<T>() where T : class, IEntity, new()
    => ToList<T>();
  }
}
