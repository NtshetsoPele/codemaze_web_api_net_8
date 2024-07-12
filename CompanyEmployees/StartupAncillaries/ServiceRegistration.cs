namespace CompanyEmployees.StartupAncillaries;

public static class ServiceRegistration
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        AddCustomServices(builder);

        AddStandardServices(builder);

        AddLoggingServices(builder);

        return builder;
    }

    private static void AddCustomServices(WebApplicationBuilder builder)
    {
        builder.Services
            .ConfigureCors()
            .ConfigureIisIntegration()
            .ConfigureLoggerService()
            .ConfigureRepositoryManager()
            .ConfigureServiceManager()
            .ConfigureSqlContext(builder.Configuration);
    }

    private static void AddStandardServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
    }

    private static void AddLoggingServices(WebApplicationBuilder builder)
    {
        builder.Host.UseNLog();
    }
}