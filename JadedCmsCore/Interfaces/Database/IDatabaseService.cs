using System.Data;

namespace JadedCmsCore.Interfaces.Database;

public interface IDatabaseService
{
    Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, IEnumerable<IDbDataParameter> parameters = null);
    Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, IEnumerable<IDbDataParameter> parameters = null);
    Task ExecuteCommandAsync(string command, IEnumerable<IDbDataParameter> parameters = null);
}