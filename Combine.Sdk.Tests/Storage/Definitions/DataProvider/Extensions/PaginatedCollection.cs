using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Excel;
using DocumentFormat.OpenXml.Packaging;
using Combine.Sdk.Data.Definitions.Paged;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;
using Combine.Sdk.Tests.Storage.DataProvider.SqlServer.Models;
using Combine.Sdk.Storage.Definitions.DataProvider.Extensions;

namespace Combine.Sdk.Tests.Storage.Definitions.DataProvider.Extensions
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the PaginatedCollection instance available
  /// </summary>
  public class PaginatedCollectionTests
  {
    /// <summary>
    /// Creates a new paginated colelction tests instance
    /// </summary>
    public PaginatedCollectionTests()
    {

    }

    /// <summary>
    /// Proves that the ToExcelDocument
    /// extension method converts a
    /// paginated collection object into
    /// a spreadsheet document
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task ToExcelDocument()
    {
      Pagination page = new Pagination();
      page.Calculate(1);
      List<TestingEntity> entities = new List<TestingEntity>() { new TestingEntity() { } };
      PaginatedCollection<TestingEntity> collection = new PaginatedCollection<TestingEntity>(page, entities);
      SpreadsheetDocument excel = await collection.ToExcelDocument();
      Assert.True(!excel.IsNotValid());
    }
  }
}
