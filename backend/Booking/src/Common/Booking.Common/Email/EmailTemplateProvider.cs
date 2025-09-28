using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Booking.Common.Email;

public sealed class EmailTemplateProvider
{
    private const string TemplateNamespace = "Booking.Common.Email.Templates";
    private readonly ILogger<EmailTemplateProvider> _logger;
    private readonly IMemoryCache _memoryCache;

    public EmailTemplateProvider(IMemoryCache memoryCache, ILogger<EmailTemplateProvider> logger)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<(string Subject, string Body)> GetTemplateAsync(
        string templateName,
        CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"EmailTemplate_{templateName}",
            async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromHours(1);
                return await ReadTemplateFromFileAsync(templateName, cancellationToken);
            });
    }

    private async Task<(string Subject, string Body)> ReadTemplateFromFileAsync(
        string templateName,
        CancellationToken cancellationToken)
    {
        var assembly = CommonAssembly.Assembly;
        var resourcePath = GetResourcePath(templateName);
        // using embeding ressources : ( as an alternative we can use file based approach )
        await using var stream = assembly.GetManifestResourceStream(resourcePath);

        if (stream is null)
        {
            _logger.LogCritical("Email template resource '{ResourcePath}' not found in assembly.", resourcePath);
            throw new FileNotFoundException("Email template file not found.", resourcePath);
        }

        using var reader = new StreamReader(stream);

        var subject = await reader.ReadLineAsync(cancellationToken) ?? string.Empty;
        var body = await reader.ReadToEndAsync(cancellationToken);

        return (subject, body);
    }

    private static string GetResourcePath(string templateName)
    {
        return $"{TemplateNamespace}.{templateName}.html";
    }
}