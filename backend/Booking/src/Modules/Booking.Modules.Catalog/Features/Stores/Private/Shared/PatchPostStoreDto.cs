using Booking.Common.Messaging;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Microsoft.AspNetCore.Http;

namespace Booking.Modules.Catalog.Features.Stores.Private.Shared;

public record PatchPostStoreRequest
{
    public required string Title { get; init; }
    public required string Slug { get; init; }
    public string Description { get; init; } = "";
    public IFormFile? File { get; init; } = null; 
    public Dictionary<string, int>? Orders { get; init; } = null;  //  product slug is a key , order : int 
    public IReadOnlyList<SocialLink>? SocialLinks { get; init; } = null;
}

public record PatchPostStoreCommand : PatchPostStoreRequest, ICommand<PatchPostStoreResponse>
{
    public required int UserId { get; init; }
}
