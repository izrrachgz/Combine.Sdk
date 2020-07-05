using Xunit;
using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Json;
using Combine.Sdk.Data.Definitions.Paged;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Definitions.Response;
using Combine.Sdk.Data.Definitions.Collection;
using Combine.Sdk.Storage.SqlServer;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;
using Combine.Sdk.Storage.SqlServer.Extensions;
using Combine.Sdk.Tests.Storage.DataProvider.SqlServer.Models;

namespace Combine.Sdk.Tests.Storage.SqlServer.Theories
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the SqlProvider instance available
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
    /// Proves that the Save method
    /// is capable of saving one thousand
    /// entities in less than a second
    /// </summary>
    /// <param name="seconds">Maximum expected time in seconds to perform the task</param>
    /// <param name="records">Total records to save</param>
    /// <returns></returns>
    [Theory, InlineData(1, 1000)]
    public async Task Save(short seconds = 1, int records = 1000)
    {
      List<TestingEntity> entities = new List<TestingEntity>(records);
      for (int x = 0; x < records; x++)
      {
        entities.Add(new TestingEntity()
        {
          Name = $@"{x} Perpetual Testing New"
        });
      }
      Stopwatch sw = new Stopwatch();
      sw.Start();
      BasicResponse result = await DataProvider.Save(entities);
      sw.Stop();
      Assert.True(result.Correct && sw.Elapsed.TotalSeconds <= seconds);
    }
  }
}
