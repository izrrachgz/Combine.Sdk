using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Extensions
{
  /// <summary>
  /// Provides extension methods for IEntity type objects
  /// </summary>
  public static class EntityExtensions
  {
    /// <summary>
    /// Gets the operation table columns from the
    /// entity, excluding the reference class types
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <param name="entity">Entity reference</param>
    /// <returns>String array</returns>
    internal static string[] OperationColumns<T>(this T entity) where T : class, IEntity, new()
    {
      if (entity == null)
        return null;
      Type type = entity.GetType();
      PropertyInfo[] info = type.GetProperties();
      return info
        .Where(p => p.PropertyType.Namespace.StartsWith(@"System") || p.PropertyType.IsEnum)
        .Select(p => p.Name)
        .ToArray();
    }

    /// <summary>
    /// Gets the operation string type table columnas
    /// from the entity, excluding the reference class
    /// types
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <param name="entity">Entity reference</param>
    /// <returns>String array</returns>
    internal static string[] SearchableColumns<T>(this T entity) where T : class, IEntity, new()
    {
      if (entity == null)
        return null;
      Type type = entity.GetType();
      PropertyInfo[] info = type.GetProperties();
      return info
        .Where(p => p.PropertyType.Name.Equals(@"String"))
        .Select(p => p.Name)
        .ToArray();
    }

    /// <summary>
    /// Converts the entity into an object array
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <param name="entity">Entity reference</param>
    /// <returns>Object array</returns>
    public static object[] AsArray<T>(this T entity) where T : class, IEntity, new()
    {
      if (entity == null)
        return null;
      Type type = entity.GetType();
      string[] columns = entity.OperationColumns();
      List<object> values = new List<object>(columns.Count());
      foreach (string column in columns)
        values.Add(type.GetProperty(column).GetValue(entity));
      return values.ToArray();
    }

    /// <summary>
    /// Converts the entities into a list of
    /// object array items
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <param name="entities">Entity list</param>
    /// <returns>Object array list</returns>
    public static List<object[]> AsArray<T>(this List<T> entities) where T : class, IEntity, new()
    {
      if (entities.IsNotValid())
        return null;
      Type type = entities.ElementAt(0).GetType();
      string[] columns = entities.ElementAt(0).OperationColumns();
      List<object[]> list = new List<object[]>(entities.Count());
      foreach (T entity in entities)
      {
        List<object> values = new List<object>(columns.Count());
        foreach (string column in columns)
          values.Add(type.GetProperty(column).GetValue(entity));
        list.Add(values.ToArray());
      }
      return list;
    }
  }
}
