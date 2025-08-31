using Booking.Modules.Users.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Booking.Common.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Modules.Users.Features.Authentication.Authorizations;

public class RoleRequirementHandler(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<RoleRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RoleRequirement requirement)
    {
        if (context.User is { Identity.IsAuthenticated: false })
        {
            context.Fail();
            return;
        }

        var userManager = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>();
        int userId = context.User.GetUserId() ??
                     throw new Exception("IdUser Claim doesnt exists , user is not authenticated");

        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            context.Fail();
            return;
        }

        if (await userManager.IsInRoleAsync(user, "Admin"))
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}