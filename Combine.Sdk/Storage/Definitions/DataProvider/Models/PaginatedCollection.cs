using System.Collections.Generic;
using Combine.Sdk.Data.Representation.Paged;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Models
{
  /// <summary>
  /// Provides a data representation for a
  /// collection page request
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class PaginatedCollection<T>
  {
    /// <summary>
    /// Page request
    /// </summary>
    public Pagination Pagination { get; }

    /// <summary>
    /// Asociated collection
    /// </summary>
    public List<T> Collection { get; }

    /// <summary>
    /// Creates a new paginated collection instance
    /// </summary>
    public PaginatedCollection()
    {
      Pagination = new Pagination();
      Collection = new List<T>(0);
    }

    /// <summary>
    /// Creates a new paginates collection instance
    /// with the specified values
    /// </summary>
    /// <param name="pagination">Page request</param>
    /// <param name="collection">Collection</param>
    public PaginatedCollection(Pagination pagination, List<T> collection)
    {
      Pagination = pagination;
      Collection = collection;
    }
  }
}
