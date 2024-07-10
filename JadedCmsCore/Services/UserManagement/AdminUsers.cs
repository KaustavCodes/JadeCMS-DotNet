using JadedCmsCore.Interfaces.Database;
using JadedCmsCore.Services.Database.DbObjects;

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
        var admins = _databaseService.ExecuteQueryAsync<Admins>("SELECT * FROM Admins;").Result.ToList();
        return admins;
    }
}
