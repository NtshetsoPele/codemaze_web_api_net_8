
try
{
    LoggerSetup.SetupLogging();
    
    LogManager
        .GetCurrentClassLogger()
        .Info("App starting up.");
}
catch (Exception logSetupFailureEx)
{
    EventViewerReporter.ReportError(logSetupFailureEx);

    throw;
}

try
{
    WebApplication
        .CreateBuilder(args)
        .RegisterServices()
        .Build()
        .ConfigurePipeline()
        .Run();
}
catch (Exception appFailureEx)
{
    LogManager
        .GetCurrentClassLogger()
        .Fatal(appFailureEx);
    
    throw;
}
finally
{
    LogManager
        .GetCurrentClassLogger()
        .Info("App shutting down.");

    LogManager.Shutdown();
}