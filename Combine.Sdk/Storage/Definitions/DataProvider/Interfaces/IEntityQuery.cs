using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Interfaces
{
  public interface IEntityQuery<T> : IQueryable where T : class, IEntity, new()
  {
  }
}
