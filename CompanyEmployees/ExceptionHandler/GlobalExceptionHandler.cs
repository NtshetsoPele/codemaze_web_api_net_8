namespace CompanyEmployees.ExceptionHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception exception, CancellationToken token)
    {
        SetResponseContentType(context);
        SetResponseStatusCode(context, exception);
        await WriteErrorResponseAsync(context, exception, token);
        
        logger.LogCritical("Failure occured: {stackTrace}", exception.ToString());

        return true;
    }

    private static void SetResponseContentType(HttpContext context) => 
        context.Response.ContentType = Resources.JsonContent;

    private static void SetResponseStatusCode(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context, Exception exception, CancellationToken token)
    {
        var error = CreateErrorDetails(exception.Message);
        await context.Response.WriteAsJsonAsync(error, token);
    }

    private static ErrorDetails CreateErrorDetails(string error)
    {
        return new()
        {
            Message = error
        };
    }
}