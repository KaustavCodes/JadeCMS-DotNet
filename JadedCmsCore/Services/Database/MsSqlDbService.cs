using System.Data;
using JadedCmsCore.Interfaces.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

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
    
    public Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, IEnumerable<IDbDataParameter> parameters = null)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, IEnumerable<IDbDataParameter> parameters = null)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteCommandAsync(string command, IEnumerable<IDbDataParameter> parameters = null)
    {
        throw new NotImplementedException();
    }
}