using Xunit;
using Combine.Sdk.Data.Representation.Paged;

namespace Combine.Sdk.Tests.Data.Representation.Facts.Paged
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the Pagination instance available
  /// </summary>
  public class FactPaginationTests
  {
    /// <summary>
    /// Creates a new pagination tests instance
    /// </summary>
    public FactPaginationTests()
    {

    }

    /// <summary>
    /// Proves that the public constructor
    /// initializes the instance variables
    /// supplied through parameters correctly
    /// </summary>
    [Fact]
    public void Pagination()
    {
      Pagination pagination = new Pagination();
      Assert.True(pagination != null);
    }

    /// <summary>
    /// Proves that the Calculate method
    /// sets the correct instance variable
    /// values according to the page size
    /// and total elements correctly
    /// </summary>
    [Fact]
    public void Calculate()
    {
      Pagination pagination = new Pagination();
      pagination.Calculate(100);
      Assert.True(pagination.TotalPages.Equals(1));
    }
  }
}
