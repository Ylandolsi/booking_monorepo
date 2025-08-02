using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Profile.SocialLinks;

internal sealed class UpdateSocialLinksCommandHandler(
    UserManager<User> userManager,
    ILogger<UpdateSocialLinksCommandHandler> logger) : ICommandHandler<UpdateSocialLinksCommand>
{
    public async Task<Result> Handle(UpdateSocialLinksCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating social links for user {UserId}", command.UserId);

        User? user = await userManager.FindByIdAsync(command.UserId.ToString());
        if (user == null)
        {
            logger.LogWarning("User with ID {UserId} not found", command.UserId);
            return Result.Failure(UserErrors.NotFoundById(command.UserId));
        }

        var socialLinks = new Domain.ValueObjects.SocialLinks(
            command.LinkedIn,
            command.Twitter,
            command.Github,
            command.Youtube,
            command.Facebook,
            command.Instagram,
            command.Portfolio
        );

        user.UpdateSocialLinks(socialLinks);

        user.ProfileCompletionStatus.UpdateCompletionStatus(user);

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            logger.LogWarning("Failed to update social links for user {UserId}. Errors: {Errors}",
                command.UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
            return Result.Failure(Error.Problem("User.UpdateFailed", "Failed to update social links"));
        }

        logger.LogInformation("Successfully updated social links for user {UserId}", command.UserId);
        return Result.Success();
    }
}