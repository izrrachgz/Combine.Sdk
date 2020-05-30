using System.Threading.Tasks;
using Combine.Sdk.Extensions.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Extensions
{
  /// <summary>
  /// Provides extension methods for PaginatedCollection type objects
  /// </summary>
  public static class PaginatedCollectionExtensions
  {
    /// <summary>
    /// Converts a paginated collection of entities into
    /// a spreadsheet document
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <param name="page">Page reference</param>
    /// <returns>SpreadsheetDocument</returns>
    public static async Task<SpreadsheetDocument> ToExcelDocument<T>(this PaginatedCollection<T> page) where T : class, IEntity, new()
    {
      //Verify page collection
      if (page == null || page.Collection.IsNotValid() || page.Pagination == null)
        return null;
      SpreadsheetDocument document = SpreadSheetExtensions.ExcelDocument();
      //Remove previous template sheets
      document.RemoveSheet(@"Report");
      document.RemoveSheet(@"Entity");
      document.AddSheet(@"Report");
      //Add a new sheet
      SheetData sheet = document.GetSheetData(@"Report");
      //Add the sheet data
      await sheet.AddRows(page.Collection.AsArray());
      return document;
    }
  }
}
