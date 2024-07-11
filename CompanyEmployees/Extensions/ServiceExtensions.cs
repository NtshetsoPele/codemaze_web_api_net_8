namespace CompanyEmployees.Extensions;

public static class ServiceExtensions
{
    // Should be as restrictive as possible for production
    // E.g.:
    //     WithOrigins("https://example.com")
    //     WithMethods("POST", "GET")
    //     WithHeaders("accept", "content-type")
    public static void ConfigureCors(this IServiceCollection services) => 
        services.AddCors((CorsOptions options) =>
        {
            options.AddPolicy("CorsPolicy", (CorsPolicyBuilder builder) => 
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

    public static void ConfigureIisIntegration(this IServiceCollection services) => 
        services.Configure<IISOptions>(options => { });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection services) => 
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) => 
        services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) => 
        services.AddDbContext<RepositoryContext>((DbContextOptionsBuilder opts) => 
            opts.UseSqlServer(configuration.GetConnectionString(Resources.SqlConn)));
}