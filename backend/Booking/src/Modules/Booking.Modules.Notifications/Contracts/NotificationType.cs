namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Represents the type/category of notification being sent.
/// Used to categorize notifications for filtering, routing, and user preferences.
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// General system announcements or updates
    /// </summary>
    System = 1,

    /// <summary>
    /// Booking-related notifications (creation, updates, cancellations)
    /// </summary>
    Booking = 2,

    /// <summary>
    /// Payment-related notifications (successful payments, failed payments, refunds)
    /// </summary>
    Payment = 3,

    /// <summary>
    /// Account-related notifications (profile updates, security alerts)
    /// </summary>
    Account = 4,

    /// <summary>
    /// Marketing communications and promotional content
    /// </summary>
    Marketing = 5,

    /// <summary>
    /// Reminder notifications (upcoming appointments, deadlines)
    /// </summary>
    Reminder = 6,

    /// <summary>
    /// Administrative notifications for staff/admin users
    /// </summary>
    Administrative = 7,

    /// <summary>
    /// Integration failures 
    /// </summary>
    Integration = 8
}