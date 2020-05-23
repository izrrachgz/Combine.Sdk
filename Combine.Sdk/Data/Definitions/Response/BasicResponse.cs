using System;
using Newtonsoft.Json;

namespace Combine.Sdk.Data.Definitions.Response
{
  /// <summary>
  /// Represents the most basic process-response
  /// data model
  /// </summary>
  public class BasicResponse
  {
    /// <summary>
    /// Specifies whether the invoked
    /// process task has completed
    /// correctly as expected
    /// </summary>
    public bool Correct { get; }

    /// <summary>
    /// Specifies the process-state
    /// message during completation
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Specifies the exception ocurred
    /// during the process resolving
    /// task
    /// </summary>
    [JsonIgnore]
    public Exception Exception { get; }

    /// <summary>
    /// Creates a new basic response
    /// </summary>
    public BasicResponse()
    {
      Correct = false;
      Message = @"There is not supplied message, more likely the task could not be solved as you requested.";
      Exception = null;
    }

    /// <summary>
    /// Creates a new basic response indicating
    /// the "correct" state
    /// </summary>
    /// <param name="correct"></param>
    public BasicResponse(bool correct)
    {
      Correct = correct;
      Message = Correct ? @"The current task has been executed as requested correctly." : @"The current task could not be solved as you requested, try again in a diferent way.";
      Exception = null;
    }

    /// <summary>
    /// Creates a new basic response indicating
    /// the "correct","message" state
    /// </summary>
    /// <param name="correct"></param>
    /// <param name="message"></param>
    public BasicResponse(bool correct, string message)
    {
      Correct = correct;
      Message = message;
      Exception = null;
    }

    /// <summary>
    /// Creates a new basic response indicating
    /// the exception ocurred during the resolving
    /// process task
    /// </summary>
    /// <param name="exception"></param>
    public BasicResponse(Exception exception)
    {
      Correct = false;
      Message = exception.Message ?? @"The current task could not be solved as you requested, see the internal error details and try again in a diferent way.";
      Exception = exception;
    }
  }
}
