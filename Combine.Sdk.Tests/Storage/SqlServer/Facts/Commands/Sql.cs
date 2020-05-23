using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Data.Representation.Response;
using Combine.Sdk.Storage.DataProvider.SqlServer.Commands;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;

namespace Combine.Sdk.Tests.Storage.DataProvider.SqlServer.Facts.Commands
{
  public class FactSqlTests
  {
    private string ConnectionString { get; }

    public FactSqlTests()
    {
      ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = Utilidades.Contexto;Integrated Security = true;";
    }

    [Fact]
    public async Task Query()
    {
      SqlServerCommand command = new SqlServerCommand(ConnectionString);
      ComplexResponse<List<ResultTable>> result = await command.Query(@"Update Bitacora set Eliminado = GetDate() where Id = 1; Select @@ROWCOUNT;");
      Assert.True(result.Correct);
    }
  }
}
