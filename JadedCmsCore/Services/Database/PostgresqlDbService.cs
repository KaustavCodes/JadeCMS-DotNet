using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using JadedCmsCore.Interfaces.Database;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Reflection;

namespace JadedCmsCore.Services.Database;

public class PostgreSqlDbService : IDatabaseService
{
    private readonly string _connectionString;

    public IDbConnection Connection { get; set; }

    public PostgreSqlDbService(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:DbConnection"];
    }

    public void OpenConnection()
    {
        Connection = new NpgsqlConnection(_connectionString);
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
        return new NpgsqlParameter
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

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(query, connection))
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
                    DataTable schemaTable = reader.GetSchemaTable();
                    var columnNames = schemaTable.Rows.Cast<DataRow>()
                                        .Select(row => row["ColumnName"].ToString()).ToList();

                    T instance = Activator.CreateInstance<T>();
                    var properties = typeof(T).GetProperties();

                    while (reader.Read())
                    {
                        foreach (var property in properties)
                        {
                            if (columnNames.Contains(property.Name) && !reader.IsDBNull(reader.GetOrdinal(property.Name)))
                            {
                                property.SetValue(instance, reader[property.Name]);
                            }
                        }
                    }
                    results.Add(instance);
                }
            }
        }

        return results;
    }

    public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, IEnumerable<IDbDataParameter> parameters = null)
    {
        var results = new List<T>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(storedProcedureName, connection))
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

    public async Task<Dictionary<string, object>> ExecuteStoredProcedureWithOutputAsync(string storedProcedureName, IEnumerable<IDbDataParameter> parameters)
    {
        var outputValues = new Dictionary<string, object>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                await command.ExecuteNonQueryAsync();

                foreach (NpgsqlParameter parameter in command.Parameters)
                {
                    if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput)
                    {
                        outputValues.Add(parameter.ParameterName, parameter.Value);
                    }
                }
            }
        }

        return outputValues;
    }

    public async Task ExecuteCommandAsync(string commandText, IEnumerable<IDbDataParameter> parameters = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(commandText, connection))
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