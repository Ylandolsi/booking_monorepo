using Booking.Common.Results;
using Booking.Modules.Users.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Booking.Modules.Users.Features.Authentication;

public class RoleService(RoleManager<IdentityRole<int>> roleManager, UserManager<User> userManager)
{
    public async Task<Result> CreateRoleAsync(string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            return Result.Success();
        }

        return Result.Failure(Error.Failure("Role.Already.Exists", $"Role with name : {roleName} alreayd exists"));
    }

    public async Task<Result> AssignRoleToUserAsync(string roleName, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user != null)
        {
            if (!await userManager.IsInRoleAsync(user, roleName)) await userManager.AddToRoleAsync(user, roleName);

            return Result.Success();
        }

        return Result.Failure(Error.Failure("UserId.Dosent.Exists", $"User with id {userId} dosent exists"));
    }
}