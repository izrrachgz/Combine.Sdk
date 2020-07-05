using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Combine.Sdk.Extensions.Json;
using Combine.Sdk.Extensions.CommonObjects;
using Combine.Sdk.Data.Definitions.Response;
using Combine.Sdk.Data.Definitions.Collection;
using Combine.Sdk.Storage.Definitions.DataProvider.Interfaces;

namespace Combine.Sdk.Storage.SqlServer.Extensions
{
  /// <summary>
  /// Provides extension methods for Entity type objects
  /// </summary>
  public static class EntityExtensions
  {
    /// <summary>
    /// Saves the changes made to an entity
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="entity">Entity reference</param>
    /// <returns>ComplexResponse Primary key inserted/updated</returns>
    public static async Task<BasicResponse> Save<T>(this T entity, SqlTransaction transaction = null) where T : class, IEntity, new()
    {
      //Verify entity state
      if (entity == null)
        return new BasicResponse(false, @"The specified entity is not valid for save operation.");
      string configUrl = $@"{AppDomain.CurrentDomain.BaseDirectory}Storage\SqlServer\DataProvider.json";
      //Verify configuration file
      if (!configUrl.IsFilePath(out Uri uri))
        return new BasicResponse(false, @"The configuration file is not present, the task could not be completed as requested.");
      JsonLoader<Configuration> jsonLoader = new JsonLoader<Configuration>(configUrl);
      await jsonLoader.Load();
      Configuration configuration = jsonLoader.Instance;
      string connectionString = configuration.Values
        .GetFirst<string>(@"ConnectionString");
      //Verify connection string value
      if (connectionString.IsNotValid())
        return new BasicResponse(false, @"The specified connection string in the configuration file is not valid for its use, the task could not be completed as requested.");
      //Save entity
      SqlServerDataProvider<T> provider = new SqlServerDataProvider<T>(connectionString);
      return await provider.Save(entity, transaction);
    }

    /// <summary>
    /// Saves the changes made to an entity
    /// list
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="entities">Entity list reference</param>
    /// <returns>ComplexResponse list of Primary key inserted/updated</returns>
    public static async Task<BasicResponse> Save<T>(this List<T> entities, SqlTransaction transaction = null) where T : class, IEntity, new()
    {
      //Verify entity state
      if (entities.IsNotValid())
        return new ComplexResponse<List<long>>(false, @"The specified entity list is not valid for save operation.");
      string configUrl = $@"{AppDomain.CurrentDomain.BaseDirectory}Storage\SqlServer\DataProvider.json";
      //Verify configuration file
      if (!configUrl.IsFilePath(out Uri uri))
        return new ComplexResponse<List<long>>(false, @"The configuration file is not present, the task could not be completed as requested.");
      JsonLoader<Configuration> jsonLoader = new JsonLoader<Configuration>(configUrl);
      await jsonLoader.Load();
      Configuration configuration = jsonLoader.Instance;
      string connectionString = configuration.Values
        .GetFirst<string>(@"ConnectionString");
      //Verify connection string value
      if (connectionString.IsNotValid())
        return new ComplexResponse<List<long>>(false, @"The specified connection string in the configuration file is not valid for its use, the task could not be completed as requested.");
      //Save entity list
      SqlServerDataProvider<T> provider = new SqlServerDataProvider<T>(connectionString);
      return await provider.Save(entities, transaction);
    }
  }
}
