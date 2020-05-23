namespace Combine.Sdk.ToolBox.Http.Models
{
  /// <summary>
  /// Represents a dowload file data model
  /// </summary>
  public class RawBytes
  {
    /// <summary>
    /// Byte array content
    /// </summary>
    public byte[] Bytes { get; set; }

    /// <summary>
    /// Creates a new download file instance
    /// </summary>
    public RawBytes()
    {

    }

    /// <summary>
    /// Creates a new download file instance 
    /// and initializes the bytes content
    /// </summary>
    /// <param name="bytes">Byte array content reference</param>
    public RawBytes(byte[] bytes)
    {
      Bytes = bytes;
    }
  }
}
