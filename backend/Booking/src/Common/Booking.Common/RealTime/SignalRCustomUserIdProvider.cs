/*
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.SignalR;

namespace Booking.Common.RealTime;

public class SignalRCustomUserIdProvider : IUserIdProvider
{
    /// <inheritdoc />
    public virtual string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
    }
}
*/

