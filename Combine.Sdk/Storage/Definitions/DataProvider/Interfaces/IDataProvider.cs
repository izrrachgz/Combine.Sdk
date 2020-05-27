using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Data.Definitions.Paged;
using Combine.Sdk.Data.Definitions.Response;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;

namespace Combine.Sdk.Storage.Definitions.DataProvider.Interfaces
{
  /// <summary>
  /// Defines the base functionallity that all entities
  /// must have
  /// </summary>
  /// <typeparam name="T">Entity Type</typeparam>
  public interface IDataProvider<T> where T : class, IEntity, new()
  {
    /// <summary>
    /// Must be the table name asociated with the entity
    /// </summary>
    string TableName { get; }

    /// <summary>
    /// Must be all the entity columns
    /// </summary>
    string[] Columns { get; }

    /// <summary>
    /// Must be all the entity string type columns
    /// </summary>
    string[] SearchColumns { get; }

    /// <summary>
    /// Must be the repository connection string
    /// </summary>
    string ConnectionString { get; }

    /// <summary>
    /// Must define the sql insert statement template
    /// </summary>
    string InsertTemplate { get; }

    /// <summary>
    /// Must define the sql update statement template
    /// </summary>
    string UpdateTemplate { get; }

    /// <summary>
    /// Must retrieve the sql insert statement with
    /// all the required parameters as an output var
    /// </summary>
    /// <param name="parameters">Sql parameters</param>
    /// <returns>Sql Insert statement</returns>
    string CreateInsertStatement(out object[] parameters);

    /// <summary>
    /// Must retrieve the sql update statement with
    /// all the required parameters as an output var
    /// </summary>
    /// <param name="parameters">Sql parameters</param>
    /// <returns>Sql Update statement</returns>
    string CreateUpdateStatement(out object[] parameters);

    /// <summary>
    /// Must retrieve the first entity that belongs
    /// to the unique primary key given
    /// </summary>
    /// <param name="id">Primary key</param>
    /// <returns>ComplexResponse T</returns>
    Task<ComplexResponse<T>> GetFirst(long id);

    /// <summary>
    /// Must retrieve the first entity that belongs
    /// to the unique primary key given and
    /// include all the specified columns
    /// </summary>
    /// <param name="id">Primary key</param>
    /// <param name="columns">Columns to include in selection</param>
    /// <returns>ComplexResponse T</returns>
    Task<ComplexResponse<T>> GetFirst(long id, string[] columns);

    /// <summary>
    /// Must mark the entity as deleted by updating
    /// its deleted column property to the current
    /// moment
    /// </summary>
    /// <param name="id">Primary key</param>
    /// <returns>BasicResponse</returns>
    Task<BasicResponse> Delete(long id, object transaction);

    /// <summary>
    /// Must mark all the entities as deleted by updating
    /// its deleted column property to the current
    /// moment using an asociated shared transaction
    /// </summary>
    /// <param name="ids">Primary key list</param>
    /// <param name="transaction">Shared transaction</param>
    /// <returns>BasicResponse</returns>
    Task<BasicResponse> Delete(List<long> ids, object transaction);

    /// <summary>
    /// Must save all the entity changes
    /// whether they need to be inserted or
    /// updated and update the id, created
    /// and modified properties after the 
    /// saving operation
    /// </summary>
    /// <param name="entity">Entity reference</param>
    /// <param name="transaction">Shared transaction</param>
    /// <returns>BasicResponse</returns>
    Task<BasicResponse> Save(T entity, object transaction);

    /// <summary>
    /// Must save all the entities changes
    /// whether they need to be inserted or
    /// updated and update the id, created
    /// and modified properties after the 
    /// saving operation
    /// </summary>
    /// <param name="entities">Entity list reference</param>
    /// <param name="transaction">Shared transaction</param>
    /// <returns>BasicResponse</returns>
    Task<BasicResponse> Save(List<T> entities, object transaction);

    /// <summary>
    /// Must retrieve all the entities that match with the
    /// page request and query conditions specified
    /// and includes the column select collection
    /// during the selection
    /// </summary>
    /// <param name="pagination">Page request</param>
    /// <param name="columns">Column selection collection</param>
    /// <param name="conditions">Query conditions to apply</param>
    /// <returns>ComplexResponse of Paginated collection</returns>
    Task<ComplexResponse<PaginatedCollection<T>>> GetRecords(Pagination pagination, string[] columns = null, List<QueryCondition> conditions = null);
  }
}
