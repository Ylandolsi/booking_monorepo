using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Mentor.BecomeMentor;

internal sealed class BecomeMentor : IEndpoint
{
    public sealed record Request(
        decimal HourlyRate,
        int BufferTimeMinutes = 15 );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Mentors.Become, async (
                Request request,
                UserContext userContext,
                ICommandHandler<BecomeMentorCommand> handler,
                CancellationToken cancellationToken) =>
            {
                int userId = userContext.UserId;
                string userSlug = userContext.UserSlug;

                var command = new BecomeMentorCommand(
                    userId,
                    userSlug,
                    request.HourlyRate,
                    request.BufferTimeMinutes
                );

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    () => Results.Ok() ,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Mentor);
    }
}