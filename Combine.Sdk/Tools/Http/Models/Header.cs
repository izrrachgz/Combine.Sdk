namespace Combine.Sdk.Tools.Http.Models
{
  /// <summary>
  /// Represents the most basic data model
  /// for a http header
  /// </summary>
  public class HttpHeader
  {
    /// <summary>
    /// Header's name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Header's value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Creates a new header instance
    /// </summary>
    /// <param name="name">Header's name</param>
    /// <param name="value">Header's value</param>
    public HttpHeader(string name, string value)
    {
      Name = name;
      Value = value;
    }
  }
}
