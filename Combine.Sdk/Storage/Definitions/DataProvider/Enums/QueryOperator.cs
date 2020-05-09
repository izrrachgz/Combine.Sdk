namespace Combine.Sdk.Storage.Definitions.DataProvider.Enums
{
  /// <summary>
  /// Enumeration of query operators for queries
  /// </summary>
  public enum QueryOperator
  {
    /// <summary>    
    /// a = b
    /// </summary>
    Equal = 0,

    /// <summary>    
    /// a < b
    /// </summary>
    LessThan = 1,

    /// <summary>    
    /// a <= b
    /// </summary>
    LessOrEqual = 2,

    /// <summary>    
    /// a <> b
    /// </summary>
    NotEqual = 3,

    /// <summary>
    /// a > b
    /// </summary>
    GreaterThan = 4,

    /// <summary>
    /// a >= b
    /// </summary>
    GreaterOrEqual = 5,

    /// <summary>
    /// a != b
    /// </summary>
    Diferent = 6,

    /// <summary>
    /// a is b
    /// </summary>
    Is = 7,

    /// <summary>
    /// a is not b
    /// </summary>
    IsNot = 8,

    /// <summary>
    /// a like b
    /// </summary>
    Like = 9,

    /// <summary>
    /// a in b
    /// </summary>
    In = 10,

    /// <summary>
    /// a not in b
    /// </summary>
    NotIn = 11,
  }
}
