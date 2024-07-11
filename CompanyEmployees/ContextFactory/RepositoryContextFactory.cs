using ContextBuilder = Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<Repository.RepositoryContext>;
using IConfig = Microsoft.Extensions.Configuration.IConfigurationRoot;

namespace CompanyEmployees.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] _)
    {
        var config = GetConfig();
        
        ContextBuilder builder = GetDbContextBuilder(config); 
            
        return new RepositoryContext(builder.Options);
    }

    private static IConfig GetConfig()
    {
        var configBuilder = GetBaseConfigBuilder();

        var envSettings = TryToGetEnvSettings();

        if (ThereIsEnvSettings(envSettings))
        {
            LoadEnvSettings(configBuilder, envSettings);
        }

        return configBuilder.Build();
    }

    private static IConfigurationBuilder GetBaseConfigBuilder() =>
        new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(Resources.MainAppSettings, optional: false, reloadOnChange: true);

    private static string TryToGetEnvSettings() => $"appsettings.{GetEnvVar()}.json";

    private static bool ThereIsEnvSettings(string envSettings) =>
        File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, envSettings));

    private static void LoadEnvSettings(IConfigurationBuilder configBuilder, string envSettings) =>
        configBuilder.AddJsonFile(envSettings, optional: true, reloadOnChange: true);

    private static string? GetEnvVar() => Environment.GetEnvironmentVariable(Resources.EnvVar);

    private static ContextBuilder GetDbContextBuilder(IConfig config)
    {
        return new ContextBuilder()
            .UseSqlServer(
                config.GetConnectionString(Resources.SqlConn),
                (SqlServerDbContextOptionsBuilder b) => b.MigrationsAssembly("CompanyEmployees"));
    }
}