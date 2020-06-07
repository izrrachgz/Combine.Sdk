using System;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Storage.Definitions.DataProvider.Enums;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;

namespace Combine.Sdk.Storage.DataProvider.SqlServer.Extensions
{
  /// <summary>
  /// Provides extension methods for QueryCondition type objects
  /// </summary>
  public static class QueryConditionExtensions
  {
    /// <summary>
    /// These properties are going to be ignored during Sql query transformation
    /// </summary>
    private static string[] IgnoredProperties = new string[2] { @"Created", @"Deleted" };

    /// <summary>
    /// Creates the sql list collection comparer part
    /// </summary>
    /// <param name="sb">String builder reference</param>
    /// <param name="value">String collection</param>
    private static void AddStringCollectionAsCondition(StringBuilder sb, object value)
    {
      if (value is IEnumerable<char>)
        sb.Append($@"(N'{string.Join(@"',N'", value as IEnumerable<char>)}')");
      if (value is IEnumerable<string>)
        sb.Append($@"(N'{string.Join(@"',N'", value as IEnumerable<string>)}')");
    }

    /// <summary>
    /// Creates the sql list collection comparer part
    /// </summary>
    /// <param name="sb">String builder reference</param>
    /// <param name="value">Numeric collection</param>
    private static void AddNumericCollectionAsCondition(StringBuilder sb, object value)
    {
      if (value is IEnumerable<byte>)
        sb.Append($@"({string.Join(@", ", value as IEnumerable<byte>)})");
      if (value is IEnumerable<short>)
        sb.Append($@"({string.Join(@", ", value as IEnumerable<short>)})");
      if (value is IEnumerable<int>)
        sb.Append($@"({string.Join(@", ", value as IEnumerable<int>)})");
      if (value is IEnumerable<long>)
        sb.Append($@"({string.Join(@", ", value as IEnumerable<long>)})");
      if (value is IEnumerable<float>)
        sb.Append($@"({string.Join(@", ", value as IEnumerable<float>)})");
      if (value is IEnumerable<decimal>)
        sb.Append($@"({string.Join(@", ", value as IEnumerable<decimal>)})");
      if (value is IEnumerable<double>)
        sb.Append($@"({string.Join(@", ", value as IEnumerable<double>)})");
    }

    /// <summary>
    /// Checks if the given collection is null or its length is zero
    /// or greater than 2100 elements
    /// </summary>
    /// <param name="conditions">Conditions to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this List<QueryCondition> conditions)
    {
      return conditions == null || !conditions.Any() || conditions.Count > 2100;
    }

    /// <summary>
    /// Converts the query condition collection into a plain
    /// text sql statement representation
    /// </summary>
    /// <param name="conditions">Query condition collection reference</param>
    /// <param name="parameters">Sql paramters</param>
    /// <returns>String</returns>
    public static string ToSqlQuery(this List<QueryCondition> conditions, out SqlParameter[] parameters)
    {
      if (conditions.IsNotValid())
      {
        parameters = new SqlParameter[0];
        return @"";
      }
      conditions = conditions
        .Where(c => !IgnoredProperties.Equals(c.Property))
        .ToList();
      parameters = new SqlParameter[conditions.Count];
      StringBuilder sqlCondition = new StringBuilder();
      for (int x = 0; x < conditions.Count; x++)
      {
        QueryCondition condition = conditions.ElementAt(x);
        string paramName = $@"@P{x}";
        object value = condition.Value ?? DBNull.Value;        
        sqlCondition.Append($@"({condition.Property} ");
        switch (condition.Comparer)
        {
          case QueryOperator.Equal:
            sqlCondition.Append($@"= {paramName}");
            break;
          case QueryOperator.LessThan:
            sqlCondition.Append($@"< {paramName}");
            break;
          case QueryOperator.LessOrEqual:
            sqlCondition.Append($@"<= {paramName}");
            break;
          case QueryOperator.NotEqual:
            sqlCondition.Append($@"!= {paramName}");
            break;
          case QueryOperator.GreaterThan:
            sqlCondition.Append($@"> {paramName}");
            break;
          case QueryOperator.GreaterOrEqual:
            sqlCondition.Append($@">= {paramName}");
            break;
          case QueryOperator.Diferent:
            sqlCondition.Append($@"<> {paramName}");
            break;
          case QueryOperator.Is:
            sqlCondition.Append($@"Is {paramName}");
            break;
          case QueryOperator.IsNot:
            sqlCondition.Append($@"Is Not {paramName}");
            break;
          case QueryOperator.Like:
            sqlCondition.Append($@"Like '%' + {paramName} + '%'");
            break;
          case QueryOperator.In:
            if (value.IsStringCollection())
            {              
              sqlCondition.Append(@"In ");
              AddStringCollectionAsCondition(sqlCondition, value);
              value = DBNull.Value;
            }
            if (value.IsNumericCollection())
            {
              sqlCondition.Append(@"In ");
              AddNumericCollectionAsCondition(sqlCondition, value);
              value = DBNull.Value;
            }
            break;
          case QueryOperator.NotIn:
            if (value.IsStringCollection())
            {
              sqlCondition.Append(@"Not In ");
              AddStringCollectionAsCondition(sqlCondition, value);
              value = DBNull.Value;
            }
            if (value.IsNumericCollection())
            {
              sqlCondition.Append(@"Not In ");
              AddNumericCollectionAsCondition(sqlCondition, value);
              value = DBNull.Value;
            }
            break;
          default:
            sqlCondition.Append($@"= {paramName}");
            break;
        };
        parameters[x] = new SqlParameter(paramName, value);
        sqlCondition.Append((x + 1).Equals(conditions.Count) ? @")" : @") And ");
      }
      return sqlCondition.ToString();
    }
  }
}
