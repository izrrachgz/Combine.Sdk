using System;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

namespace Combine.Sdk.Tests.Storage.DataProvider.SqlServer.Models
{
  /// <summary>
  /// Represents a basic testing entity data model
  /// </summary>
  internal class TestingEntity : IEntity
  {
    public long Id { get; set; }

    public string Name { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public DateTime? Deleted { get; set; }
  }
}
