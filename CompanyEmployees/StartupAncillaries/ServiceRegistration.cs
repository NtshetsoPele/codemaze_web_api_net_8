using WebAppBuilder = Microsoft.AspNetCore.Builder.WebApplicationBuilder;

namespace CompanyEmployees.StartupAncillaries;

public static class ServiceRegistration
{
    public static WebAppBuilder RegisterServices(this WebAppBuilder builder)
    {
        AddCustomServices(builder);
        AddStandardServices(builder);
        AddLoggingServices(builder);

        return builder;
    }

    private static void AddCustomServices(WebAppBuilder builder)
    {
        builder.Services
            .ConfigureCors()
            .ConfigureIisIntegration()
            .ConfigureLoggerService()
            .ConfigureRepositoryManager()
            .ConfigureServiceManager()
            .ConfigureSqlContext(builder.Configuration)
            .AddAutoMapper(typeof(MapperAssemblyReference).Assembly);
    }

    private static void AddStandardServices(WebAppBuilder builder)
    {
        builder.Services
            .AddControllers()
            .AddApplicationPart(typeof(PresentationAssemblyReference).Assembly);
    }

    private static void AddLoggingServices(WebAppBuilder builder)
    {
        builder.Host.UseNLog();
    }
}