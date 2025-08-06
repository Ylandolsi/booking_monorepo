using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Mentor.UpdateProfile;

internal sealed class UpdateMentorProfile : IEndpoint
{
    public sealed record Request(
        decimal HourlyRate,
        int? BufferTimeMinutes = null);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(MentorshipEndpoints.Mentors.UpdateProfile, async (
            Request request,
            UserContext userContext,
            ICommandHandler<UpdateMentorProfileCommand> handler,
            CancellationToken cancellationToken) =>
        {
            int mentorId = userContext.UserId; // Assuming mentorId = userId for now

            var command = new UpdateMentorProfileCommand(
                mentorId,
                request.HourlyRate,
                request.BufferTimeMinutes);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Mentor);
    }
}
