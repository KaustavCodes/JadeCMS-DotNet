using System.Data;
using JadedCmsCore.Interfaces.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;

namespace JadedCmsCore.Services.Database;

public class MsSqlDbService : IDatabaseService
{
    private string _connectionString { get; set; }
    
    public IDbConnection Conection { get; set; }
    public MsSqlDbService(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:DbConnection"];
    }
    
    public void OpenConection()
    {
        Conection = new SqlConnection(_connectionString);
        Conection.Open();
    }
    
    public void CloseConnection()
    {
        Conection.Close();
    }

    public IDbDataParameter CreateParameter(string name, object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input, int size = 0)
    {
        var parameter = new SqlParameter
        {
            ParameterName = name,
            Value = value,
            DbType = dbType,
            Direction = direction,
            Size = size
        };
        return parameter;
    }
    
    public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, IEnumerable<IDbDataParameter> parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(query, parameters);
        }
    }

    public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, IEnumerable<IDbDataParameter> parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public async Task ExecuteCommandAsync(string command, IEnumerable<IDbDataParameter> parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var sqlCommand = new SqlCommand(command, connection))
            {
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        sqlCommand.Parameters.Add(parameter);
                    }
                }
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
    }
}