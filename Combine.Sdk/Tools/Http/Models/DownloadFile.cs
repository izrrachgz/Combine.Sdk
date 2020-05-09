namespace Combine.Sdk.Tools.Http.Models
{
  /// <summary>
  /// Represents a dowload file data model
  /// </summary>
  public class DownloadFile
  {
    /// <summary>
    /// Byte array content
    /// </summary>
    public byte[] Bytes { get; set; }

    /// <summary>
    /// Creates a new download file instance
    /// </summary>
    public DownloadFile()
    {

    }

    /// <summary>
    /// Creates a new download file instance 
    /// and initializes the bytes content
    /// </summary>
    /// <param name="bytes">Byte array content reference</param>
    public DownloadFile(byte[] bytes)
    {
      Bytes = bytes;
    }
  }
}
