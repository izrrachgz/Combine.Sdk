using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Definitions.Paged;
using Combine.Sdk.Data.Definitions.Response;
using Combine.Sdk.Storage.DataProvider.SqlServer.Commands;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;
using Combine.Sdk.Storage.DataProvider.SqlServer.Extensions;
using Combine.Sdk.Storage.Definitions.DataProvider.Extensions;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

namespace Combine.Sdk.Storage.DataProvider.SqlServer
{
  /// <summary>
  /// Data provider implementation for sql server
  /// </summary>
  /// <typeparam name="T">Entity type</typeparam>
  public class SqlServerDataProvider<T> : IDataProvider<T> where T : class, IEntity, new()
  {
    /// <summary>
    /// Entity's table asociated name
    /// </summary>
    public string TableName { get; }

    /// <summary>
    /// Entity's table columns
    /// </summary>
    public string[] Columns { get; }

    /// <summary>
    /// Repository connection string
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// Plain sql insert statement text
    /// </summary>
    public string InsertTemplate => $@"Insert Into [dbo].[{TableName}] ([[columns]], [Created], [Modified], [Deleted]) OutPut Inserted.Id Values ([[values]], GetDate(), GetDate(), Null);";

    /// <summary>
    /// Plain sql update statement text
    /// </summary>
    public string UpdateTemplate => $@"Update [dbo].[{TableName}] Set [[values]], [Modified] = GetDate() Where [[conditions]]; Select RowCount_Big() as Total;";

    /// <summary>
    /// Creates a new sql server data provider instance
    /// </summary>
    /// <param name="connectionString">Repository's connection string</param>
    public SqlServerDataProvider(string connectionString)
    {
      ConnectionString = connectionString;
      T entity = new T();
      Columns = entity.GetPropertyNames();
      Type entityType = entity.GetType();
      TableName = entityType.Name;
    }

    /// <summary>
    /// Creates the entity sql insert statement
    /// </summary>
    /// <param name="parameters">Sql parameters</param>
    /// <returns>Sql insert statement including its parameters</returns>
    public string CreateInsertStatement(out object[] parameters)
    {
      string[] columns = Columns
        .Where(c => !c.Equals(@"Id") && !c.Equals(@"Created") && !c.Equals(@"Modified") && !c.Equals(@"Deleted"))
        .ToArray();
      parameters = new SqlParameter[columns.Length];
      for (int x = 0; x < columns.Length; x++)
        parameters[x] = new SqlParameter($@"@{columns[x]}", DBNull.Value);
      return InsertTemplate
       .Replace(@"[[columns]]", $@"[{string.Join(@"], [", columns)}]")
       .Replace(@"[[values]]", $@"@{string.Join(@", @", columns)}");
    }

    /// <summary>
    /// Creates the entity sql update statement
    /// </summary>
    /// <param name="parameters">Sql parameters</param>
    /// <returns>Sql insert statement including its parameters</returns>
    public string CreateUpdateStatement(out object[] parameters)
    {
      string[] columns = Columns
       .Where(c => !c.Equals(@"Id") && !c.Equals(@"Created") && !c.Equals(@"Modified") && !c.Equals(@"Deleted"))
       .ToArray();
      parameters = new SqlParameter[columns.Length];
      StringBuilder values = new StringBuilder();
      for (int x = 0; x < columns.Length; x++)
      {
        parameters[x] = new SqlParameter($@"@{columns[x]}", DBNull.Value);
        values.Append($@"[{columns[x]}] = @{columns[x]}");
        if (!(x + 1).Equals(columns.Length))
          values.Append(@",");
      }
      return UpdateTemplate
        .Replace(@"[[values]]", values.ToString())
        .Replace(@"[[conditions]]", @"[Id] = @Id And [Deleted] Is Null");
    }

    /// <summary>
    /// Retrieves the first entity that
    /// belongs to the specified key
    /// </summary>
    /// <param name="id">Primary key</param>
    /// <returns>ComplexResponse T</returns>
    public async Task<ComplexResponse<T>> GetFirst(long id)
      => await GetFirst(id, Columns);

    /// <summary>
    /// Retrieves the first entity that
    /// belongs to the specified key and
    /// includes the specified columns during
    /// selection
    /// </summary>
    /// <param name="id">Primary key</param>
    /// <param name="columns">Columns to include</param>
    /// <returns>ComplexReponse T</returns>
    public async Task<ComplexResponse<T>> GetFirst(long id, string[] columns)
    {
      //Verify entity primary key
      if (id <= 0)
        return new ComplexResponse<T>(false, @"The specified primary key is not valid.");
      //Verify select columns
      if (columns.IsNotValid())
        columns = Columns;
      //Verify integrity Columns (All the supplied columns must exists inside the current entity)
      if (!columns.ToList().TrueForAll(Columns.Contains))
        return new ComplexResponse<T>(false, @"The supplied columns does not exist in the current entity.");
      ComplexResponse<T> response;
      try
      {
        SqlServerCommand command = new SqlServerCommand(ConnectionString);
        string sql = $@"Select Top 1 [{string.Join(@"], [", columns)}] From [dbo].[{TableName}] Where [Id] = @Id And [Deleted] Is Null;";
        SqlParameter[] parameters = new SqlParameter[1]
        {
          new SqlParameter(@"@Id",id)
        };
        ComplexResponse<List<ResultTable>> commandResult = await command.Query(sql, parameters);
        if (commandResult.Correct)
        {
          T model = commandResult.Model
            .FirstOrDefault()
            .Rows.ElementAt(0)
            .ToEntity<T>();
          response = new ComplexResponse<T>(model);
        }
        else
        {
          response = new ComplexResponse<T>(false, commandResult.Message);
        }
      }
      catch (Exception ex)
      {
        response = new ComplexResponse<T>(ex);
      }
      return response;
    }

    /// <summary>
    /// Deletes the entity that belongs to the
    /// specified primary key
    /// </summary>
    /// <param name="id">Primary key</param>
    /// <returns>ComplexResponse</returns>
    public async Task<ComplexResponse<bool>> Delete(long id, object transaction = null)
    {
      //Verify entity primery key
      if (id <= 0)
        return new ComplexResponse<bool>(false, @"The specified primary key is not valid for delete operation.");
      return await Delete(new List<long>(1) { id }, transaction);
    }

    /// <summary>
    /// Deletes all the entities that belongs to
    /// the specified primary key list using a
    /// shared transaction
    /// </summary>
    /// <param name="ids">Primary key list</param>
    /// <param name="transaction">Shared transaction</param>
    /// <returns>ComplexResponse</returns>
    public async Task<ComplexResponse<bool>> Delete(List<long> ids, object transaction = null)
    {
      //Verify entities primary key list
      if (ids.IsNotValid())
        return new ComplexResponse<bool>(false, @"The primary key list specified is not valid for delete operation.");
      //Verify object transaction instance
      if (transaction != null && !(transaction is SqlTransaction))
        return new ComplexResponse<bool>(false, @"The specified shared transaction is not valid for delete operation.");
      ComplexResponse<bool> response;
      try
      {
        SqlTransaction tran = transaction as SqlTransaction;
        //Create an sql transaction and dispose all the resources
        //if the transaction is not supplied
        bool dispose = tran == null;
        SqlConnection connection;
        if (dispose)
        {
          //Initialize connection and transaction
          connection = new SqlConnection(ConnectionString);
          await connection.OpenAsync();
          tran = connection.BeginTransaction();
        }
        else
        {
          connection = tran.Connection;
        }
        string sql = $@"Update [dbo].[{TableName}] Set [Deleted] = GetDate() Where [Id] In ({string.Join(@",", ids)});";
        using (SqlCommand command = new SqlCommand(sql, tran.Connection, tran))
        {
          int deleted = await command.ExecuteNonQueryAsync();
          response = new ComplexResponse<bool>(deleted.Equals(ids.Count));
          if (dispose)
          {
            if (response.Correct)
              tran.Commit();
            else
              tran.Rollback();
          }
          command.Dispose();
        }
        //Dispose all the resources if the transaction is not supplied
        if (dispose)
        {
          tran.Dispose();
          if (connection.State.Equals(ConnectionState.Open))
            connection.Close();
          connection.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new ComplexResponse<bool>(ex);
      }
      return response;
    }

    /// <summary>
    /// Saves all the entity changes whether they
    /// need to be inserted or updated
    /// </summary>
    /// <param name="entity">Entity reference</param>
    /// <param name="transaction">Shared transaction</param>
    /// <returns>ComplexResponse Primary key</returns>
    public async Task<ComplexResponse<long>> Save(T entity, object transaction = null)
    {
      //Verify entities state before insert operation
      if (entity == null)
        return new ComplexResponse<long>(false, @"The specified entity is not valid for saving operation.");
      //Save all changes
      ComplexResponse<List<long>> result = await Save(new List<T>(1) { entity }, transaction);
      return new ComplexResponse<long>(result.Model.FirstOrDefault());
    }

    /// <summary>
    /// Saves all the entities whether they
    /// need to be inserted or updated
    /// </summary>
    /// <param name="entities">Entity list reference</param>
    /// <param name="transaction">Shared transaction</param>
    /// <returns>ComplexResponse Primary key list</returns>
    public async Task<ComplexResponse<List<long>>> Save(List<T> entities, object transaction = null)
    {
      //Verify entities state before insert operation
      if (entities.IsNotValid())
        return new ComplexResponse<List<long>>(false, @"The specified entity list is not valid for saving operation.");
      //Verify object transaction instance
      if (transaction != null && !(transaction is SqlTransaction))
        return new ComplexResponse<List<long>>(false, @"The specified shared transaction is not valid for delete operation.");
      ComplexResponse<List<long>> response;
      try
      {
        SqlTransaction tran = transaction as SqlTransaction;
        //Create an sql transaction and dispose all the resources
        //if the transaction is not supplied
        bool dispose = tran == null;
        SqlConnection connection;
        if (dispose)
        {
          //Initialize connection and transaction
          connection = new SqlConnection(ConnectionString);
          await connection.OpenAsync();
          tran = connection.BeginTransaction();
        }
        else
        {
          connection = tran.Connection;
        }
        //Create the sql statement
        string sqlInsert = CreateInsertStatement(out object[] parameters);
        string sqlUpdate = CreateUpdateStatement(out parameters);
        SqlParameter[] @params = parameters as SqlParameter[];
        //Create the sql command reference with the required values
        using (SqlCommand command = new SqlCommand(@"", connection, tran))
        {
          if (@params.Any())
            command.Parameters.AddRange(@params);
          //Inserted ids
          List<long> ids = new List<long>();
          //Insert each entity
          foreach (T e in entities)
          {
            //Determine operation
            bool insert = e.Id.Equals(0);
            command.CommandText = insert ? sqlInsert : sqlUpdate.Replace(@"@Id", e.Id.ToString());
            //Initialize parameter values            
            @params.SetValues(e);
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
              if (reader.HasRows)
              {
                //Read the results
                await reader.ReadAsync();
                long result = reader.GetInt64(0);
                //Get the inserted/updated id
                long id = insert ? result : (result > 0 ? e.Id : 0);
                ids.Add(id);
                reader.Close();
              }
              reader.Dispose();
            }
          }
          //Clear parameters
          command.Parameters.Clear();
          //Commit the transaction if is not supplied
          if (dispose)
          {
            //If there is any invalid id do rollback
            if (ids.Any(i => i.Equals(0)))
              tran.Rollback();
            else
              tran.Commit();
          }
          //Retrieve the ids inserted
          response = new ComplexResponse<List<long>>(ids);
          command.Dispose();
        }
        //Dispose all the resources if the transaction is not supplied
        if (dispose)
        {
          tran.Dispose();
          if (connection.State.Equals(ConnectionState.Open))
            connection.Close();
          connection.Dispose();
        }
      }
      catch (Exception ex)
      {
        response = new ComplexResponse<List<long>>(ex);
      }
      return response;
    }

    /// <summary>
    /// Retrieves all the entities that match with the
    /// page request and query conditions specified
    /// and includes the column select collection
    /// during the selection
    /// </summary>
    /// <param name="pagination">Page request</param>
    /// <param name="columns">Column selection collection</param>
    /// <param name="conditions">Query conditions to apply</param>
    /// <returns>ComplexResponse of Paginated collection</returns>
    public async Task<ComplexResponse<PaginatedCollection<T>>> GetRecords(Pagination pagination, string[] columns = null, List<QueryCondition> conditions = null)
    {
      //Verify pagination instance
      if (pagination == null)
        return new ComplexResponse<PaginatedCollection<T>>(false, @"The specified pagination instance is not valid.");
      //Initialize select columns
      if (columns.IsNotValid())
        columns = Columns;
      //Verify integrity Columns (All the supplied columns must exists inside the current entity)
      if (!columns.ToList().TrueForAll(Columns.Contains))
        return new ComplexResponse<PaginatedCollection<T>>(false, @"The supplied columns does not exist in the current entity.");
      //Inicialize query conditions
      if (conditions.IsNotValid())
      {
        conditions = new List<QueryCondition>();
      }
      else
      {
        //Validate query conditions integrity (All the supplied property-column query condition reference must exists inside the current entity)
        if (!conditions.Select(c => c.Property).ToList().TrueForAll(Columns.Contains))
          return new ComplexResponse<PaginatedCollection<T>>(false, @"The supplied columns does not exist in the current entity.");
      }
      ComplexResponse<PaginatedCollection<T>> response;
      try
      {
        //Build count query using the given parameters
        StringBuilder sqlCount = new StringBuilder($@"Select Cast(count(*) as BigInt) as Total From [dbo].[{TableName}] ");
        //Conditions to apply to
        List<string> defaultConditions = new List<string>();
        //Parameter list to pass
        List<SqlParameter> parameterList = new List<SqlParameter>();
        //Include or not deleted records
        if (!pagination.IncludeAll)
          defaultConditions.Add(@"[Deleted] Is Null");
        //Include keyword search inside searchable columns
        if (!pagination.KeyWords.IsNotValid())
        {
          string[] searchColumns = new T().GetStringPropertyNames();
          defaultConditions.Add($@"[{string.Join(@"] Like '%' + @SearchKeyWord + '%' Or [", searchColumns)}] Like '%' + @SearchKeyWord + '%'");
          parameterList.Add(new SqlParameter(@"@SearchKeyWord", pagination.KeyWords));
        }
        //Include date range start
        if (!pagination.Start.IsNotValid())
        {
          defaultConditions.Add(@"[Created] >= @StartAt");
          parameterList.Add(new SqlParameter(@"@StartAt", pagination.Start));
        }
        //Include date range end
        if (!pagination.End.IsNotValid())
        {
          defaultConditions.Add(@"[Created] <= @EndAt");
          parameterList.Add(new SqlParameter(@"@EntAt", pagination.End));
        }
        //Adds any conditions if applies
        if (defaultConditions.Any())
          sqlCount.Append($@"Where ({string.Join(@") And (", defaultConditions)}) ");
        //Command instance
        SqlServerCommand command = new SqlServerCommand(ConnectionString);
        //Get the user supplied conditions
        string userSqlConditions = conditions.ToSqlQuery(out SqlParameter[] userParameters);
        //Add the supplied user conditions
        if (!userSqlConditions.IsNotValid())
        {
          //Add 'where' clausule if there is not any default condition
          if (!defaultConditions.Any())
            sqlCount.Append(@"Where ");
          sqlCount.Append(userSqlConditions);
          parameterList.AddRange(userParameters);
        }
        sqlCount.Append(@";");
        //Params added
        SqlParameter[] parameters = parameterList.ToArray();
        //Count results
        ComplexResponse<List<ResultTable>> resultCount = await command.Query(sqlCount.ToString(), parameters);
        if (resultCount.Correct)
        {
          //Get the total records
          long total = resultCount.Model
            .First()
            .GetFirstResult<long>(@"Total");
          //Calculate the pagination size by the given total records
          pagination.Calculate(total);
          //Sql select instance
          StringBuilder sqlSelect = new StringBuilder($@"Select [{string.Join(@"], [", columns)}] From [dbo].[{TableName}] ");
          //Apply the same conditions as the count sql
          if (defaultConditions.Any())
            sqlSelect.Append($@"Where ({string.Join(@") And (", defaultConditions)})");
          //Add the supplied user conditions
          if (!userSqlConditions.IsNotValid())
          {
            //Add 'where' clausule if there is not any default condition
            if (!defaultConditions.Any())
              sqlSelect.Append(@"Where ");
            sqlSelect.Append(userSqlConditions);
          }
          //Skip and take records ordered by ascending id
          sqlSelect
            .Append($@"Order By [Id] Asc ")
            .Append($@"Offset {pagination.RequestedIndex * pagination.PageSize} Rows ")
            .Append($@"Fetch Next {pagination.PageSize} Rows Only;");
          //Page result
          ComplexResponse<List<ResultTable>> resultPage = await command.Query(sqlSelect.ToString(), parameters);
          if (resultPage.Correct)
          {
            //Create the page reference
            PaginatedCollection<T> page = new PaginatedCollection<T>(pagination, resultPage.Model.First().ToEntities<T>());
            //Create response
            response = new ComplexResponse<PaginatedCollection<T>>(page);
          }
          else
          {
            //The sql select query result is succeded but it has not returned any record
            response = new ComplexResponse<PaginatedCollection<T>>(false, resultPage.Message);
          }
        }
        else
        {
          //The sql count query result is succeded but it has not returned any record
          response = new ComplexResponse<PaginatedCollection<T>>(false, resultCount.Message);
        }
      }
      catch (Exception ex)
      {
        response = new ComplexResponse<PaginatedCollection<T>>(ex);
      }
      return response;
    }
  }
}
