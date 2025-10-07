using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Notifications.Infrastructure.TemplateEngine;

/// <summary>
/// Template engine that loads templates from embedded resources and performs simple variable replacement
/// </summary>
public sealed class EmbeddedTemplateEngine : ITemplateEngine
{
    private const string TemplateNamespace = "Booking.Modules.Notifications.Infrastructure.TemplateEngine.Templates";
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<EmbeddedTemplateEngine> _logger;
    private readonly Assembly _assembly;

    public EmbeddedTemplateEngine(
        IMemoryCache memoryCache,
        ILogger<EmbeddedTemplateEngine> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _assembly = AssemblyReference.Assembly;
    }

    public async Task<(string Subject, string HtmlBody)> RenderAsync(
        string templateName,
        object? data = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Rendering template: {TemplateName}", templateName);

        // Get template from cache or load it
        var (subject, body) = await GetTemplateAsync(templateName, cancellationToken);

        // Replace variables if data is provided
        if (data != null)
        {
            subject = ReplaceVariables(subject, data);
            body = ReplaceVariables(body, data);
        }

        return (subject, body);
    }

    public bool TemplateExists(string templateName)
    {
        var resourcePath = GetResourcePath(templateName);
        var resourceName = _assembly.GetManifestResourceNames()
            .FirstOrDefault(name => name.Equals(resourcePath, StringComparison.OrdinalIgnoreCase));

        return resourceName != null;
    }

    private async Task<(string Subject, string Body)> GetTemplateAsync(
        string templateName,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"EmailTemplate_{templateName}";

        var template = await _memoryCache.GetOrCreateAsync(
            cacheKey,
            async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromHours(1);
                _logger.LogDebug("Loading template from embedded resource: {TemplateName}", templateName);
                return await ReadTemplateFromResourceAsync(templateName, cancellationToken);
            });

        if (template.Subject == null || template.Body == null)
        {
            throw new InvalidOperationException($"Template {templateName} could not be loaded");
        }

        return template;
    }

    private async Task<(string Subject, string Body)> ReadTemplateFromResourceAsync(
        string templateName,
        CancellationToken cancellationToken)
    {
        var resourcePath = GetResourcePath(templateName);

        await using var stream = _assembly.GetManifestResourceStream(resourcePath);

        if (stream == null)
        {
            _logger.LogError("Email template resource '{ResourcePath}' not found in assembly", resourcePath);
            throw new FileNotFoundException($"Email template '{templateName}' not found.", resourcePath);
        }

        using var reader = new StreamReader(stream);

        // First line is the subject
        var subject = await reader.ReadLineAsync(cancellationToken) ?? string.Empty;

        // Rest is the body
        var body = await reader.ReadToEndAsync(cancellationToken);

        _logger.LogDebug("Successfully loaded template: {TemplateName}", templateName);

        return (subject, body);
    }

    private static string GetResourcePath(string templateName)
    {
        return $"{TemplateNamespace}.{templateName}.html";
    }

    /// <summary>
    /// Simple variable replacement using {{VARIABLE_NAME}} syntax
    /// Supports nested properties using reflection
    /// </summary>
    private string ReplaceVariables(string template, object data)
    {
        if (string.IsNullOrEmpty(template) || data == null)
            return template;

        // Match {{VARIABLE_NAME}} pattern
        var pattern = @"\{\{([A-Z_]+)\}\}";
        var regex = new Regex(pattern);

        return regex.Replace(template, match =>
        {
            var variableName = match.Groups[1].Value;
            var value = GetPropertyValue(data, variableName);
            return value ?? match.Value; // Keep original if not found
        });
    }

    /// <summary>
    /// Gets property value from object using reflection
    /// Supports both PascalCase and UPPER_CASE property names
    /// </summary>
    private string? GetPropertyValue(object obj, string propertyName)
    {
        if (obj == null)
            return null;

        var type = obj.GetType();

        // Try exact match first
        var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        // Try converting UPPER_CASE to PascalCase (e.g., VERIFICATION_LINK -> VerificationLink)
        if (property == null)
        {
            var pascalCase = ConvertToPascalCase(propertyName);
            property = type.GetProperty(pascalCase, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        }

        if (property != null)
        {
            var value = property.GetValue(obj);
            return value?.ToString();
        }

        _logger.LogWarning("Property '{PropertyName}' not found on type '{TypeName}'", propertyName, type.Name);
        return null;
    }

    /// <summary>
    /// Converts UPPER_CASE to PascalCase
    /// Example: VERIFICATION_LINK -> VerificationLink
    /// </summary>
    private static string ConvertToPascalCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var words = input.Split('_');
        var result = string.Join("", words.Select(word =>
        {
            if (string.IsNullOrEmpty(word))
                return word;

            return char.ToUpper(word[0]) + word.Substring(1).ToLower();
        }));

        return result;
    }
}
