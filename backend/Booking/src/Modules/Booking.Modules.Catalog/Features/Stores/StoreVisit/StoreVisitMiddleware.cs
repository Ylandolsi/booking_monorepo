using System.Threading.Channels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Stores.StoreVisit;

public class StoreVisitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly StoreVisitChannel _channel;
    private readonly ILogger<StoreVisitMiddleware> _logger;


    public StoreVisitMiddleware(RequestDelegate next, StoreVisitChannel channel, ILogger<StoreVisitMiddleware> logger)
    {
        _next = next;
        _channel = channel;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();

        // We only track store-related routes
        if (path is not null && path.StartsWith("/api/stores/"))
        {
            _logger.LogDebug("Store visit middleware triggered for path: {Path}", path);
            // Example paths:
            // /api/stores/my-shop/
            // /api/stores/my-shop/products/product-type/{{t-shirt : slug }}

            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length >= 3)
            {
                var storeSlug = segments[2];
                string cookieName = $"store_{storeSlug}_visitor";

                _logger.LogDebug("Processing store visit for storeSlug: {StoreSlug}, cookieName: {CookieName}", storeSlug, cookieName);

                if (!context.Request.Cookies.ContainsKey(cookieName))
                {
                    _logger.LogDebug("No existing cookie found, creating new visit for store: {StoreSlug}", storeSlug);

                    try
                    {
                        string? productSlug = null;

                        if (segments.Length >= 6 && segments[3] == "products")
                            productSlug = segments[5];
                        

                        var userAgent = context.Request.Headers["User-Agent"].ToString();
                        var ip = context.Connection.RemoteIpAddress?.ToString();

                        // Get referrer (the site the visitor came from)
                        var referrer = context.Request.Headers["Referer"].ToString();

                        _logger.LogDebug("Creating StoreVisit: StoreSlug={StoreSlug}, ProductSlug={ProductSlug}, UserAgent={UserAgent}, IP={IP}",
                            storeSlug, productSlug, userAgent, ip);

                        var visit = new Domain.Entities.StoreVisit(
                            storeSlug,
                            userAgent,
                            ip,
                            productSlug,
                            referrer
                        );

                        _logger.LogDebug("Queuing visit to channel");
                        await _channel.QueueVisitAsync(visit);
                        _logger.LogInformation("Successfully queued store visit for store: {StoreSlug}. Channel count: {Count}", 
                            storeSlug, _channel.GetCurrentCount());
                        
                        context.Response.Cookies.Append(cookieName, Guid.NewGuid().ToString(), new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddHours(1), // reset next hour
                            HttpOnly = true,
                            SameSite = SameSiteMode.Lax
                        });

                        _logger.LogDebug("Cookie set for store: {StoreSlug}", storeSlug);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process store visit for path: {Path}, store: {StoreSlug}", path, storeSlug);
                    }
                }
                else
                {
                    _logger.LogDebug("Cookie already exists for store: {StoreSlug}, skipping visit tracking", storeSlug);
                }
            }
            else
            {
                _logger.LogDebug("Path segments insufficient for store visit tracking. Path: {Path}, Segments: {Segments}", path, segments.Length);
            }
        }

        
        await _next(context);
    }
}