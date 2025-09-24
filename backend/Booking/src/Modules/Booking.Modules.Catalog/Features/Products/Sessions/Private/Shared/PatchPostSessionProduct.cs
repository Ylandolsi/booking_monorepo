using System.Text.Json;
using Booking.Common.Messaging;
using Booking.Modules.Catalog.Features.Products.Shared;
using Microsoft.AspNetCore.Http;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.Shared;

public record PostSessionProductCommand : PatchPostSessionProductRequest, ICommand<PatchPostProductResponse>
{
    public int UserId { get; init; }
}

public record PatchSessionProductCommand : PatchPostSessionProductRequest, ICommand<PatchPostProductResponse>
{
    public int UserId { get; init; }
    public required string ProductSlug { get; init; }
}

public record PatchPostSessionProductRequest
{
    public required string Title { get; init; }
    public required string Subtitle { get; init; }
    public string Description { get; init; } = "";
    public IFormFile? PreviewImage { get; init; }
    public IFormFile? ThumbnailImage { get; init; }
    public required string ClickToPay { get; init; }
    public required decimal Price { get; init; }
    public int DurationMinutes { get; init; }
    public int BufferTimeMinutes { get; init; }
    public string MeetingInstructions { get; init; } = "";
    public string TimeZoneId { get; init; } = "Africa/Tunis";

    public string? DayAvailabilitiesJson { get; init; } = null;

    // Computed property to deserialize SocialLinks
    public List<DayAvailability>? DayAvailabilities => string.IsNullOrWhiteSpace(DayAvailabilitiesJson)
        ? null
        : JsonSerializer.Deserialize<List<DayAvailability>>(DayAvailabilitiesJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
}