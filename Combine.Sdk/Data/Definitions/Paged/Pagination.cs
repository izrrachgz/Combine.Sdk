using System;

namespace Combine.Sdk.Data.Definitions.Paged
{
  /// <summary>
  /// Represent a page request data model
  /// </summary>
  public class Pagination
  {
    /// <summary>
    /// Internal access current page index position
    /// </summary>
    private long _CurrentIndex { get; set; }

    /// <summary>
    /// Current page index position
    /// </summary>
    public long CurrentIndex => _CurrentIndex;

    /// <summary>
    /// Internal access requested page index position
    /// </summary>
    private long _RequestedIndex { get; set; }

    /// <summary>
    /// Requested page index position
    /// </summary>
    public long RequestedIndex => _RequestedIndex;

    /// <summary>
    /// Internal access requested page size elements to show
    /// </summary>
    public long _PageSize { get; set; }

    /// <summary>
    /// Elements to show per page
    /// </summary>
    public long PageSize => _PageSize;

    /// <summary>
    /// Internal access total elements in collection
    /// </summary>
    public long _TotalElements { get; set; }

    /// <summary>
    /// Total elements in collection
    /// </summary>
    public long TotalElements => _TotalElements;

    /// <summary>
    /// Internal total pages in collection
    /// </summary>
    public long _TotalPages { get; set; }

    /// <summary>
    /// Total pages in collection
    /// </summary>
    public long TotalPages => _TotalPages;

    /// <summary>
    /// Internal date time start range
    /// </summary>
    private DateTime _Start { get; set; }

    /// <summary>
    /// Date time start range
    /// </summary>
    public DateTime Start => _Start;

    /// <summary>
    /// Internal date time end range
    /// </summary>
    private DateTime _End { get; set; }

    /// <summary>
    /// Date time end range
    /// </summary>
    public DateTime End => _End;

    /// <summary>
    /// Internal include all
    /// </summary>
    private bool _IncludeAll { get; set; }

    /// <summary>
    /// Include all elements
    /// </summary>
    public bool IncludeAll => _IncludeAll;

    /// <summary>
    /// Internal search keywords
    /// </summary>
    private string _KeyWords { get; set; }

    /// <summary>
    /// Search keywords
    /// </summary>
    public string KeyWords => _KeyWords;

    /// <summary>
    /// Creates a new pagination request instance
    /// </summary>
    public Pagination()
    {
      _KeyWords = @"";
      _IncludeAll = false;
      _CurrentIndex = 0;
      _RequestedIndex = 0;
      _PageSize = 100;
      _TotalElements = 0;
      _TotalPages = 0;
      _Start = DateTime.MinValue;
      _End = DateTime.MinValue;
    }

    /// <summary>
    /// Creates a new pagination request instance
    /// using the supplied configuration values
    /// </summary>
    /// <param name="index">Index to request for</param>
    /// <param name="pageSize">Page size to request for</param>
    /// <param name="keywords">Search keywords to include</param>
    /// <param name="includeAll">Include all elements</param>
    /// <param name="start">Time creation start range</param>
    /// <param name="end">Time creation end range</param>
    public Pagination(long index = 0, long pageSize = 100, string keywords = null, bool includeAll = false, DateTime? start = null, DateTime? end = null)
    {
      _RequestedIndex = index <= 0 ? 0 : index;
      _PageSize = pageSize <= 0 ? 100 : pageSize;
      _KeyWords = keywords ?? @"";
      _IncludeAll = includeAll;
      _Start = start ?? DateTime.MinValue;
      _End = end ?? DateTime.MinValue;
    }

    /// <summary>
    /// Calculates the page by the total
    /// elements founded in collection
    /// </summary>
    /// <param name="total">Total elements retained in collection</param>
    public void Calculate(long total)
    {
      if (total <= 0)
      {
        _CurrentIndex = 0;
        _RequestedIndex = 0;
        _PageSize = 0;
        _TotalElements = 0;
        _TotalPages = 0;
        return;
      }
      if (total <= _PageSize)
      {
        _CurrentIndex = 0;
        _RequestedIndex = 0;
        _PageSize = total;
        _TotalElements = total;
        _TotalPages = 1;
        return;
      }
      _TotalElements = total;
      _TotalPages = total / _PageSize;
      if (!(_TotalPages * _TotalElements).Equals(total)) _TotalPages++;
    }
  }
}
