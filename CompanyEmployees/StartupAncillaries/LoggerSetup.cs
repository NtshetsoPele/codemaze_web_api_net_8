namespace CompanyEmployees.StartupAncillaries;

public static class LoggerSetup
{
    public static void SetupLogging()
    {
        LogManager.Setup().LoadConfigurationFromFile(GetNLogFile());
    }
    
    private static string GetNLogFile() =>
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Resources.NLogConfig);
}