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
            .AddExceptionHandler<GlobalExceptionHandler>()
            .ConfigureCors()
            .ConfigureIisIntegration()
            .ConfigureLoggerService()
            .ConfigureRepositoryManager()
            .ConfigureServiceManager()
            .ConfigureSqlContext(builder.Configuration)
            .ConfigureEmployeeDataShaper()
            .AddAutoMapper(typeof(MapperAssemblyReference).Assembly)
            .AddScoped<ValidationFilterAttribute>(); ;
    }

    private static void AddStandardServices(WebAppBuilder builder)
    {
        builder.Services
            .Configure((ApiBehaviorOptions options) =>
            {
                options.SuppressModelStateInvalidFilter = true;
            })
            .AddControllers(configure: (MvcOptions opts) =>
            {
                opts.RespectBrowserAcceptHeader = true;
                opts.ReturnHttpNotAcceptable = true;
                opts.OutputFormatters.Add(new CsvOutputFormatter());
            })
            .AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters()
            .AddApplicationPart(typeof(PresentationAssemblyReference).Assembly);
    }

    private static void AddLoggingServices(WebAppBuilder builder)
    {
        builder.Host.UseNLog();
    }
}