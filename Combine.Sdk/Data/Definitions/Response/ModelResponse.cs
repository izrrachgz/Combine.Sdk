using System;

namespace Combine.Sdk.Data.Definitions.Response
{
  /// <summary>
  /// Represents the complex process-response
  /// data model
  /// </summary>
  /// <typeparam name="T">Complex type</typeparam>
  public class ModelResponse<T> : BasicResponse
  {
    /// <summary>
    /// Retains the process-asociated complex
    /// model
    /// </summary>
    public T Model { get; }

    /// <summary>
    /// Creates a new complex response instance
    /// </summary>
    public ModelResponse() : base(false)
    {
      Model = default;
    }

    /// <summary>
    /// Creates a new complex response instance
    /// containing the supplied model
    /// </summary>
    /// <param name="model"></param>
    public ModelResponse(T model) : base(model != null)
    {
      Model = model;
    }

    /// <summary>
    /// Creates a new complex response instance
    /// with an exception error ocurred during
    /// the resolving process task
    /// </summary>
    /// <param name="exception"></param>
    public ModelResponse(Exception exception) : base(exception)
    {
      Model = default;
    }

    /// <summary>
    /// Creates a new complex response instance
    /// with the default model value and the
    /// "correct","message" state
    /// </summary>
    /// <param name="correct"></param>
    /// <param name="message"></param>
    public ModelResponse(bool correct, string message) : base(correct, message)
    {
      Model = default;
    }
  }
}
