using Xunit;
using System.Text;
using System.Threading.Tasks;
using Combine.Sdk.ToolBox.Terminal;
using Combine.Sdk.Data.Definitions.Response;

namespace Combine.Sdk.Tests.ToolBox.Terminal.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the TerminalTunnel instance available
  /// </summary>
  public class TerminalTunnelTests
  {
    /// <summary>
    /// Creates a new terminal tunnel test instance
    /// </summary>
    public TerminalTunnelTests()
    {

    }

    /// <summary>
    /// Proves that the Run method
    /// runs the specified command
    /// through the terminal tunnel
    /// instance and collects the
    /// redirected output stream
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task Run()
    {
      TerminalTunnel tunnel = new TerminalTunnel();
      ModelResponse<StringBuilder> output = await tunnel.Run(@"wmic", @"cpu get name /Value");
      Assert.True(output.Correct);
    }
  }
}
