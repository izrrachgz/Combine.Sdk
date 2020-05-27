using Xunit;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Json;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Definitions.Response;
using Combine.Sdk.Data.Definitions.Collection;
using Combine.Sdk.Storage.DataProvider.SqlServer.Commands;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;

namespace Combine.Sdk.Tests.Storage.DataProvider.SqlServer.Facts.Commands
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the SqlServerCommand instance available
  /// </summary>
  public class FactSqlServerCommandTests
  {
    /// <summary>
    /// Sql connection string
    /// </summary>
    private string ConnectionString { get; }

    /// <summary>
    /// Creates a new Sql Server Command tests instance
    /// </summary>
    public FactSqlServerCommandTests()
    {
      JsonLoader<Configuration> config = new JsonLoader<Configuration>($@"{AppDomain.CurrentDomain.BaseDirectory}Storage\SqlServer\DataProvider.json");
      config.Load()
        .Wait();
      ConnectionString = config.Instance.Values
        .GetFirst<string>(@"ConnectionString");
    }

    /// <summary>
    /// Proves that the Query method returns
    /// a collection of result table objects
    /// containing the results from sql server
    /// correctly
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task Query()
    {
      SqlServerCommand command = new SqlServerCommand(ConnectionString);
      ComplexResponse<List<ResultTable>> result = await command.Query(@"SELECT 1, 2, 3;");
      Assert.True(result.Correct);
    }
  }
}
