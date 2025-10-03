using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products;

public class ArrangeProductsOrderEndpoint : IEndpoint
{
    public record Request
    {
        public Dictionary<string, int>? Orders { get; init; } = null; //  product slug is a key , order : int
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(CatalogEndpoints.Products.Arrange,
                async (Request request, UserContext userContext, ICommandHandler<ArrangeProductsOrderCommand> handler,
                    CancellationToken cancellationToken) =>
                {
                    var userId = userContext.UserId; // Placeholder

                    var command = new ArrangeProductsOrderCommand
                    {
                        UserId = userId,
                        Orders = request.Orders ?? new Dictionary<string, int>()
                    };

                    var result = await handler.Handle(command, cancellationToken);

                    return result.Match(() => Results.Ok(result), CustomResults.Problem);
                })
            .WithTags("Product")
            .WithSummary("Update products order")
            .RequireAuthorization();
    }
}
