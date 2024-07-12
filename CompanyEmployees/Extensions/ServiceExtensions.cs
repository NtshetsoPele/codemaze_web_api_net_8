using IServices = Microsoft.Extensions.DependencyInjection.IServiceCollection;

namespace CompanyEmployees.Extensions;

public static class ServiceExtensions
{
    // Should be as restrictive as possible in production
    // E.g.:
    //     WithOrigins("https://example.com")
    //     WithMethods("POST", "GET")
    //     WithHeaders("accept", "content-type")
    public static IServices ConfigureCors(this IServices services)
    {
        return services.AddCors((CorsOptions options) =>
        {
            options.AddPolicy(name: "CorsPolicy", configurePolicy: (CorsPolicyBuilder builder) =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
    }

    public static IServices ConfigureIisIntegration(this IServices services)
    {
        return services.Configure<IISOptions>(options => { });
    }

    // Not needed. Plugged into the "Host"'s logging framework.
    public static IServices ConfigureLoggerService(this IServices services)
    {
        return services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static IServices ConfigureRepositoryManager(this IServices services)
    {
        return services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static IServices ConfigureServiceManager(this IServices services)
    {
        return services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static IServices ConfigureSqlContext(this IServices services, IConfiguration config)
    {
        return services.AddDbContext<RepositoryContext>((DbContextOptionsBuilder opts) =>
            opts.UseSqlServer(config.GetConnectionString(Resources.SqlConn)));
    }
}