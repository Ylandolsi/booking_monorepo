namespace Booking.Common.Authorization;

public sealed class PermissionProvider
{
    public Task<HashSet<string>> GetForUserIdAsync(int userId)
    {
        // Here you'll implement your logic to fetch permissions.
        HashSet<string> permissionsSet = [];

        return Task.FromResult(permissionsSet);
    }
}