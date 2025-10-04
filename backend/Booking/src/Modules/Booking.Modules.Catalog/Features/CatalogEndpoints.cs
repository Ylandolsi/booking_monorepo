namespace Booking.Modules.Catalog.Features;

public static class CatalogEndpoints
{
    private const string Base = "api";

    public static class Stores
    {
        public const string Create = Base + "/stores";
        public const string Update = Base + "/stores";
        public const string CheckSlugAvailability = Base + "/stores/slug-availability/{slug}";
        public const string UpdatePicture = Base + "/stores/picture";
        public const string GetMy = Base + "/stores/me";

        public const string GetPublic = Base + "/stores/{slug}";
    }

    public static class Products
    {
        public static class Sessions
        {
            public const string Create = $"{Base}/products/s/create";
            public const string Update = $"{Base}/products/s/{{productSlug}}";
            public const string GetMy = $"{Base}/products/s/{{productSlug}}/private";
            public const string GetAllSessions = $"{Base}/products/s"; // get booked session for the store owner 
            // queryParam :string? month ,string?year , string? timeZoneId


            // PUBLIC : 
            public const string Get = $"{Base}/products/s/{{productSlug}}";
            public const string Book = $"{Base}/products/s/{{productSlug}}";
            public const string GetDailyAv = $"{Base}/products/s/{{productSlug}}/availability"; // Query: date

            public const string
                GetMonthlyAv =
                    $"{Base}/products/s/{{productSlug}}/availability/month";
        }

        public const string Arrange = $"{Base}/produtcs/arrange"; 
        public const string Delete = $"{Base}/products/{{productSlug}}";
        public const string TogglePublished = $"{Base}/products/toggle/{{productSlug}}";
    }

    public static class Orders
    {
        public const string Get = $"{Base}" + "/orders/{orderId:int}";
        public const string GetMy = $"{Base}" + "/orders/me";
    }

    public static class Payouts
    {
        public const string Payout = $"{Base}/payout";
        public const string PayoutHistory = $"{Base}/payout";

        public static class Admin
        {
            public const string ApprovePayout = $"{Base}/admin/payout/approve"; // body : payoutId
            public const string WebhookPayout = $"{Base}/admin/payout/webhook";
            public const string RejectPayout = $"{Base}/admin/payout/reject"; // body : payoutId 

            public const string
                GetAllPayouts =
                    $"{Base}/admin/payout"; // query : status (Pending, Approved, Rejected , Completed) , upToDate , timeZoneId
            // public const string GetDetailled = $"{Base}/admin/payout/{{payoutId}}";
        }
    }

    public static class Payment
    {
        public const string Create = $"{Base}/payments";
        public const string GetWallet = $"{Base}/payments/wallet";
        public const string Webhook = $"{Base}/payments/webhook"; //  payment_ref=5f9498735289e405fc7c18ac
    }
}