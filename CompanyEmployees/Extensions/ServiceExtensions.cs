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

    public static IServices ConfigureResponseCaching(this IServices services)
    {
        return services.AddResponseCaching(); // --> Response Caching
    }

    public static IServices ConfigureOutputCaching(this IServices services)
    {
        return services.AddOutputCache((OutputCacheOptions opts) =>
        {
            // Applied to all controller actions
            opts.AddBasePolicy((OutputCachePolicyBuilder builder) => builder.Expire(TimeSpan.FromSeconds(10)));
            // Used for targeted usages
            opts.AddPolicy("120SecondsDuration", (OutputCachePolicyBuilder builder) => builder.Expire(TimeSpan.FromSeconds(120)));
        }); // --> Output Caching
    }

    public static IServices ConfigureRateLimiting(this IServices services)
    {
        return services.AddRateLimiter((RateLimiterOptions opts) =>
        {
            // Applies to all endpoints
            opts.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(partitioner: context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: "GlobalLimiter", factory: (string partition) => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true, // Default value
                            PermitLimit = 30,
                            QueueLimit = 2, // Served in the next window
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // Default value
                            Window = TimeSpan.FromMinutes(1)
                        }));

            // Doesn't override the above. 
            // After the 4th request in 10 seconds, the above takes over
            opts.AddPolicy("SpecificPolicy", partitioner: (HttpContext context) =>
                RateLimitPartition.GetFixedWindowLimiter(partitionKey: "SpecificLimiter",
                    factory: (string partition) => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true, 
                        PermitLimit = 3,
                        Window = TimeSpan.FromSeconds(10)
                    }));

            //opt.RejectionStatusCode = 429;
            opts.OnRejected = async (OnRejectedContext context, CancellationToken token) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                var errorMessage = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter) 
                    ? $"Too many requests. Please try again after { retryAfter.TotalSeconds } second(s)." 
                    : "Too many requests. Please try again later.";
                await context.HttpContext.Response.WriteAsync(errorMessage, token);
            };
        });
    }

    public static IServices AddAuth(this IServices services)
    {
        return services.AddAuthentication().Services;
    }

    public static IServices ConfigureIdentity(this IServices services)
    {
        return 
            services
                .AddIdentity<User, IdentityRole>((IdentityOptions opts) =>
                {
                    opts.Password.RequireDigit = true;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequiredLength = 10;
                    opts.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<RepositoryContext>()
                .AddDefaultTokenProviders()
                .Services;
    }
}