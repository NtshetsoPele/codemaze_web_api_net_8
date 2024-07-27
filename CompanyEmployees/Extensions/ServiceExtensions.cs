using IServices = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using IConfig = Microsoft.Extensions.Configuration.IConfiguration;

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
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Pagination"));
        });
    }

    public static IServices ConfigureIisIntegration(this IServices services)
    {
        return services.Configure<IISOptions>(options => { });
    }

    // Not needed. Plugged into the "Hosts" logging framework.
    public static IServices AddLoggerService(this IServices services)
    {
        return services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static IServices AddRepositoryManager(this IServices services)
    {
        return services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static IServices AddServiceManager(this IServices services)
    {
        return services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static IServices AddDbContext(this IServices services, IConfig config)
    {
        return services.AddDbContext<RepositoryContext>((DbContextOptionsBuilder opts) =>
            opts.UseSqlServer(config.GetConnectionString(Resources.SqlConn)));
    }
    
    public static IServices AddEmployeeDataShaper(this IServices services)
    {
        return services.AddScoped<IDataShaper<ToClientEmployee>, DataShaper<ToClientEmployee>>();
    }

    public static IServices AddVersioning(this IServices services)
    {
        return 
            services.AddApiVersioning((ApiVersioningOptions opts) =>
            {
                opts.ReportApiVersions = true;
                opts.AssumeDefaultVersionWhenUnspecified = true;
                opts.DefaultApiVersion = new ApiVersion(majorVersion: 1, minorVersion: 0);
                opts.ApiVersionReader = new HeaderApiVersionReader(headerName: "api-version");
            })
            .AddMvc((MvcApiVersioningOptions opts) =>
            {
                opts.Conventions.Controller<CompaniesController>().HasApiVersion(new ApiVersion(1, 0));
                opts.Conventions.Controller<CompaniesV2Controller>().HasDeprecatedApiVersion(new ApiVersion(2, 0));
                opts.Conventions.Controller<RootController>().HasApiVersion(new ApiVersion(1, 0));
            })
            .Services;
    }

    public static IServices ConfigureResponseCaching(this IServices services) => services.AddResponseCaching();
}