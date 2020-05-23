using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Representation.Response;
using Combine.Sdk.Storage.Definitions.DataProvider.Models;
using Combine.Sdk.Storage.DataProvider.SqlServer.Extensions;

namespace Combine.Sdk.Storage.DataProvider.SqlServer.Commands
{
  /// <summary>
  /// Provides the functionallity to execute
  /// sql server commands
  /// </summary>
  public sealed class SqlServerCommand
  {
    /// <summary>
    /// Repository connection string
    /// </summary>
    public string ConnectinString { get; }

    /// <summary>
    /// Creates a new sql server command instance
    /// using the specified connection string
    /// </summary>
    /// <param name="connectionString">Connection string</param>
    public SqlServerCommand(string connectionString)
    {
      ConnectinString = connectionString;
    }

    /// <summary>
    /// Retrieves the sql command results
    /// </summary>
    /// <param name="sql">Sql plain text query</param>
    /// <param name="parameters">Sql parameters</param>
    /// <param name="type">Command type</param>
    /// <param name="timeout">Timeout</param>
    /// <returns>Result Table list</returns>
    public async Task<ComplexResponse<List<ResultTable>>> SqlQuery(string sql, SqlParameter[] parameters = null, CommandType type = CommandType.Text, byte timeout = 30)
    {
      //Verify sql connection string
      if (sql.IsNotValid())
        return new ComplexResponse<List<ResultTable>>(false, @"The specified sql statement is not a valid to work with string value, your request has been rejected.");
      ComplexResponse<List<ResultTable>> response;
      using (SqlConnection connection = new SqlConnection(ConnectinString))
      {
        try
        {
          //Stablish connection to database
          await connection.OpenAsync();
          using (SqlCommand command = new SqlCommand(sql, connection))
          {
            //Set command execution type
            command.CommandType = type;
            //Set sql asociated parameters
            if (!parameters.IsNotValid())
              command.Parameters.AddRange(parameters);
            //Set timeout wait
            command.CommandTimeout = timeout <= 0 ? 30 : timeout;
            //Read results            
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
              //Begin to read if there is any row
              if (reader.HasRows)
              {
                List<ResultTable> tables = new List<ResultTable>();
                int tableIndex = 0;
                AddTableResult:
                ResultTable table = new ResultTable(tableIndex);
                int rowIndex = 0;
                //Read current result set
                while (await reader.ReadAsync())
                {
                  //Add the row data
                  ResultRow row = new ResultRow(rowIndex);
                  for (int x = 0; x < reader.FieldCount; x++)
                  {
                    //Add cell data
                    string column = reader.GetName(x);
                    column = column.IsNotValid() ? $@"{x}" : column;
                    row.Cells.Add(new ResultCell(rowIndex, x, column, reader.GetValue(x)));
                    //Add column name
                    if (rowIndex.Equals(0))
                      table.Columns.Add(column);
                  }
                  table.Rows.Add(row);
                  rowIndex++;
                }
                tables.Add(table);
                tableIndex++;
                //Read next result set and go back
                if (await reader.NextResultAsync())
                  goto AddTableResult;
                response = new ComplexResponse<List<ResultTable>>(tables);
                //Close data reader stream
                reader.Close();
              }
              else
              {
                //The sql statement has not returned any valid response, use default value.
                response = new ComplexResponse<List<ResultTable>>(false, @"The current sql statement has been executed as requested correctly, however, it did not returned any response.");
              }
              //Dispose all reader resources
              reader.Dispose();
            }
            //Clear parameters assosiation
            command.Parameters.Clear();
            //Clear timeout assosiation
            command.ResetCommandTimeout();
            //Dispose all the command resources
            command.Dispose();
          }
          //Close connection if opened
          if (connection.State.Equals(ConnectionState.Open))
            connection.Close();
        }
        catch (Exception ex)
        {
          response = new ComplexResponse<List<ResultTable>>(ex);
        }
        connection.Dispose();
      }
      return response;
    }

    /// <summary>
    /// Executes the specified stored procedure
    /// by name
    /// </summary>
    /// <param name="name">Stored procedure's name</param>
    /// <param name="parameters">Sql parameters</param>
    /// <param name="timeout">Timeout</param>
    /// <returns>Result Table list</returns>
    public async Task<ComplexResponse<List<ResultTable>>> StoredProcedure(string name, SqlParameter[] parameters = null, byte timeout = 30)
      => await SqlQuery(name, parameters, CommandType.StoredProcedure, timeout);

    /// <summary>
    /// Executes the plain text sql query 
    /// </summary>
    /// <param name="sql">Sql query</param>
    /// <param name="parameters">Sql parameters</param>
    /// <param name="timeout">Timeout</param>
    /// <returns>Result Table list</returns>
    public async Task<ComplexResponse<List<ResultTable>>> Query(string sql, SqlParameter[] parameters = null, byte timeout = 15)
      => await SqlQuery(sql, parameters, CommandType.Text, timeout);
  }
}
