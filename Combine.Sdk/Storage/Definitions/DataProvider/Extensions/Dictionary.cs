using System;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Extensions
{
  /// <summary>
  /// Provides extension methods for Dictionary type objects
  /// </summary>
  public static class DictionaryExtensions
  {
    /// <summary>
    /// Converts the given dictionary into an entity
    /// class instance
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <param name="dictionary">Dictionary reference</param>
    /// <returns>T Entity</returns>
    public static T ToEntity<T>(this Dictionary<string, dynamic> dictionary) where T : class, IEntity, new()
    {
      if (dictionary == null || dictionary.Count.Equals(0))
        return default(T);
      T entity = new T();
      Type type = entity.GetType();
      string[] properties = entity.GetPropertyNames();
      foreach (string column in properties)
      {
        if (!dictionary.ContainsKey(column))
          continue;
        object value = dictionary[column];
        value = value == DBNull.Value ? null : value;
        type.GetProperty(column).SetValue(entity, value);
      }
      return entity;
    }

    /// <summary>
    /// Converts the given dictionary list into an entity
    /// class instance list
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <param name="list">List reference</param>
    /// <returns>T Entity list</returns>
    public static List<T> ToEntities<T>(this List<Dictionary<string, dynamic>> list) where T : class, IEntity, new()
    {
      if (list.IsNotValid())
        return new List<T>(0);
      T entity = new T();
      Type type = entity.GetType();
      string[] properties = entity.GetPropertyNames();
      List<T> entities = new List<T>(list.Count);
      list.ForEach(d =>
      {
        T model = new T();
        foreach (string column in properties)
        {
          if (!d.ContainsKey(column))
            continue;
          object value = d[column];
          value = value == DBNull.Value ? null : value;
          type.GetProperty(column).SetValue(model, value);
        }
        entities.Add(model);
      });
      return entities;
    }
  }
}
