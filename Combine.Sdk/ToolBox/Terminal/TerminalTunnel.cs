using System;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Definitions.Response;

namespace Combine.Sdk.ToolBox.Terminal
{
  /// <summary>
  /// Provides a mechanism to run command prompt/terminal commands
  /// through a non ui visible tunnel
  /// </summary>
  public class TerminalTunnel
  {
    /// <summary>
    /// Creates a new terminal tunnel instance
    /// </summary>
    public TerminalTunnel()
    {

    }

    /// <summary>
    /// Runs the specified command and
    /// collects the output response
    /// </summary>
    /// <param name="fileName">File, program, process</param>
    /// <param name="arguments">Command arguments</param>
    /// <returns>String output</returns>
    public async Task<ComplexResponse<StringBuilder>> Run(string fileName = @"cmd.exe", string arguments)
    {
      //Verify executable and command args
      if (fileName.IsNotValid() || arguments.IsNotValid())
        return new ComplexResponse<StringBuilder>(false, @"The supplied filename/arguments are not valid to work with.");
      ComplexResponse<StringBuilder> response;
      try
      {
        //Create process reference
        ProcessStartInfo info = new ProcessStartInfo
        {
          FileName = fileName,
          Arguments = arguments,
          RedirectStandardOutput = true
        };
        //Invoke process
        using (Process process = Process.Start(info))
        {
          StringBuilder sb = new StringBuilder();
          //Read redirected output stream
          while (!process.StandardOutput.EndOfStream)
            sb.AppendLine(await process.StandardOutput.ReadLineAsync());
          response = new ComplexResponse<StringBuilder>(sb);
          process.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new ComplexResponse<StringBuilder>(ex);
      }
      return response;
    }
  }
}
