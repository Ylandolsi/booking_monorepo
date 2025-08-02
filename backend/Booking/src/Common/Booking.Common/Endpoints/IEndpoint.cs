using Microsoft.AspNetCore.Routing;

namespace Booking.Common.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
