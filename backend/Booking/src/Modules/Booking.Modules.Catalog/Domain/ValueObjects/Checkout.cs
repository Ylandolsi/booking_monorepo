// using Booking.Common;

// namespace Booking.Modules.Catalog.Domain.ValueObjects;

// public class Checkout : ValueObject
// {
//     public string Url { get; private set; }
//     public string? Title { get; private set; }
//     public string? Thumbnail { get; private set; }
//     public string? Description { get; private set; }
//     public bool UseProductDefaults { get; private set; } = true;

//     private Checkout()
//     {
//     } // EF Core

//     public static Checkout Create(string url,
//         string? title = null,
//         string? thumbnail = null,
//         string? description = null)
//     {
//         return new Checkout
//         {
//             Url = url,
//             Title = title,
//             Thumbnail = thumbnail,
//             Description = description,
//             UseProductDefaults = string.IsNullOrEmpty(title) &&
//                                  string.IsNullOrEmpty(thumbnail) &&
//                                  string.IsNullOrEmpty(description)
//         };
//     }


//     protected override IEnumerable<object> GetEqualityComponents()
//     {
//         yield return Url;
//         yield return Title ?? "";
//         yield return Thumbnail ?? "";
//         yield return Description ?? "";
//         yield return UseProductDefaults;
//     }
// }

