using Microsoft.Extensions.Configuration;

namespace JadedCmsCore.Services.Database;

public class DatabaseConfigurationService
{
    private readonly IConfiguration _configuration;

    public DatabaseConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetDatabaseType()
    {
        // Assuming there's a configuration key that specifies the database type
        return _configuration["DatabaseType"];
    }
}
