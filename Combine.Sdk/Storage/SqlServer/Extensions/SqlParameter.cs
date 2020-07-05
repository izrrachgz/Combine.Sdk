using System;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;
using Combine.Sdk.Storage.Definitions.DataProvider.Extensions;

namespace Combine.Sdk.Storage.SqlServer.Extensions
{
  /// <summary>
  /// Provides extension methods for SqlParameter type objects
  /// </summary>
  internal static class SqlParameterExtensions
  {
    /// <summary>
    /// Checks if the given SqlParameter value is null or is equal to the min value
    /// </summary>
    /// <param name="parameter">SqlParameter to evaluate</param>
    /// <returns>True or False</returns>
    internal static bool IsNotValid(this SqlParameter parameter)
    {
      return parameter == null || parameter.ParameterName.IsNotValid();
    }

    /// <summary>
    /// Checks if the given SqlParameter array is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="array">Array of SqlParameter to evaluate</param>
    /// <returns>True or False</returns>
    internal static bool IsNotValid(this SqlParameter[] array)
    {
      return array == null || !array.Any() || array.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of SqlParameter to evaluate</param>
    /// <returns>True or False</returns>
    internal static bool IsNotValid(this List<SqlParameter> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Checks if the given list is not valid or contains any non-valid value inside
    /// </summary>
    /// <param name="list">Collection of SqlParameter to evaluate</param>
    /// <returns>True or False</returns>
    internal static bool IsNotValid(this List<SqlParameter[]> list)
    {
      return list == null || !list.Any() || list.Any(e => e.IsNotValid());
    }

    /// <summary>
    /// Sets the parameters values using the specified
    /// entity data model
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="parameters">Asociated parameters</param>
    /// <param name="entity">Entity reference</param>
    internal static void SetValues<T>(this SqlParameter[] parameters, T entity) where T : class, IEntity, new()
    {
      if (parameters.IsNotValid() || entity == null)
        return;
      Type type = entity.GetType();
      string[] columns = entity.OperationColumns();
      foreach (SqlParameter parameter in parameters)
      {
        string property = parameter.ParameterName.Replace(@"@", @"");
        if (columns.Contains(property))
        {
          object value = type.GetProperty(property).GetValue(entity);
          value = value ?? DBNull.Value;
          parameter.Value = value;
        }
      }
    }
  }
}
