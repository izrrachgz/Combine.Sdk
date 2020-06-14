using System;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Definitions.Response;

namespace Combine.Sdk.ToolBox.Zip
{
  /// <summary>
  /// Provides a mechanism to creates zip files
  /// </summary>
  public class Zipper
  {
    /// <summary>
    /// Creates a new Zipper instance
    /// </summary>
    public Zipper()
    {

    }

    /// <summary>
    /// Creates a zip archive and adds all the files
    /// contained in the specified directory
    /// </summary>
    /// <param name="directory">Directory path</param>
    /// <param name="compression">Compression level</param>
    /// <returns>Zip path</returns>
    public async Task<BasicResponse> ZipDirectory(string directory, CompressionLevel compression = CompressionLevel.Optimal)
    {
      if (directory == null || !directory.IsDirectoryPath())
        return new BasicResponse(false, @"The supplied directory is not valid to work with.");
      BasicResponse response;
      try
      {
        string zipUrl = $@"{AppDomain.CurrentDomain.BaseDirectory}{Guid.NewGuid():N}.zip";
        await Task.Run(() => ZipFile.CreateFromDirectory(directory, zipUrl, compression, false));
        response = new BasicResponse(true, zipUrl);
      }
      catch (Exception ex)
      {
        response = new BasicResponse(ex);
      }
      return response;
    }

    /// <summary>
    /// Creates a zip archive and adds the file
    /// specified
    /// </summary>
    /// <param name="file">File reference</param>
    /// <param name="compression">Compression level</param>
    /// <returns>Zip path</returns>
    public async Task<BasicResponse> Zip(FileInfo file, CompressionLevel compression = CompressionLevel.Optimal)
    {
      if (file.IsNotValid())
        return new BasicResponse(false, @"The supplied file info is not valid to work with.");
      return await ZipFiles(new FileInfo[1] { file }, compression);
    }

    /// <summary>
    /// Creates a zip archive and adds all the files
    /// specified
    /// </summary>
    /// <param name="files">Files reference</param>
    /// <param name="compression">Compression level</param>
    /// <returns>Zip path</returns>
    public async Task<BasicResponse> ZipFiles(string[] files, CompressionLevel compression = CompressionLevel.Optimal)
    {
      if (files.IsNotValid())
        return new BasicResponse(false, @"The supplied file collection is not valid to work with.");
      BasicResponse response;
      try
      {
        List<FileInfo> archives = new List<FileInfo>(files.Count());
        foreach (string file in files)
          if (file.IsFilePath(out Uri uri))
            archives.Add(new FileInfo(uri.AbsolutePath));
        archives.TrimExcess();
        response = await ZipFiles(archives, compression);
      }
      catch (Exception ex)
      {
        response = new BasicResponse(ex);
      }
      return response;
    }

    /// <summary>
    /// Creates a zip archive and adds all the files
    /// specified
    /// </summary>
    /// <param name="files">Files reference</param>
    /// <param name="compression">Compression level</param>
    /// <returns>Zip path</returns>
    public async Task<BasicResponse> ZipFiles(IEnumerable<FileInfo> files, CompressionLevel compression = CompressionLevel.Optimal)
    {
      if (files == null || !files.Any())
        return new BasicResponse(false, @"The supplied file collection is not valid to work with.");
      BasicResponse response;
      try
      {
        string zipUrl = $@"{AppDomain.CurrentDomain.BaseDirectory}{Guid.NewGuid():N}.zip";
        using (FileStream fsZip = File.Create(zipUrl))
        {
          using (ZipArchive zip = new ZipArchive(fsZip, ZipArchiveMode.Update))
          {
            foreach (FileInfo file in files)
              await Task.Run(() => zip.CreateEntryFromFile(file.FullName, file.Name, compression));
          }
          await fsZip.FlushAsync();
        }
        response = new BasicResponse(true, zipUrl);
      }
      catch (Exception ex)
      {
        response = new BasicResponse(ex);
      }
      return response;
    }
  }
}
