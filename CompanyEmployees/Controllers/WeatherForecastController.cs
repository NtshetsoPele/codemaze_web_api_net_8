namespace CompanyEmployees.Controllers;

[ApiController]
[Route(template: "[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        logger.LogDebug("Debug test log.");
        logger.LogWarning("Warn test log.");
        logger.LogInformation("Info test log.");
        logger.LogError("Error test log.");

        return ["vale1", "value2"];
    }
}