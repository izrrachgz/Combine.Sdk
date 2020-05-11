using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

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
    public List<KeyValuePair<Task<T>, TimeSpan>> Measure<T>(Task<T>[] tasks)
    {
      Stopwatch stopwatch = new Stopwatch();
      List<KeyValuePair<Task<T>, TimeSpan>> times = new List<KeyValuePair<Task<T>, TimeSpan>>();
      stopwatch.Start();
      while (tasks.Length > 0)
      {
        int index = Task.WaitAny(tasks);
        Task<T> completed = tasks[index];
        times.Add(new KeyValuePair<Task<T>, TimeSpan>(completed, stopwatch.Elapsed));
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
    public List<KeyValuePair<Task<T>, KeyValuePair<int, TimeSpan>>> MeasureInParallel<T>(Task<T>[] tasks)
    {
      List<KeyValuePair<Task<T>, KeyValuePair<int, TimeSpan>>> times = new List<KeyValuePair<Task<T>, KeyValuePair<int, TimeSpan>>>();
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      Parallel.ForEach(tasks, task =>
      {
        task.Wait();
        times.Add(new KeyValuePair<Task<T>, KeyValuePair<int, TimeSpan>>(task, new KeyValuePair<int, TimeSpan>(Thread.CurrentThread.ManagedThreadId, stopwatch.Elapsed)));
      });
      stopwatch.Stop();
      return times;
    }
  }
}
