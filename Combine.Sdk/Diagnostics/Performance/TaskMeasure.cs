using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Diagnostics.Definitions.Models;

namespace Combine.Sdk.Diagnostics.Performance
{
  /// <summary>
  /// Provides a mechanism to measure tasks performance
  /// </summary>
  public class TaskMeasure
  {
    /// <summary>
    /// Creates a new task performance instance
    /// </summary>
    public TaskMeasure()
    {

    }

    /// <summary>
    /// Measures the time that takes each
    /// task to complte
    /// </summary>
    /// <typeparam name="T">Type of task</typeparam>
    /// <param name="tasks">Task array</param>
    /// <returns>List of key value task</returns>
    public List<TaskMetric<T>> Measure<T>(Task<T>[] tasks)
    {
      List<TaskMetric<T>> times = new List<TaskMetric<T>>();
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      while (tasks.Length > 0)
      {
        int index = Task.WaitAny(tasks);
        Task<T> completed = tasks[index];
        times.Add(new TaskMetric<T>(completed, stopwatch.Elapsed));
        List<Task<T>> temp = tasks
          .ToList();
        temp.RemoveAt(index);
        tasks = temp.ToArray();
      }
      stopwatch.Stop();
      return times;
    }

    /// <summary>
    /// Measures the time that takes each
    /// task to complte in parallel
    /// </summary>
    /// <typeparam name="T">Type of task</typeparam>
    /// <param name="tasks">Task array</param>
    /// <returns>List of key value task</returns>
    public List<TaskMetric<T>> MeasureInParallel<T>(Task<T>[] tasks)
    {
      List<TaskMetric<T>> times = new List<TaskMetric<T>>();
      Parallel.ForEach(tasks, task =>
      {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        task.Wait();
        times.Add(new TaskMetric<T>(Thread.CurrentThread.ManagedThreadId, task, stopwatch.Elapsed));
        stopwatch.Stop();
      });
      return times;
    }
  }
}
