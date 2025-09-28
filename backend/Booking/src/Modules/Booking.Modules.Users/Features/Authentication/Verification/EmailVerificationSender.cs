using Booking.Common.Authentication;
using Booking.Modules.Users.BackgroundJobs.SendingVerificationEmail;
using Booking.Modules.Users.Domain.Entities;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.Verification;

internal sealed class EmailVerificationSender
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly EmailVerificationLinkFactory _emailVerificationLinkFactory;
    private readonly ILogger<EmailVerificationSender> _logger;
    private readonly UserManager<User> _userManager;


    public EmailVerificationSender(UserManager<User> userManager,
        EmailVerificationLinkFactory emailVerificationLinkFactory,
        IBackgroundJobClient backgroundJobClient,
        ILogger<EmailVerificationSender> logger)
    {
        _userManager = userManager;
        _emailVerificationLinkFactory = emailVerificationLinkFactory;
        _logger = logger;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task SendVerificationEmailAsync(User user)
    {
        _logger.LogInformation("Generating email verification token for user {UserId}", user.Id);

        var emailVerificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var verificationEmailLink = _emailVerificationLinkFactory.Create(emailVerificationToken, user.Email!);

        _logger.LogInformation("Enqueuing verification email job for {Email}", user.Email);

        try
        {
            _backgroundJobClient.Enqueue<VerificationEmailForRegistrationJob>(job =>
                job.SendAsync(user.Email!, verificationEmailLink, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enqueue verification email to {UserEmail}", user.Email);
            throw;
        }
    }
}