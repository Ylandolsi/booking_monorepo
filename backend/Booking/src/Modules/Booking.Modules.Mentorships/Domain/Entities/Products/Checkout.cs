using Booking.Common;

namespace Booking.Modules.Mentorships.Domain.Entities.Products;

public class Checkout : ValueObject
{
    public string Url { get; private set; }
    public string? CustomTitle { get; private set; }
    public string? CustomThumbnail { get; private set; }
    public string? CustomDescription { get; private set; }
    public bool UseProductDefaults { get; private set; } = true;

    private Checkout()
    {
    } // EF Core

    public static Checkout Create(string url,
        string? customTitle = null,
        string? customThumbnail = null,
        string? customDescription = null)
    {
        return new Checkout
        {
            Url = url,
            CustomTitle = customTitle,
            CustomThumbnail = customThumbnail,
            CustomDescription = customDescription,
            UseProductDefaults = string.IsNullOrEmpty(customTitle) &&
                                 string.IsNullOrEmpty(customThumbnail) &&
                                 string.IsNullOrEmpty(customDescription)
        };
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
        yield return CustomTitle ?? "";
        yield return CustomThumbnail ?? "";
        yield return CustomDescription ?? "";
        yield return UseProductDefaults;
    }
}