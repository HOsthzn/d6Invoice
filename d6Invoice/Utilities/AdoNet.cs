using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace d6Invoice.Utilities
{
  public class AdoNet
  {
    public AdoNet( string connectionString ) { ConnectionString = connectionString; }

    private string ConnectionString { get; set; }

    //This function will execute any SQL stored procedure and map it to an object
    public List< TModel > Stp< TModel >( string                          stpName
                                       , Hashtable                       parameters )
      where TModel : class, new()
    {
      if ( string.IsNullOrEmpty( stpName ) ) throw new ArgumentNullException( nameof( stpName ) );
      List< TModel > models = new List< TModel >();
      try
      {
        using ( TransactionScope transactionScope = new TransactionScope() )
        {
          using ( SqlConnection connection = new SqlConnection( ConnectionString ) )
          {
            using ( SqlCommand command = new SqlCommand() )
            {
              //command configuration, set connection, type and the stored procedure to be executed
              command.Connection  = connection;
              command.CommandType = CommandType.StoredProcedure;
              command.CommandText = stpName;

              MapParameters(command,parameters);
              
              //open the connection to the dataBase
              connection.Open();
              using ( SqlDataReader reader = command.ExecuteReader( CommandBehavior.CloseConnection ) )
              {
                try
                {
                  if ( reader.HasRows )
                  {
                    TModel model = new TModel();
                    while ( reader.Read() )
                    {
                      //use reflection to map the results to an object
                      Dictionary< string, int > indexer = model.GetType()
                                                               .GetProperties()
                                                               .ToDictionary( propInfo => propInfo.Name
                                                                           , propInfo
                                                                               => reader.GetOrdinal( propInfo.Name ) );
                      foreach ( KeyValuePair< string, int > keyValuePair in indexer )
                      {
                        model.GetType()
                             .GetProperty( keyValuePair.Key )
                             ?.SetValue( model, reader[ keyValuePair.Value ] );
                      }

                      models.Add( model );
                    }
                  }
                }
                finally
                {
                  //this step is only to make sure the connection was closed and the command disposed of correctly
                  command.Dispose();
                  if ( connection.State == ConnectionState.Open ) connection.Close();
                }
              }
            }
          }

          // The Complete method commits the transaction. If an exception has been thrown,
          // Complete is not  called and the transaction is rolled back.
          transactionScope.Complete();
        }
      }
      catch ( Exception e )
      {
        Console.WriteLine( e );
        throw;
      }

      return models;
    }

    //This function is exactly the same as the Stp function but executes asynchronously
    //(recommended: this is an I/O function)
    public async Task< List< TModel > > StpAsync< TModel >( string                          stpName
                                                          , Hashtable                       parameters )
      where TModel : class, new()
    {
      if ( string.IsNullOrEmpty( stpName ) ) throw new ArgumentNullException( nameof( stpName ) );
      List< TModel > models = new List< TModel >();
      try
      {
        using ( TransactionScope transactionScope = new TransactionScope() )
        {
          using ( SqlConnection connection = new SqlConnection( ConnectionString ) )
          {
            using ( SqlCommand command = new SqlCommand() )
            {
              //command configuration, set connection, type and the stored procedure to be executed
              command.Connection  = connection;
              command.CommandType = CommandType.StoredProcedure;
              command.CommandText = stpName;

              MapParameters(command,parameters);

              //open the connection to the dataBase
              connection.Open();
              using ( SqlDataReader reader = await command.ExecuteReaderAsync( CommandBehavior.CloseConnection ) )
              {
                try
                {
                  if ( reader.HasRows )
                  {
                    TModel model = new TModel();
                    while ( await reader.ReadAsync() )
                    {
                      //use reflection to map the results to an object
                      Dictionary< string, int > indexer = model.GetType()
                                                               .GetProperties()
                                                               .ToDictionary( propInfo => propInfo.Name
                                                                           , propInfo
                                                                               => reader.GetOrdinal( propInfo.Name ) );
                      foreach ( KeyValuePair< string, int > keyValuePair in indexer )
                      {
                        model.GetType()
                             .GetProperty( keyValuePair.Key )
                             ?.SetValue( model, reader[ keyValuePair.Value ] );
                      }

                      models.Add( model );
                    }
                  }
                }
                finally
                {
                  //this step is only to make sure the connection was closed and the command disposed of correctly
                  command.Dispose();
                  if ( connection.State == ConnectionState.Open ) connection.Close();
                }
              }
            }
          }

          // The Complete method commits the transaction. If an exception has been thrown,
          // Complete is not  called and the transaction is rolled back.
          transactionScope.Complete();
        }
      }
      catch ( Exception e )
      {
        Console.WriteLine( e );
        throw;
      }

      return models;
    }

    //This function will execute any SQL stored procedure but does not map to an object
    public DataSet Stp( string                          stpName
                      , Hashtable                       parameters )
    {
      DataSet dataSet = new DataSet();
      try
      {
        using ( TransactionScope transactionScope = new TransactionScope() )
        {
          using ( SqlConnection connection = new SqlConnection( ConnectionString ) )
          {
            using ( SqlCommand command = new SqlCommand() )
            {
              //command configuration, set connection, type and the stored procedure to be executed
              command.Connection  = connection;
              command.CommandType = CommandType.StoredProcedure;
              command.CommandText = stpName;

              MapParameters(command,parameters);

              //open the connection to the dataBase
              connection.Open();
              using ( SqlDataAdapter adapter = new SqlDataAdapter() )
              {
                try
                {
                  adapter.SelectCommand = command;
                  adapter.Fill( dataSet );
                }
                finally
                {
                  //this step is only to make sure the connection was closed and the command disposed of correctly
                  command.Dispose();
                  if ( connection.State == ConnectionState.Open ) connection.Close();
                }
              }

            }
          }
        }
      }
      catch ( Exception e )
      {
        Console.WriteLine( e );
        throw;
      }

      return dataSet;
    }

    //DataSets doesn't provide asynchronous functions, but putting the execution in a separate thread will prevent the main thread from locking up
    public async Task< DataSet > StpAsync( string    stpName
                                         , Hashtable parameters )
    {
      Task< DataSet > T = Task.Run( () => Stp( stpName, parameters ) );
      return await T;
    }

    private static void MapParameters( SqlCommand command, Hashtable parameters )
    {
      //add the parameter values to the command
      foreach ( DictionaryEntry parameter in parameters )
      {
        command.Parameters.AddWithValue( ( parameter.Key.ToString().StartsWith( "@" )
                                             ? parameter.Key
                                             : $@"{parameter.Key}" ).ToString()
                                      , parameter.Value );
      }
    }

  }
}