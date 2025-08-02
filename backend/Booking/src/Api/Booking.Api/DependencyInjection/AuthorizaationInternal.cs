using Booking.Common.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Booking.Api.DependencyInjection;

public static  class AuthorizaationInternal
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