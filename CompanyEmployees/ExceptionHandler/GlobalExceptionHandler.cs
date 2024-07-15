namespace CompanyEmployees.ExceptionHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        SetResponseContentType(httpContext);
        SetResponseStatusCode(httpContext, exception);
        await WriteErrorResponseAsync(httpContext, exception, cancellationToken);
        
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
        await context.Response.WriteAsJsonAsync(CreateErrorDetails(exception.Message), token);
    }

    private static ErrorDetails CreateErrorDetails(string error)
    {
        return new()
        {
            Message = error
        };
    }
}