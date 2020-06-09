using Xunit;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Json;
using Combine.Sdk.Data.Definitions.Paged;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Definitions.Response;
using Combine.Sdk.Data.Definitions.Collection;
using Combine.Sdk.Storage.DataProvider.SqlServer;
using Combine.Sdk.Storage.Definitions.DataProvider.Enums;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;
using Combine.Sdk.Storage.DataProvider.SqlServer.Extensions;
using Combine.Sdk.Tests.Storage.DataProvider.SqlServer.Models;

namespace Combine.Sdk.Tests.Storage.DataProvider.SqlServer.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the query condition
  /// extension methods availables
  /// </summary>
  public class QueryConditionTests
  {
    /// <summary>
    /// Sql connection string
    /// </summary>
    private string ConnectionString { get; }

    /// <summary>
    /// Sql Data Provider
    /// </summary>
    private SqlServerDataProvider<TestingEntity> DataProvider { get; }

    /// <summary>
    /// Creates a new query condition tests instance
    /// </summary>
    public QueryConditionTests()
    {
      JsonLoader<Configuration> config = new JsonLoader<Configuration>($@"{AppDomain.CurrentDomain.BaseDirectory}Storage\SqlServer\DataProvider.json");
      config.Load()
        .Wait();
      ConnectionString = config.Instance.Values
        .GetFirst<string>(@"ConnectionString");
      DataProvider = new SqlServerDataProvider<TestingEntity>(ConnectionString);
    }

    /// <summary>
    /// Proves that the IsNotValid method
    /// evaluates a non valid to work with
    /// condition collection object correctly
    /// </summary>
    [Fact]
    public void IsNotValid()
    {
      List<QueryCondition> conditions = new List<QueryCondition>();
      Assert.True(!conditions.ToSqlQuery(out SqlParameter[] @params).IsNotValid());
    }

    /// <summary>
    /// Proves that the ToSqlQuery method
    /// creates a standard sql server query
    /// using the supplied condition collection
    /// </summary>
    [Fact]
    public void ToSqlQuery()
    {
      List<QueryCondition> conditions = new List<QueryCondition>(1)
      {
        new QueryCondition(@"Id", QueryOperator.Equal, 1052)
      };
      Assert.True(!conditions.ToSqlQuery(out SqlParameter[] @params).IsNotValid());
    }

    /// <summary>
    /// Proves that the QueryCondition object
    /// can be used in the data provider
    /// instance to get records by the specified
    /// conditions
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetRecordsByConditions()
    {
      List<QueryCondition> conditions = new List<QueryCondition>()
      {
        //new QueryCondition(@"Id", QueryOperator.In, new long[2]{ 1052, 1053 }),
        new QueryCondition(e => e.Id, QueryOperator.In, new long[2]{ 1052, 1053 }),
      };
      Pagination page = new Pagination();
      ComplexResponse<PaginatedCollection<TestingEntity>> result = await DataProvider.GetRecords(page, conditions: conditions);
      Assert.True(result.Correct);
    }
  }
}
