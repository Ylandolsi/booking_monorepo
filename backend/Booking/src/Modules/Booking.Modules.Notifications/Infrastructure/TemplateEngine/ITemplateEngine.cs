namespace Booking.Modules.Notifications.Infrastructure.TemplateEngine;

/// <summary>
/// Template engine for rendering email templates with data
/// </summary>
public interface ITemplateEngine
{
    /// <summary>
    /// Renders a template with the provided data
    /// </summary>
    /// <param name="templateName">Name of the template to render</param>
    /// <param name="data">Data to populate the template</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Tuple containing (Subject, HtmlBody)</returns>
    Task<(string Subject, string HtmlBody)> RenderAsync(
        string templateName,
        object? data = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a template exists
    /// </summary>
    /// <param name="templateName">Name of the template</param>
    /// <returns>True if the template exists, false otherwise</returns>
    bool TemplateExists(string templateName);
}
