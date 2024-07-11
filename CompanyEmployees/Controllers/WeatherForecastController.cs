namespace CompanyEmployees.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILoggerManager logger) : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        logger.LogDebug("Debug test log.");
        logger.LogWarn("Warn test log.");
        logger.LogInfo("Info test log.");
        logger.LogError("Error test log.");

        return ["vale1", "value2"];
    }
}