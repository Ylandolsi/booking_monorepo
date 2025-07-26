using Application.Abstractions.Messaging;
using Application.Users.Language;
using Application.Users.Language.Expose;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users.Language;

internal sealed class GetAllLanguages : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersEndpoints.GetAllLanguages, async (
                IQueryHandler<AllLanguagesQuery, List<LanguageResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                var query = new AllLanguagesQuery();
                Result<List<LanguageResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Language);
    }
}