using Booking.Common.Authentication;
using Booking.Modules.Users.Domain.Entities;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Identity;

namespace Booking.Api.Middlewares;

public class HangfireDashboardAuthorizationFilter(IApplicationBuilder app) : IDashboardAsyncAuthorizationFilter
{
    public async Task<bool> AuthorizeAsync(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();


        if (httpContext.User is { Identity.IsAuthenticated: false }) return false;

        var scope = app.ApplicationServices.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        // TODO : or we can replace this by checking the claims of user 
        var userId = httpContext.User.GetUserId() ??
                     throw new Exception("IdUser Claim doesnt exists , user is not authenticated");

        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null) return false;

        var isAdmin = await userManager.IsInRoleAsync(user, "admin");
        return isAdmin;
    }
}