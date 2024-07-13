﻿namespace CompanyEmployees.StartupAncillaries;

public static class PipelineConfiguration
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }
        
        app.UseExceptionHandler((IApplicationBuilder opts) => { });

        app.UseHttpsRedirection();
        //app.UseStaticFiles(); No static content available
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });
        app.UseCors(policyName: Resources.CorsPolicy);

        app.UseAuthorization();

        app.MapControllers();
        
        return app;
    }
}