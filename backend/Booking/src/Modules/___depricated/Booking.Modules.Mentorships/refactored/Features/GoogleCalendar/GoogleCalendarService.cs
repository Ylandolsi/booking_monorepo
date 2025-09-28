using System.Net;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Booking.Modules.Mentorships.refactored.Features.GoogleCalendar;

public class GoogleCalendarService
{
    private readonly GoogleOAuthOptions GoogleOAuthOptions;
    private readonly ILogger<GoogleCalendarService> Logger;
    private readonly string ApplicationName = "Meetini";
    private IUsersModuleApi UsersModuleApi;
    private CalendarService CalendarService;


    public GoogleCalendarService(
        IOptions<GoogleOAuthOptions> googleOAuthOptions,
        IUsersModuleApi usersModuleApi,
        ILogger<GoogleCalendarService> logger)
    {
        GoogleOAuthOptions = googleOAuthOptions.Value;
        UsersModuleApi = usersModuleApi;

        Logger = logger;
    }

    #region Init , Tokens management

    public async Task<Result> InitializeAsync(int userId)
    {
        var googleTokens = await UsersModuleApi.GetUserTokensAsync(userId);

        var clientSecrets = new ClientSecrets
        {
            ClientId = GoogleOAuthOptions.ClientId,
            ClientSecret = GoogleOAuthOptions.ClientSecret,
        };

        var flow =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = new[] { CalendarService.Scope.Calendar }
            });

        try
        {
            GoogleTokensDto? newTokens = await RefreshTokenAsync(googleTokens.RefreshToken, userId.ToString());
            newTokens ??= googleTokens; // keep old tokens if failed to refresh 
            var tokenResponse = new TokenResponse
            {
                AccessToken = newTokens.AccessToken,
                RefreshToken = newTokens.RefreshToken,
            };
            var credential = new UserCredential(flow, userId.ToString(), tokenResponse);

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            CalendarService = service;

            var updatedTokenInfo = new GoogleTokensDto
            {
                AccessToken = credential.Token.AccessToken,
                RefreshToken = credential.Token.RefreshToken,
                /*
                causes problem here
                ExpiresAt = tokenResponse.IssuedUtc.Add(TimeSpan.FromSeconds((long)tokenResponse.ExpiresInSeconds)),
                */
            };

            await UsersModuleApi.StoreUserTokensAsyncById(userId, updatedTokenInfo);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(GoogleCalendarErrors.FailedToCreateService);
        }
    }

    private async Task<GoogleTokensDto?> RefreshTokenAsync(string refreshToken, string userId = "user")
    {
        try
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = GoogleOAuthOptions.ClientId,
                    ClientSecret = GoogleOAuthOptions.ClientSecret,
                }
            });

            var tokenResponse = await flow.RefreshTokenAsync(userId, refreshToken, CancellationToken.None);

            return new GoogleTokensDto
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken ?? refreshToken, // Keep original if not returned
                /*
                 working here
                ExpiresAt = tokenResponse.IssuedUtc.Add(TimeSpan.FromSeconds((long)tokenResponse.ExpiresInSeconds)),
            */
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to refresh token");
            return null;
        }
    }

    public async Task<bool> ValidateTokenAsync(string accessToken)
    {
        try
        {
            const string linkToVerify = "https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=";
            using var httpClient = new HttpClient();
            var response =
                await httpClient.GetAsync($"{linkToVerify}{accessToken}");

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            var content = await response.Content.ReadAsStringAsync();
            // You can parse the JSON response to get more details about the token
            return !string.IsNullOrEmpty(content);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to validate token");
            return false;
        }
    }

    #endregion

    #region Calendar Crud operations

    public async Task<Result<Calendar>> GetCalendarAsync(string calendarId = "primary")
    {
        try
        {
            var request = CalendarService.Calendars.Get(calendarId);
            var response = await request.ExecuteAsync();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<Calendar>(GoogleCalendarErrors.FailedToGetCalendar);
        }
    }


    public async Task<Result<Calendar>> CreateEventAsync(Event newEvent, string calendarId = "primary")
    {
        try
        {
            var request = CalendarService.Events.Insert(newEvent, calendarId);
            var response = await request.ExecuteAsync();

            var reqCalendar = CalendarService.Calendars.Get(calendarId);
            var respCalendar = await reqCalendar.ExecuteAsync();
            return Result.Success(respCalendar);
        }
        catch (Exception ex)
        {
            return Result.Failure<Calendar>(GoogleCalendarErrors.EventCreationFailed);
        }
    }

    public async Task<Result<IList<Event>>> GetEventsAsync(int userId,
        string calendarId = "primary", DateTime? timeMin = null, DateTime? timeMax = null, int maxResults = 10)
    {
        // TODO : change DateTime to datetimeoffset 
        var googleTokens = await UsersModuleApi.GetUserTokensAsync(userId);

        try
        {
            var request = CalendarService.Events.List(calendarId);
            request.TimeMaxDateTimeOffset = timeMin ?? DateTime.Now;
            request.TimeMaxDateTimeOffset = timeMax;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = maxResults;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            var events = await request.ExecuteAsync(); // TODO : add polly here S
            var response = events.Items ?? new List<Event>();

            return Result.Success(response);
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.Unauthorized)
        {
            Logger.LogWarning("Unauthorized access - token may be invalid or expired");
            return Result.Failure<IList<Event>>(GoogleCalendarErrors.Unauthorized);
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogWarning("Access forbidden - insufficient permissions");
            return Result.Failure<IList<Event>>(GoogleCalendarErrors.Forbidden);
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
        {
            Logger.LogWarning("Calendar not found: {CalendarId}", calendarId);
            return Result.Failure<IList<Event>>(GoogleCalendarErrors.CalendarNotFound);
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.TooManyRequests)
        {
            Logger.LogWarning("Rate limit exceeded");
            return Result.Failure<IList<Event>>(GoogleCalendarErrors.RateLimitExceeded);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected error occurred while fetching events");
            return Result.Failure<IList<Event>>(GoogleCalendarErrors.UnexpectedError);
        }
    }

    public async Task<Result<Event>> UpdateEventAsync(Event eventToUpdate, string calendarId = "primary")
    {
        if (CalendarService == null)
        {
            return Result.Failure<Event>(GoogleCalendarErrors.ServiceNotInitialized);
        }

        try
        {
            var request = CalendarService.Events.Update(eventToUpdate, calendarId, eventToUpdate.Id);
            var updatedEvent = await request.ExecuteAsync();
            return Result.Success(updatedEvent);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to update event");
            return Result.Failure<Event>(GoogleCalendarErrors.EventUpdateFailed);
        }
    }

    public async Task<Result> DeleteEventAsync(string eventId, string calendarId = "primary")
    {
        if (CalendarService == null)
        {
            return Result.Failure(GoogleCalendarErrors.ServiceNotInitialized);
        }

        try
        {
            var request = CalendarService.Events.Delete(calendarId, eventId);
            await request.ExecuteAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to delete event");
            return Result.Failure(GoogleCalendarErrors.EventDeletionFailed);
        }
    }

    public static Event CreateSimpleEvent(string title, string description, DateTime startTime, DateTime endTime,
        string? location = null)
    {
        return new Event
        {
            Summary = title,
            Description = description,
            Location = location,
            Start = new EventDateTime()
            {
                DateTimeDateTimeOffset = startTime,
                TimeZone = TimeZoneInfo.Local.Id,
            },
            End = new EventDateTime()
            {
                DateTimeDateTimeOffset = endTime,
                TimeZone = TimeZoneInfo.Local.Id,
            },
            Reminders = new Event.RemindersData()
            {
                UseDefault = true
            }
        };
    }

    #endregion

    #region Meets

    public async Task<Result<Event>> CreateEventWithMeetAsync(
        MeetingRequest meetingRequest,
        CancellationToken cancellationToken,
        string calendarId = "primary")
    {
        if (CalendarService == null)
        {
            return Result.Failure<Event>(GoogleCalendarErrors.ServiceNotInitialized);
        }

        MeetSettings meetSettings = new MeetSettings();
        try
        {
            var meetEvent = CreateEventDataWithMeet(
                meetingRequest.Title,
                meetingRequest.Description,
                meetingRequest.StartTime,
                meetingRequest.EndTime,
                meetingRequest.Location,
                meetingRequest.AttendeeEmails,
                meetSettings
            );

            var request = CalendarService.Events.Insert(meetEvent, calendarId);
            request.ConferenceDataVersion = 1;
            request.SendNotifications = meetingRequest.SendInvitations;

            var createdEvent = await request.ExecuteAsync(cancellationToken);
            return Result.Success(createdEvent);
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.Unauthorized)
        {
            Logger.LogWarning("Unauthorized access - token may be invalid or expired");
            return Result.Failure<Event>(GoogleCalendarErrors.Unauthorized);
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogWarning("Access forbidden - insufficient permissions");
            return Result.Failure<Event>(GoogleCalendarErrors.Forbidden);
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
        {
            Logger.LogWarning("Calendar not found: {CalendarId}", calendarId);
            return Result.Failure<Event>(GoogleCalendarErrors.CalendarNotFound);
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.TooManyRequests)
        {
            Logger.LogWarning("Rate limit exceeded");
            return Result.Failure<Event>(GoogleCalendarErrors.RateLimitExceeded);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to create meeting event: {Title}", meetingRequest.Title);
            return Result.Failure<Event>(GoogleCalendarErrors.EventCreationFailed);
        }
    }

    public Event CreateEventDataWithMeet(
        string title,
        string description,
        DateTime startTime,
        DateTime endTime,
        string? location = null,
        List<string>? attendeeEmails = null,
        MeetSettings? meetSettings = null)
    {
        meetSettings ??= new MeetSettings();
        var calendarEvent = new Event
        {
            Summary = title,
            Description = description,
            Location = location,
            Start = new EventDateTime()
            {
                DateTimeDateTimeOffset = startTime,
                TimeZone = TimeZoneInfo.Local.Id,
            },
            End = new EventDateTime()
            {
                DateTimeDateTimeOffset = endTime,
                TimeZone = TimeZoneInfo.Local.Id,
            },

            // Google Meet integration
            ConferenceData = new ConferenceData()
            {
                CreateRequest = new CreateConferenceRequest()
                {
                    RequestId = Guid.NewGuid().ToString(), // Unique ID for the request
                    ConferenceSolutionKey = new ConferenceSolutionKey()
                    {
                        Type = "hangoutsMeet" // This creates a Google Meet link
                    }
                }
            },

            Attendees = attendeeEmails?.Select(email => new EventAttendee
            {
                Email = email,
                ResponseStatus = "needsAction"
            }).ToList(),

            // Default reminders
            Reminders = new Event.RemindersData()
            {
                UseDefault = meetSettings.UseDefaultReminders,
                Overrides = meetSettings.CustomReminders?.Select(r => new EventReminder
                {
                    Method = r.Method,
                    Minutes = r.Minutes
                }).ToList()
            },

            // Configure guest permissions based on settings
            GuestsCanSeeOtherGuests = meetSettings.GuestsCanSeeOtherGuests,
            GuestsCanModify = meetSettings.GuestsCanModify,
            GuestsCanInviteOthers = meetSettings.GuestsCanInviteOthers,
        };

        return calendarEvent;
    }

    #endregion
}

public class MeetInfo
{
    public string MeetLink { get; set; }
}