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
            .AddLoggerService()
            .AddRepositoryManager()
            .AddServiceManager()
            .AddDbContext(builder.Configuration)
            .AddEmployeeDataShaper()
            .AddVersioning()
            .ConfigureResponseCaching()
            .AddAutoMapper(typeof(MapperAssemblyReference).Assembly)
            .AddScoped<ValidationFilterAttribute>();
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
                opts.OutputFormatters
                    .OfType<SystemTextJsonOutputFormatter>()
                    .FirstOrDefault()?.SupportedMediaTypes.Add("application/vnd.my-vendor.apiroot+json");
                opts.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                opts.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
            })
            .AddXmlDataContractSerializerFormatters()
            .AddApplicationPart(typeof(PresentationAssemblyReference).Assembly);
    }

    private static void AddLoggingServices(WebAppBuilder builder)
    {
        builder.Host.UseNLog();
    }

    private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
        new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
            .Services.BuildServiceProvider()
            .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>().First();
}