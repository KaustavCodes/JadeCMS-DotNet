using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using JadedCmsCore.Interfaces.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace JadedCmsCore.Services.Database;

public class MsSqlDbService : IDatabaseService
{
    private readonly string _connectionString;

    public IDbConnection Connection { get; set; }

    public MsSqlDbService(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:DbConnection"];
    }

    public void OpenConnection()
    {
        Connection = new SqlConnection(_connectionString);
        Connection.Open();
    }

    public void CloseConnection()
    {
        if (Connection != null && Connection.State == ConnectionState.Open)
        {
            Connection.Close();
        }
    }

    public IDbDataParameter CreateParameter(string name, object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input, int size = 0)
    {
        return new SqlParameter
        {
            ParameterName = name,
            Value = value,
            DbType = dbType,
            Direction = direction,
            Size = size
        };
    }

    public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, IEnumerable<IDbDataParameter> parameters = null)
    {
        var results = new List<T>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // Assuming T has a constructor that takes an IDataRecord
                        results.Add((T)Activator.CreateInstance(typeof(T), reader));
                    }
                }
            }
        }

        return results;
    }

    public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, IEnumerable<IDbDataParameter> parameters = null)
    {
        var results = new List<T>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // Assuming T has a constructor that takes an IDataRecord
                        results.Add((T)Activator.CreateInstance(typeof(T), reader));
                    }
                }
            }
        }

        return results;
    }

    public async Task ExecuteCommandAsync(string commandText, IEnumerable<IDbDataParameter> parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(commandText, connection))
            {
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}