using ContextBuilder = Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<Repository.RepositoryContext>;
using IConfigRoot = Microsoft.Extensions.Configuration.IConfigurationRoot;
using IConfigBuilder = Microsoft.Extensions.Configuration.IConfigurationBuilder;

namespace CompanyEmployees.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] _)
    {
        IConfigRoot config = GetConfig();
        
        ContextBuilder builder = GetDbContextBuilder(config); 
            
        return new RepositoryContext(builder.Options);
    }

    private static IConfigRoot GetConfig()
    {
        IConfigBuilder builder = GetBaseConfigBuilder();

        string envSettings = TryToGetEnvSettings();

        if (ThereIsEnvSettings(envSettings))
        {
            LoadEnvSettings(builder, envSettings);
        }

        return builder.Build();
    }

    private static IConfigBuilder GetBaseConfigBuilder() =>
        new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(Resources.MainAppSettings, optional: false, reloadOnChange: true);

    private static string TryToGetEnvSettings() => $"appsettings.{GetEnvVar()}.json";

    private static string? GetEnvVar() => Environment.GetEnvironmentVariable(Resources.EnvVar);

    private static bool ThereIsEnvSettings(string envSettings) =>
        File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, envSettings));

    private static void LoadEnvSettings(IConfigBuilder builder, string settings) =>
        builder.AddJsonFile(settings, optional: true, reloadOnChange: true);

    private static ContextBuilder GetDbContextBuilder(IConfigRoot config)
    {
        return new ContextBuilder()
            .UseSqlServer(
                config.GetConnectionString(Resources.SqlConn),
                (SqlServerDbContextOptionsBuilder b) => 
                    b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
    }
}