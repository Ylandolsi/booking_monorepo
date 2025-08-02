using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Profile.SocialLinks;

public record UpdateSocialLinksCommand(int UserId,
                                       string? LinkedIn,
                                       string? Twitter,
                                       string? Github,
                                       string? Youtube,
                                       string? Facebook,
                                       string? Instagram,
                                       string? Portfolio) : ICommand;