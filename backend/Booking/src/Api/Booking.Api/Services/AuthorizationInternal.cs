using Booking.Common.Authorization;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features.Authentication.Authorizations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Booking.Api.Services;

public static class AuthorizationInternal
{
    public static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole",
                policy => policy.Requirements.Add(new RoleRequirement("Admin")));
        });

        services.AddScoped<PermissionProvider>();
        // services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        //services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, RoleRequirementHandler>();

        return services;
    }
}