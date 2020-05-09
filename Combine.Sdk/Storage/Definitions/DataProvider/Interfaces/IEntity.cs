using System;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Interfaces
{
  /// <summary>
  /// Provides the base data model representation
  /// for all entities
  /// </summary>
  public interface IEntity
  {
    /// <summary>
    /// Integer unique primary key
    /// </summary>
    long Id { get; set; }

    /// <summary>
    /// Moment of time when the entity was created
    /// </summary>
    DateTime Created { get; set; }

    /// <summary>
    /// Moment of time when the entity was last modified
    /// </summary>
    DateTime Modified { get; set; }

    /// <summary>
    /// Moment of time when the entity was deleted
    /// </summary>
    DateTime? Deleted { get; set; }
  }
}
