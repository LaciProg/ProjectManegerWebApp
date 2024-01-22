namespace ProjectManager.Server.Middleware;

public class ErrorHandlerMiddleware {
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception)
        {
            context.Response.Redirect("/Error");
        }
    }
}