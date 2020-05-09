using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Representation.Response;
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
    /// <returns>Dynamic dictionary collection</returns>
    private async Task<ComplexResponse<List<Dictionary<string, dynamic>>>> SqlQuery(string sql, SqlParameter[] parameters = null, CommandType type = CommandType.Text, byte timeout = 30)
    {
      if (sql.IsNotValid())
      {
        return new ComplexResponse<List<Dictionary<string, dynamic>>>(false, @"The specified sql statement is not a valid to work with string value, your request has been rejected.");
      }
      ComplexResponse<List<Dictionary<string, dynamic>>> response;
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
            command.CommandTimeout = timeout;
            //Read results
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
              //Begin to read if there is any row
              if (reader.HasRows)
              {
                List<Dictionary<string, dynamic>> results = new List<Dictionary<string, dynamic>>();
                //Take all entries into an object array
                while (await reader.ReadAsync())
                {
                  Dictionary<string, dynamic> data = new Dictionary<string, dynamic>(reader.FieldCount);
                  for (int x = 0; x < reader.FieldCount; x++)
                  {
                    string column = reader.GetName(x);
                    column = column.IsNotValid() ? $@"{x}" : column;
                    object value = reader.GetValue(x);
                    value = value.Equals(DBNull.Value) ? null : value;
                    data.Add(column, value);
                  }
                  results.Add(data);
                }
                response = new ComplexResponse<List<Dictionary<string, dynamic>>>(results);
                //Close data reader stream
                reader.Close();
              }
              else
              {
                //The sql statement has not returned any valid response, use default value.
                response = new ComplexResponse<List<Dictionary<string, dynamic>>>(false, @"The current sql statement has been executed as requested correctly, however, it did not returned any response.");
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
          response = new ComplexResponse<List<Dictionary<string, dynamic>>>(ex);
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
    /// <returns>Dynamic dictionary collection</returns>
    public async Task<ComplexResponse<List<Dictionary<string, dynamic>>>> StoredProcedure(string name, SqlParameter[] parameters = null, byte timeout = 30)
      => await SqlQuery(name, parameters, CommandType.StoredProcedure, timeout);

    /// <summary>
    /// Executes the plain text sql query 
    /// </summary>
    /// <param name="sql">Sql query</param>
    /// <param name="parameters">Sql parameters</param>
    /// <param name="timeout">Timeout</param>
    /// <returns>Dynamic dictionary collection</returns>
    public async Task<ComplexResponse<List<Dictionary<string, dynamic>>>> Query(string sql, SqlParameter[] parameters = null, byte timeout = 15)
      => await SqlQuery(sql, parameters, CommandType.Text, timeout);
  }
}
