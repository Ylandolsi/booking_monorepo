using Microsoft.AspNetCore.Authorization;

namespace Booking.Modules.Users.Features.Authentication.Authorizations;

public class RoleRequirement : IAuthorizationRequirement
{
    public RoleRequirement(params string[] roles)
    {
        RequiredRoles = roles;
    }

    public string[] RequiredRoles { get; set; }
}