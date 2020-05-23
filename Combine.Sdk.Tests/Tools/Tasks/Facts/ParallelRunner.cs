using Xunit;
using System.Linq;
using Combine.Sdk.ToolBox.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Combine.Sdk.Tests.Tasks.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the ParallelRunner instance available
  /// </summary>
  public class FactParallelRunnerTests
  {
    /// <summary>
    /// Creates a new parallel runner tests instance
    /// </summary>
    public FactParallelRunnerTests()
    {

    }

    /// <summary>
    /// Proves that the Run method runs
    /// all the supplied tasks in an 
    /// asynchronous parallel way correctly
    /// </summary>
    /// <returns>Task State</returns>
    [Fact]
    public async Task Run()
    {
      Task<int>[] tasks = new Task<int>[3];
      tasks[0] = Task.Run(() => { Thread.Sleep(2000); return 1; });
      tasks[1] = Task.Run(() => { Thread.Sleep(1000); return 2; });
      tasks[2] = Task.Run(() => { Thread.Sleep(3000); return 3; });
      ParallelRunner runner = new ParallelRunner();
      await runner.Run(tasks);
      Assert.True(tasks.Select(t => t.Status).ToList().TrueForAll(s => s.Equals(TaskStatus.RanToCompletion)));
    }

    /// <summary>
    /// Proves that the Run method runs
    /// all the supplied tasks and waits in an 
    /// asynchronous parallel way correctly
    /// </summary>    
    [Fact]
    public void RunAndWait()
    {
      Task<int>[] tasks = new Task<int>[3];
      tasks[0] = Task.Run(() => { Thread.Sleep(2000); return 1; });
      tasks[1] = Task.Run(() => { Thread.Sleep(1000); return 2; });
      tasks[2] = Task.Run(() => { Thread.Sleep(3000); return 3; });
      ParallelRunner runner = new ParallelRunner();
      runner.RunAndWait(tasks);
      Assert.True(tasks.Select(t => t.Status).ToList().TrueForAll(s => s.Equals(TaskStatus.RanToCompletion)));
    }
  }
}
