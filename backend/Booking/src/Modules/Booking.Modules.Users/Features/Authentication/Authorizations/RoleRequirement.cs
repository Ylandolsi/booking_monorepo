using Microsoft.AspNetCore.Authorization;

namespace Booking.Modules.Users.Features.Authentication.Authorizations;

public class RoleRequirement : IAuthorizationRequirement
{
    public string[] RequiredRoles { get; set; }

    public RoleRequirement(params string[] roles)
    {
        RequiredRoles = roles;
    }
}