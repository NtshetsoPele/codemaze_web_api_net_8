namespace CompanyEmployees.StartupAncillaries;

public static class PipelineConfiguration
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

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