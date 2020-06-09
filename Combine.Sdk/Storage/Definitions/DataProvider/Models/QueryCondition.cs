using System;
using System.Linq.Expressions;
using Combine.Sdk.Storage.Definitions.DataProvider.Enums;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Models
{
  /// <summary>
  /// Provides a query condition data model representation
  /// </summary>
  public class QueryCondition<T> where T : class, IEntity, new()
  {
    /// <summary>
    /// Asociated entity property to evaluate
    /// </summary>
    public string Property { get; }

    /// <summary>
    /// Operator to compare with
    /// </summary>
    public QueryOperator Comparer { get; }

    /// <summary>
    /// Value to compare with
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Creates a new query condition instance
    /// using the specified value
    /// </summary>
    /// <param name="property">Entity's property expression</param>
    /// <param name="comparer">Operator to evaluate with</param>
    /// <param name="value">Value to compare with</param>
    public QueryCondition(Expression<Func<T, object>> expresion, QueryOperator comparer, object value)
    {
      MemberExpression operand = expresion.Body as MemberExpression;
      Property = operand.Member.Name;
      Comparer = comparer;
      Value = value;
    }
  }
}
