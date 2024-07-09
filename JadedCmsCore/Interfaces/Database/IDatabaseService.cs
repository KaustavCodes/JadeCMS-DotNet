using System.Data;

namespace JadedCmsCore.Interfaces.Database;

public interface IDatabaseService
{
    string ConnectionString { get; set; }
    IDbConnection Conection { get; set; }
    void OpenConection();
    void CloseConnection();
    
    Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, IEnumerable<IDbDataParameter> parameters = null);
    Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, IEnumerable<IDbDataParameter> parameters = null);
    Task ExecuteCommandAsync(string command, IEnumerable<IDbDataParameter> parameters = null);
}