using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Delete;

public class DeleteProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(CatalogEndpoints.Products.Delete,
                async (string slug,
                    UserContext userContext,
                    ICommandHandler<DeleteProductCommand> handler,
                    CancellationToken cancellationToken) =>
                {
                    int userId = userContext.UserId;

                    var command = new DeleteProductCommand(userId, slug);
                    var result = await handler.Handle(command, cancellationToken);

                    return result.Match(
                        () => Results.Ok(), CustomResults.Problem
                    );
                }).RequireAuthorization()
            .WithTags("Products")
            .WithDescription("Delete product");
    }
}