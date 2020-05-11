using System.Threading.Tasks;
using System.Collections.Generic;

namespace Combine.Sdk.Tools.Tasks
{
  /// <summary>
  /// Provides a mechanism to runs tasks
  /// in parallel
  /// </summary>
  public class ParallelRunner
  {
    /// <summary>
    /// Creates a new runner task instance
    /// </summary>
    public ParallelRunner()
    {

    }

    /// <summary>
    /// Runs all the specified tasks in parallel
    /// in a non blocking thread way
    /// </summary>
    /// <typeparam name="T">Task result type</typeparam>
    /// <param name="tasks">Array of task</param>
    /// <returns>Task State</returns>
    public async Task Run<T>(Task<T>[] tasks)
    => await Task.Run(() => Parallel.ForEach(tasks, task => task.Wait()));

    /// <summary>
    /// Runs all the specified tasks in parallel
    /// and waits until all are finish
    /// </summary>
    /// <typeparam name="T">Task result type</typeparam>
    /// <param name="tasks">Array of task</param>
    public void RunAndWait<T>(Task<T>[] tasks)
    => Parallel.ForEach(tasks, task => task.Wait());

    /// <summary>
    /// Runs all the specified tasks in parallel
    /// in a non blocking thread way
    /// </summary>
    /// <typeparam name="T">Task result type</typeparam>
    /// <param name="tasks">Task list</param>
    /// <returns>Task State</returns>
    public async Task Run<T>(List<Task<T>> tasks)
    => await Task.Run(() => Parallel.ForEach(tasks, task => task.Wait()));

    /// <summary>
    /// Runs all the specified tasks in parallel
    /// and waits until all are finish
    /// </summary>
    /// <typeparam name="T">Task result type</typeparam>
    /// <param name="tasks">Task list</param>
    public void RunAndWait<T>(List<Task<T>> tasks)
    => Parallel.ForEach(tasks, task => task.Wait());
  }
}
