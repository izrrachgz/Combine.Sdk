using System;
using System.Linq;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Extensions
{
  /// <summary>
  /// Provides extension methods for ResultRow type objects
  /// </summary>
  public static class ResultRowExtensions
  {
    /// <summary>
    /// Converts a result row into a entity
    /// class instance
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <param name="row">Row reference</param>
    /// <returns>Entity</returns>
    public static T ToEntity<T>(this ResultRow row) where T : class, IEntity, new()
    {
      if (row == null || row.Cells.IsNotValid())
        return default(T);
      T entity = new T();
      Type type = entity.GetType();
      string[] properties = entity.GetPropertyNames();
      string[] columns = row.Cells
        .Select(c => c.ColumnName)
        .ToArray();
      foreach (string column in properties)
      {
        if (!columns.Contains(column))
          continue;
        object value = row.Cells
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
