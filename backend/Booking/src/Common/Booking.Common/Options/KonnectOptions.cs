namespace Booking.Modules.Mentorships.Options;

public class KonnectOptions
{
    public static string OptionsKey = "Konnect";
    public string ApiUrl { get; set; }
    public string ApiKey { get; set;}
    public string WalletKey { get; set; }
    public int PaymentLifespan { get; set; }
    public string Webhook { get; set; }
    public string SuccessUrl { get; set; }
    public string FailureUrl { get; set; }
    
}