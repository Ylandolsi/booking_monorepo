using Booking.Common.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Booking.Api.Services;

public static  class AuthorizationInternal
{
    public static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        
        services.AddAuthorization();

        services.AddScoped<PermissionProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}