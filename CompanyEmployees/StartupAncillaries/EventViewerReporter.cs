namespace CompanyEmployees.StartupAncillaries;

public abstract class EventViewerReporter
{
    public static void ReportError(Exception ex)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }
            
        EventLog eventLog = new()
        {
            Source = Resources.EventViewerSource
        };

        eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
    }
}