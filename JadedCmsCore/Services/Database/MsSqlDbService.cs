using System.Data;
using JadedCmsCore.Interfaces.Database;
using Microsoft.Extensions.Configuration;

namespace JadedCmsCore.Services.Database;

public class MsSqlDbService : IDatabaseService
{
    private string ConnectionString { get; }
    
    private IDbConnection Conection { get; set; }
    public MsSqlDbService(IConfiguration configuration)
    {
        ConnectionString = configuration["ConnectionStrings:DbConnection"];
        
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