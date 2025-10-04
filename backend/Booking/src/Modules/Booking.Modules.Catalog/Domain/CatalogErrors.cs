using Booking.Common.Results;

namespace Booking.Modules.Catalog.Domain;

/// <summary>
/// Centralized error definitions for the Catalog module.
/// Provides consistent, structured error messages throughout the module.
/// </summary>
public static class CatalogErrors
{
    public static class Store
    {
        public static readonly Error NotFound = Error.NotFound(
            "Store.NotFound",
            "The requested store was not found.");

        public static readonly Error SlugAlreadyExists = Error.Conflict(
            "Store.SlugAlreadyExists",
            "A store with this slug already exists.");

        public static readonly Error UserAlreadyHasStore = Error.Conflict(
            "Store.UserAlreadyHasStore",
            "User already owns a store.");

        public static readonly Error NotPublished = Error.Problem(
            "Store.NotPublished",
            "This store is not published.");

        public static readonly Error InvalidUserId = Error.Problem(
            "Store.InvalidUserId",
            "User ID must be greater than 0.");

        public static readonly Error InvalidTitle = Error.Problem(
            "Store.InvalidTitle",
            "Store title cannot be empty.");

        public static readonly Error TitleTooLong = Error.Problem(
            "Store.TitleTooLong",
            "Store title cannot exceed 100 characters.");

        public static readonly Error InvalidSlug = Error.Problem(
            "Store.InvalidSlug",
            "Store slug cannot be empty.");

        public static readonly Error SlugTooLong = Error.Problem(
            "Store.SlugTooLong",
            "Store slug cannot exceed 50 characters.");

        public static readonly Error DescriptionTooLong = Error.Problem(
            "Store.DescriptionTooLong",
            "Store description cannot exceed 1000 characters.");

        public static readonly Error RetrievalFailed = Error.Problem(
            "Store.Retrieval.Failed",
            "Failed to retrieve store details.");

        public static readonly Error UpdateFailed = Error.Problem(
            "Store.Update.Failed",
            "Failed to update store.");

        public static readonly Error CreationFailed = Error.Problem(
            "Store.Creation.Failed",
            "Failed to create store.");
    }

    public static class Product
    {
        public static readonly Error NotFound = Error.NotFound(
            "Product.NotFound",
            "The requested product was not found.");

        public static readonly Error UnauthorizedAccess = Error.Unauthorized(
            "Product.UnauthorizedAccess",
            "You do not have permission to access this product.");

        public static readonly Error NotPublished = Error.Problem(
            "Product.NotPublished",
            "This product is not published.");

        public static readonly Error InvalidDuration = Error.Problem(
            "Product.InvalidDuration",
            "Product duration must be a positive value.");

        public static readonly Error InvalidBufferTime = Error.Problem(
            "Product.InvalidBufferTime",
            "Buffer time cannot be negative.");

        public static readonly Error InvalidUserId = Error.Problem(
            "Product.InvalidUserId",
            "User ID must be greater than 0.");

        public static readonly Error InvalidSlug = Error.Problem(
            "Product.InvalidSlug",
            "Product slug cannot be empty.");

        public static readonly Error TogglePublishedFailed = Error.Problem(
            "Product.TogglePublished.Failed",
            "Failed to toggle product published status.");

        public static readonly Error DeleteFailed = Error.Problem(
            "Product.Delete.Failed",
            "Failed to delete product.");

        public static readonly Error SessionCreationFailed = Error.Problem(
            "SessionProduct.Creation.Failed",
            "Failed to create session product.");

        public static readonly Error SessionUpdateFailed = Error.Problem(
            "SessionProduct.Update.Failed",
            "Failed to update session product.");

        public static readonly Error SessionNotFound = Error.NotFound(
            "SessionProduct.NotFound",
            "Session product not found.");

        public static readonly Error ScheduleSetFailed = Error.Problem(
            "SessionProduct.ScheduleSetFailed",
            "Failed to set product schedule.");

        public static readonly Error ScheduleUpdateFailed = Error.Problem(
            "SessionProduct.ScheduleUpdateFailed",
            "Failed to update product schedule.");

        public static readonly Error ArrangeOrderFailed = Error.Failure(
            "ArrangeProductsOrderError",
            "Failed to arrange products order.");

        public static readonly Error ArrangeInvalidUserId = Error.Problem(
            "Arrange.Failed.InvalidUserId",
            "User ID must be greater than 0.");
    }

    public static class Wallet
    {
        public static readonly Error NotFound = Error.NotFound(
            "Wallet.NotFound",
            "Wallet not found for this store.");

        public static readonly Error InsufficientBalance = Error.Problem(
            "Wallet.InsufficientBalance",
            "Insufficient wallet balance for this operation.");
    }

    public static class Payout
    {
        public static readonly Error NotFound = Error.NotFound(
            "Payout.NotFound",
            "The requested payout was not found.");

        public static readonly Error InsufficientBalance = Error.Problem(
            "Payout.InsufficientBalance",
            "Insufficient balance to process this payout request.");

        public static readonly Error KonnectNotIntegrated = Error.Problem(
            "Payout.KonnectNotIntegrated",
            "Please integrate your account with Konnect before requesting a payout.");

        public static readonly Error PaymentLinkCreationFailed = Error.Failure(
            "Payout.PaymentLinkCreationFailed",
            "Failed to create payment link for payout.");
    }

    public static class Payment
    {
        public static readonly Error NotFound = Error.NotFound(
            "Payment.NotFound",
            "The requested payment was not found.");

        public static readonly Error AlreadyCompleted = Error.Problem(
            "Payment.AlreadyCompleted",
            "This payment has already been completed.");

        public static readonly Error AmountMismatch = Error.Problem(
            "Payment.AmountMismatch",
            "Payment amount does not match the order amount.");

        public static readonly Error ProcessingFailed = Error.Failure(
            "Payment.ProcessingFailed",
            "Payment processing failed. Please try again.");
    }

    public static class Order
    {
        public static readonly Error NotFound = Error.NotFound(
            "Order.NotFound",
            "The requested order was not found.");

        public static readonly Error SessionNotAvailable = Error.Problem(
            "Order.SessionNotAvailable",
            "The requested session is not available.");

        public static readonly Error InvalidTimeSlot = Error.Problem(
            "Order.InvalidTimeSlot",
            "The selected time slot is invalid or unavailable.");
    }

    public static class Upload
    {
        public static readonly Error FileNotProvided = Error.Problem(
            "Upload.FileNotProvided",
            "No file was provided for upload.");

        public static readonly Error InvalidFileType = Error.Problem(
            "Upload.InvalidFileType",
            "Invalid file type. Only image files are allowed.");

        public static readonly Error FileTooLarge = Error.Problem(
            "Upload.FileTooLarge",
            "File size exceeds the maximum allowed limit.");

        public static readonly Error UploadFailed = Error.Failure(
            "Upload.UploadFailed",
            "File upload failed. Please try again.");
    }

    public static class GoogleCalendar
    {
        public static readonly Error Unauthorized = Error.Unauthorized(
            "GoogleCalendar.Unauthorized",
            "Google Calendar access is unauthorized. Please reconnect your account.");

        public static readonly Error Forbidden = Error.Unauthorized(
            "GoogleCalendar.Forbidden",
            "Insufficient permissions to access Google Calendar.");

        public static readonly Error CalendarNotFound = Error.NotFound(
            "GoogleCalendar.CalendarNotFound",
            "The specified calendar was not found.");

        public static readonly Error RateLimitExceeded = Error.Problem(
            "GoogleCalendar.RateLimitExceeded",
            "Google Calendar API rate limit exceeded. Please try again later.");

        public static readonly Error TokenRefreshFailed = Error.Failure(
            "GoogleCalendar.TokenRefreshFailed",
            "Failed to refresh Google Calendar access token.");

        public static readonly Error EventCreationFailed = Error.Failure(
            "GoogleCalendar.EventCreationFailed",
            "Failed to create calendar event.");

        public static readonly Error NotIntegrated = Error.Problem(
            "GoogleCalendar.NotIntegrated",
            "Google Calendar is not integrated. Please connect your Google Calendar.");
    }

    public static class Session
    {
        public static readonly Error NotFound = Error.NotFound(
            "Session.NotFound",
            "The requested session was not found.");

        public static readonly Error InvalidTime = Error.Problem(
            "Session.InvalidTime",
            "The selected time is invalid or in the past.");

        public static readonly Error ProductNotAvailable = Error.Problem(
            "Session.ProductNotAvailable",
            "This session product is not available for booking.");

        public static readonly Error TimeConflict = Error.Problem(
            "Session.TimeConflict",
            "The selected time slot conflicts with an existing booking.");

        public static readonly Error InvalidPrice = Error.Problem(
            "Session.InvalidPrice",
            "Invalid session price.");

        public static readonly Error BookingFailed = Error.Failure(
            "Session.BookingFailed",
            "Failed to book the session. Please try again.");

        public static readonly Error CreationFailed = Error.Failure(
            "Session.CreationFailed",
            "Failed to create session product.");

        public static readonly Error UpdateFailed = Error.Failure(
            "Session.UpdateFailed",
            "Failed to update session product.");

        public static readonly Error ScheduleSetFailed = Error.Problem(
            "Session.ScheduleSetFailed",
            "Failed to set session schedule.");

        public static readonly Error ScheduleUpdateFailed = Error.Problem(
            "Session.ScheduleUpdateFailed",
            "Failed to update session schedule.");
    }

    public static class Availability
    {
        public static readonly Error GetMonthlyFailed = Error.Problem(
            "Availability.GetMonthlyFailed",
            "Failed to retrieve monthly availability.");

        public static readonly Error GetDailyFailed = Error.Problem(
            "Availability.GetDailyFailed",
            "Failed to retrieve daily availability.");
    }

    public static class Validation
    {
        public static readonly Error InvalidUserId = Error.Problem(
            "Validation.InvalidUserId",
            "User ID must be greater than 0.");

        public static readonly Error InvalidStatus = Error.Problem(
            "Validation.InvalidStatus",
            "Invalid status value provided.");
    }
}
