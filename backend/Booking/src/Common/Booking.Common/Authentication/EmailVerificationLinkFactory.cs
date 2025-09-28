using System.Web;
using Microsoft.Extensions.Options;

namespace Booking.Common.Authentication;

public sealed class EmailVerificationLinkFactory(IOptions<FrontendApplicationOptions> frontEndOptions)
{
    private readonly FrontendApplicationOptions _frontEndOptions = frontEndOptions.Value;

    public string Create(string emailVerificationToken, string emailAdress)
    {
        // Ensure the token is properly encoded for URL usage
        // cuz it contains characters that are not safe for URL ( + , / , = .. )

        var builder = new UriBuilder(_frontEndOptions.BaseUrl)
        {
            Path = _frontEndOptions.EmailVerification,
            Query = $"token={HttpUtility.UrlEncode(emailVerificationToken)}&email={HttpUtility.UrlEncode(emailAdress)}"
        };
        var resetUrl = builder.ToString();

        return resetUrl ??
               throw new InvalidOperationException(
                   "Failed to generate email verification link. The link is null or empty.");
    }
}