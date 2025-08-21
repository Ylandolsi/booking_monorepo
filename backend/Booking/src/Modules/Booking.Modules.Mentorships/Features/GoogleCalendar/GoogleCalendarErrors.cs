using Booking.Common.Results;

namespace Booking.Modules.Mentorships.Features.GoogleCalendar;

public static class GoogleCalendarErrors
{
    public static readonly Error TokensNotFound = Error.Problem(
        "GoogleCalendar.TokensNotFound", 
        "Google tokens not found for user");
    
    public static readonly Error FailedToCreateService = Error.Problem(
        "GoogleCalendar.FailedToCreateService", 
        "Failed to create Google Calendar service");
    
    public static readonly Error InitializationFailed = Error.Problem(
        "GoogleCalendar.InitializationFailed", 
        "Failed to initialize Google Calendar service");
    
    public static readonly Error ServiceNotInitialized = Error.Problem(
        "GoogleCalendar.ServiceNotInitialized", 
        "Google Calendar service not initialized");
    
    public static readonly Error Unauthorized = Error.Problem(
        "GoogleCalendar.Unauthorized", 
        "Authentication failed - please re-authenticate");
    
    public static readonly Error Forbidden = Error.Problem(
        "GoogleCalendar.Forbidden", 
        "Insufficient permissions to access calendar");
    
    public static readonly Error CalendarNotFound = Error.Problem(
        "GoogleCalendar.CalendarNotFound", 
        "Calendar not found");
    
    public static readonly Error RateLimitExceeded = Error.Problem(
        "GoogleCalendar.RateLimitExceeded", 
        "Rate limit exceeded - please try again later");
    
    public static readonly Error UnexpectedError = Error.Problem(
        "GoogleCalendar.UnexpectedError", 
        "An unexpected error occurred");
    
    public static readonly Error EventCreationFailed = Error.Problem(
        "GoogleCalendar.EventCreationFailed", 
        "Failed to create calendar event");
    
    public static readonly Error EventUpdateFailed = Error.Problem(
        "GoogleCalendar.EventUpdateFailed", 
        "Failed to update calendar event");
    
    public static readonly Error EventDeletionFailed = Error.Problem(
        "GoogleCalendar.EventDeletionFailed", 
        "Failed to delete calendar event");
}