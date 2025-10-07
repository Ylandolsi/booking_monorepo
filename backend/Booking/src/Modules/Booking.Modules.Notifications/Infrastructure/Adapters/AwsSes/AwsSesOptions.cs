namespace Booking.Modules.Notifications.Infrastructure.Adapters.AwsSes;

public sealed class AwsSesOptions
{
    // TODO : we need to configure envs variable 
    public const string SectionKey = "Email:AwsSes";

    public string SenderEmail { get; set; } = string.Empty;
    public int MaxRetryAttempts { get; set; } = 3;
    public int RetryDelayMilliseconds { get; set; } = 1000;
}
