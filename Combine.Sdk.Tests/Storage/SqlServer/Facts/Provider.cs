using Xunit;
using System;
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
  /// Provides a mechanism to test all the methods
  /// in the SqlDataProvider instance available
  /// </summary>
  public class SqlDataProviderTests
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
    /// Creates a new sql data provider test instance
    /// </summary>
    public SqlDataProviderTests()
    {
      JsonLoader<Configuration> config = new JsonLoader<Configuration>($@"{AppDomain.CurrentDomain.BaseDirectory}Storage\SqlServer\DataProvider.json");
      config.Load()
        .Wait();
      ConnectionString = config.Instance.Values
        .GetFirst<string>(@"ConnectionString");
      DataProvider = new SqlServerDataProvider<TestingEntity>(ConnectionString);
    }

    /// <summary>
    /// Proves that the extension method CreateInsertStatement
    /// creates an sql insert statement correctly
    /// </summary>
    [Fact]
    public void CreateInsertStatement()
    {
      string sql = DataProvider.CreateInsertStatement(out object[] parameters);
      Assert.True(!sql.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method CreateUpdateStatement
    /// creates an sql update statement correctly
    /// </summary>
    [Fact]
    public void CreateUpdateStatement()
    {
      string sql = DataProvider.CreateUpdateStatement(out object[] parameters);
      Assert.True(!sql.IsNotValid());
    }

    /// <summary>
    /// Proves that the extension method GetFirst
    /// retrieves the first entity that belongs
    /// to a primary key
    /// </summary>
    [Fact]
    public async Task GetFirst()
    {
      ComplexResponse<TestingEntity> result = await DataProvider.GetFirst(11);
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the extension method GetFirst
    /// retrieves the first entity that belongs
    /// to a primary key including the value
    /// of the specified columns only
    /// </summary>
    [Fact]
    public async Task GetFirstWithColumns()
    {
      ComplexResponse<TestingEntity> result = await DataProvider.GetFirst(11, new string[1] { @"Id" });
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the extension method GetFirst
    /// retrieves the first entity that
    /// compels to the supplied conditions and
    /// includes the specified columns during
    /// selection
    /// </summary>
    [Fact]
    public async Task GetFirstByConditions()
    {
      List<QueryCondition<TestingEntity>> conditions = new List<QueryCondition<TestingEntity>>(1)
      {
        new QueryCondition<TestingEntity>(e => e.Id, QueryOperator.Equal, 1052)
      };
      ComplexResponse<TestingEntity> result = await DataProvider.GetFirst(conditions);
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the extension method Delete
    /// marks as deleted an entity
    /// correctly
    /// </summary>
    [Fact]
    public async Task Delete()
    {
      BasicResponse result = await DataProvider.Delete(10);
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the extension method Delete
    /// marks as deleted a set of entities 
    /// correctly
    /// </summary>
    [Fact]
    public async Task DeleteList()
    {
      BasicResponse result = await DataProvider.Delete(new List<long>(1) { 10 });
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the extension method Save
    /// saves all the changes made to an entity
    /// correclty
    /// </summary>
    [Fact]
    public async Task Save()
    {
      TestingEntity entity = new TestingEntity()
      {
        Name = @"Testing"
      };
      BasicResponse result = await DataProvider.Save(entity);
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the extension method Save
    /// saves all the changes made to a set
    /// of entities correctly
    /// </summary>
    [Fact]
    public async Task SaveList()
    {
      List<TestingEntity> entities = new List<TestingEntity>(2)
      {
        new TestingEntity(){Id = 12, Name = @"Perpetual Testing Edited"},
        new TestingEntity(){Name = @"Perpetual Testing New"},
      };
      BasicResponse result = await DataProvider.Save(entities);
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the extension method Save
    /// saves all the changes made to an entity
    /// correctly
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SaveByStandAlone()
    {
      TestingEntity entity = new TestingEntity() { Name = @"New Saved by standalone method" };
      BasicResponse result = await entity.Save();
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the extension method Save
    /// saves all the changes made to an entity
    /// list correctly
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SaveListByStandAlone()
    {
      List<TestingEntity> entities = new List<TestingEntity>(2)
      {
        new TestingEntity(){Name = @"New list Saved by standalone method 1"},
        new TestingEntity(){Name = @"New list Saved by standalone method 2"},
      };
      BasicResponse result = await entities.Save();
      Assert.True(result.Correct);
    }

    /// <summary>
    /// Proves that the extension method GetRecords
    /// retrieves all the entities that match
    /// a pagination request instance
    /// </summary>
    [Fact]
    public async Task GetRecords()
    {
      Pagination page = new Pagination();
      ComplexResponse<PaginatedCollection<TestingEntity>> result = await DataProvider.GetRecords(page);
      Assert.True(result.Correct);
    }
  }
}
