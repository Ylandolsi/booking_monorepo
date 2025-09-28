using Booking.Api.Middlewares;

namespace Booking.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCancellationMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<CancellationMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}