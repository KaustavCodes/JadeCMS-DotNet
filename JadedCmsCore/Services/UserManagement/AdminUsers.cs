using JadedCmsCore.Interfaces.Database;
using JadedCmsCore.Services.Database.DbObjects;
using JadedEncryption;

namespace JadedCmsCore.Services.UserManagement;

public class AdminUsers
{
    private readonly IDatabaseService _databaseService;
    public AdminUsers(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public List<Admins> GetAdmins()
    {
        var admins = _databaseService.ExecuteQueryAsync<Admins>("SELECT * FROM Admins;").GetAwaiter().GetResult().ToList();
        return admins;
    }
}
