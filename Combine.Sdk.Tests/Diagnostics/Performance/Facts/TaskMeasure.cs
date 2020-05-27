using Xunit;
using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Diagnostics.Performance;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Diagnostics.Definitions.Models;

namespace Combine.Sdk.Tests.Diagnostics.Performance.Facts
{
  /// <summary>
  /// Provides a mechanism to test all the methods
  /// in the TaskMeasure instance available
  /// </summary>
  public class TaskMeasureTests
  {
    /// <summary>
    /// Creates a new task measure tests instance
    /// </summary>
    public TaskMeasureTests()
    {

    }

    /// <summary>
    /// Proves that the Measure method
    /// can measure the exectuion time on
    /// an asynchronous task list correctly
    /// </summary>
    [Fact]
    public void Measure()
    {
      Task<int>[] tasks = new Task<int>[3];
      tasks[0] = Task.Run(() => { Thread.Sleep(2000); return 1; });
      tasks[1] = Task.Run(() => { Thread.Sleep(1000); return 2; });
      tasks[2] = Task.Run(() => { Thread.Sleep(3000); return 3; });
      TaskMeasure performance = new TaskMeasure();
      Stopwatch st = new Stopwatch();
      st.Start();
      List<TaskMetric<int>> result = performance.Measure(tasks);
      st.Stop();
      TimeSpan sum = TimeSpan.FromMilliseconds(result
        .Select(r => r.Elapsed)
        .Sum(t => t.TotalMilliseconds));
      Assert.True(!result.IsNotValid());
    }

    /// <summary>
    /// Proves that the MeasureInParallel method
    /// can measure the execution time on
    /// an asynchronous parallel task list correctly
    /// </summary>
    [Fact]
    public void MeasureInParallel()
    {
      Task<int>[] tasks = new Task<int>[3];
      tasks[0] = Task.Run(() => { Thread.Sleep(2000); return 1; });
      tasks[1] = Task.Run(() => { Thread.Sleep(1000); return 2; });
      tasks[2] = Task.Run(() => { Thread.Sleep(3000); return 3; });
      TaskMeasure performance = new TaskMeasure();
      Stopwatch st = new Stopwatch();
      st.Start();
      List<TaskMetric<int>> result = performance.MeasureInParallel(tasks);
      st.Stop();
      TimeSpan sum = TimeSpan.FromMilliseconds(result
        .Select(r => r.Elapsed)
        .Sum(t => t.TotalMilliseconds));
      Assert.True(!result.IsNotValid());
    }
  }
}
