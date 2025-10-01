using System.Text.Json;
using System.Text.Json.Serialization;
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
    public Dictionary<string, int>? Orders { get; init; } = null; //  product slug is a key , order : int
    public string? SocialLinksJson { get; init; } = null;

    // Computed property to deserialize SocialLinks
    public IList<SocialLink>? SocialLinks => string.IsNullOrWhiteSpace(SocialLinksJson)
        ? null
        : JsonSerializer.Deserialize<IList<SocialLink>>(SocialLinksJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() } // convert enum to string and vice vers  a 
        });

    /**
     * We need to deseralzie the social links because when using formdata
     * all of them are string => .net core can not bind string into object ( List<() > )
     */
}

public record PatchStoreCommand : PatchPostStoreRequest, ICommand<PatchPostStoreResponse>
{
    public required int UserId { get; init; }
}

public record PostStoreCommand : PatchStoreCommand ;