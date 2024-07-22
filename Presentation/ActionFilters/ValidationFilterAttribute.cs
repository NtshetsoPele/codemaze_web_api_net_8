namespace Presentation.ActionFilters;

public class ValidationFilterAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        object? action = GetAction(context);
        object? controller = GetController(context);
        object? payload = GetPayload(context);
        
        if (payload is null)
        {
            AssignBadRequestResponse(context, controller, action);
            return;
        }
        
        if (!context.ModelState.IsValid)
        {
            AssignUnprocessableEntityResponse(context);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    { }

    private static object? GetAction(ActionExecutingContext context) => 
        context.RouteData.Values["action"];
    
    private static object? GetController(ActionExecutingContext context) => 
        context.RouteData.Values["controller"];

    private static object? GetPayload(ActionExecutingContext context)
    {
        return context.ActionArguments
            .SingleOrDefault((KeyValuePair<string, object?> pair) => ArgumentValueContainsKeyword(pair, "Request"))
            .Value;

        static bool ArgumentValueContainsKeyword(KeyValuePair<string, object?> pair, string keyword) =>
            pair.Value?.ToString()?.Contains(keyword) ?? false;
    }

    private static void AssignBadRequestResponse(ActionExecutingContext context, object? controller, object? action)
    {
        context.Result = new BadRequestObjectResult($"Payload is null. Controller: {controller}, action: {action}");
    }

    private static void AssignUnprocessableEntityResponse(ActionExecutingContext context)
    {
        context.Result = new UnprocessableEntityObjectResult(context.ModelState);
    }
}