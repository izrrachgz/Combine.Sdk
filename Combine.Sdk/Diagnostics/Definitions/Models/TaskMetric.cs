using System;
using System.Threading;
using System.Threading.Tasks;

namespace Combine.Sdk.Diagnostics.Definitions.Models
{
  /// <summary>
  /// Provides a data model representation for a task measure
  /// object entity
  /// </summary>
  public class TaskMetric<T>
  {
    /// <summary>
    /// Thread reference identifier
    /// </summary>
    public int IdThread { get; set; }

    /// <summary>
    /// Task reference
    /// </summary>
    public Task<T> Task { get; set; }

    /// <summary>
    /// Time elapsed during task resolve state
    /// </summary>
    public TimeSpan Elapsed { get; set; }

    /// <summary>
    /// Creates a new task metric empty instance
    /// </summary>
    public TaskMetric() { }

    /// <summary>
    /// Creates a new task metric instance
    /// using the supplied property values 
    /// </summary>
    /// <param name="task">Task reference</param>
    /// <param name="elapsed">Time elapsed</param>
    public TaskMetric(Task<T> task, TimeSpan elapsed)
    {
      IdThread = Thread.CurrentThread.ManagedThreadId;
      Task = task;
      Elapsed = elapsed;
    }

    /// <summary>
    /// Creates a new task metric instance
    /// using the supplied property values
    /// </summary>
    /// <param name="thread">Thread identifier</param>
    /// <param name="task">Task reference</param>
    /// <param name="elapsed">Time elapsed</param>
    public TaskMetric(int thread, Task<T> task, TimeSpan elapsed)
    {
      IdThread = thread;
      Task = task;
      Elapsed = elapsed;
    }
  }
}
