using System;
using System.Linq;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Extensions
{
  /// <summary>
  /// Provides extension methods for ResultTable type objects
  /// </summary>
  public static class ResultTableExtensions
  {
    /// <summary>
    /// Converts the result table into
    /// a grid list
    /// </summary>
    /// <param name="table">Result Table reference</param>
    /// <returns>Grid list</returns>
    public static List<object[]> ToGrid(this ResultTable table)
    {
      if (table == null || table.Rows.IsNotValid())
        return new List<object[]>(0);
      List<object[]> grid = new List<object[]>(table.Rows.Count);
      grid.Add(table.Columns.ToArray());
      for (int r = 0; r < table.Rows.Count; r++)
      {
        List<object> row = new List<object>(table.Columns.Count);
        for (int c = 0; c < table.Columns.Count; c++)
          row.Add(table.Rows.ElementAt(r).Cells.ElementAt(c).Value);
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
    public static List<T> ToList<T>(this ResultTable table) where T : class, new()
    {
      if (table == null || table.Rows.IsNotValid())
        return new List<T>(0);
      T entity = new T();
      Type type = entity.GetType();
      string[] properties = entity.GetPropertyNames();
      string[] columns = table.Columns.ToArray();
      List<T> entities = new List<T>(table.Rows.Count);
      table.Rows.ForEach(r =>
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
    public static List<T> ToEntities<T>(this ResultTable table) where T : class, IEntity, new()
    => table.ToList<T>();
  }
}
