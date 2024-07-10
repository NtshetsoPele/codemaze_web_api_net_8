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
}