using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Sessions;

public class SessionRetrievalTests : MentorshipTestBase
{
    public SessionRetrievalTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }
    #region Get Sessions List Tests

    [Fact]
    public async Task GetSessions_ShouldReturnMenteeSessions_WhenMenteeRequests()
    {
        // Arrange
        var (mentor1Arrange, mentor1Act) = await CreateMentor("mentor1_list");
        var (mentor2Arrange, mentor2Act) = await CreateMentor("mentor2_list");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_list");

        await SetupMentorAvailability(mentor1Arrange, DayOfWeek.Monday, "09:00", "17:00");
        await SetupMentorAvailability(mentor2Arrange, DayOfWeek.Tuesday, "09:00", "17:00");

        // Book multiple sessions
        var session1Id = await BookValidSession(mentor1Arrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");
        var session2Id = await BookValidSession(mentor2Arrange, menteeAct, DayOfWeek.Tuesday, "14:00", "15:00");

        // Act
        var response = await menteeAct.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        Assert.True(sessions.ValueKind == JsonValueKind.Array);
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.True(sessionArray.Count >= 2);
        
        var sessionIds = sessionArray.Select(s => s.GetProperty("id").GetInt32()).ToList();
        Assert.Contains(session1Id, sessionIds);
        Assert.Contains(session2Id, sessionIds);
    }

    [Fact]
    public async Task GetSessions_ShouldReturnMentorSessions_WhenMentorRequests()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_list_own");
        var (mentor2Arrange, mentor2Act) = await CreateMentor("mentor2");
        var (mentee1Arrange, mentee1Act) = await CreateMentee("mentee1_list_mentor");
        var (mentee2Arrange, mentee2Act) = await CreateMentee("mentee2_list_mentor");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        await SetupMentorAvailability(mentor2Arrange, DayOfWeek.Monday, "09:00", "17:00");

        // Book sessions with different mentees
        var session1Id = await BookValidSession(mentorArrange, mentee1Act, DayOfWeek.Monday, "10:00", "11:00");
        var session2Id = await BookValidSession(mentorArrange, mentee2Act, DayOfWeek.Monday, "12:00", "13:00");
        
        var session3Id = await BookValidSession(mentor2Arrange , mentorAct ,  DayOfWeek.Monday, "14:00", "15:00");
        
        
        var response = await mentorAct.GetAsync($"{MentorshipEndpoints.Sessions.GetSessions}");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        Assert.True(sessions.ValueKind == JsonValueKind.Array);
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.True(sessionArray.Count == 3);
        
        
        Assert.Equal(2, sessionArray.Count(s => s.GetProperty("iamMentor").GetBoolean()));
        
        var sessionIds = sessionArray.Select(s => s.GetProperty("id").GetInt32()).ToList();
        Assert.Contains(session1Id, sessionIds);
        Assert.Contains(session2Id, sessionIds);
        Assert.Contains(session3Id, sessionIds);
    }

    [Fact]
    public async Task GetSessions_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();

        // Act
        var response = await unauthClient.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSessions_ShouldReturnEmptyList_WhenNoSessions()
    {
        // Arrange
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_empty_list");

        // Act
        var response = await menteeAct.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        Assert.True(sessions.ValueKind == JsonValueKind.Array);
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.Empty(sessionArray);
    }

    [Fact]
    public async Task GetSessions_ShouldFilterByDateRange_WhenParameterProvided()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_filter_date");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_filter_date");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        await SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "09:00", "17:00");

        // Book sessions on different days
        var session1Id = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");
        var session2Id = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Tuesday, "10:00", "11:00");

        // Act - request sessions for next 7 days
        var response = await menteeAct.GetAsync($"{MentorshipEndpoints.Sessions.GetSessions}?daysFromNow=7");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        Assert.True(sessions.ValueKind == JsonValueKind.Array);
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.True(sessionArray.Count >= 2);
    }

    [Fact]
    public async Task GetSessions_ShouldFilterByTimezone_WhenParameterProvided()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_filter_timezone");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_filter_timezone");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        // Book session in specific timezone
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00", "Europe/Paris");

        // Act - request sessions with timezone parameter
        var response = await menteeAct.GetAsync($"{MentorshipEndpoints.Sessions.GetSessions}?timeZoneId=Europe/Paris&daysFromNow=7");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        Assert.True(sessions.ValueKind == JsonValueKind.Array);
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.True(sessionArray.Count >= 1);
        
        var sessionIds = sessionArray.Select(s => s.GetProperty("id").GetInt32()).ToList();
        Assert.Contains(sessionId, sessionIds);
    }

    [Fact]
    public async Task GetSessions_ShouldSortByScheduledTime()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_sort_time");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_sort_time");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        await SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "09:00", "17:00");

        // Book sessions in different order (not chronological)
        var laterSessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Tuesday, "14:00", "15:00");
        var earlierSessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");

        // Act
        var response = await menteeAct.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        var sessionArray = sessions.EnumerateArray().ToList();
        Assert.True(sessionArray.Count >= 2);

        // Verify sessions are sorted by scheduled time
        for (int i = 0; i < sessionArray.Count - 1; i++)
        {
            var currentScheduledAt = DateTime.Parse(sessionArray[i].GetProperty("scheduledAt").GetString()!);
            var nextScheduledAt = DateTime.Parse(sessionArray[i + 1].GetProperty("scheduledAt").GetString()!);
            Assert.True(currentScheduledAt <= nextScheduledAt);
        }
    }

    [Fact]
    public async Task GetSessions_ShouldIncludeTimezoneInformation()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_include_timezone");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_include_timezone");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00", "Europe/Paris");

        // Act
        var response = await menteeAct.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        var sessionArray = sessions.EnumerateArray().ToList();
        var bookedSession = sessionArray.FirstOrDefault(s => s.GetProperty("id").GetInt32() == sessionId);
        Assert.True(bookedSession.ValueKind != JsonValueKind.Undefined);
        
        // Verify timezone-related fields are present
        Assert.True(bookedSession.TryGetProperty("scheduledAt", out _));
        Assert.True(bookedSession.TryGetProperty("durationInMinutes", out _));
    }

    [Fact]
    public async Task GetSessions_ShouldShowBookedStatus_ForNewSession()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_status_booked");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_status_booked");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var sessionId = await BookValidSession(mentorArrange, menteeAct, DayOfWeek.Monday, "10:00", "11:00");

        // Act
        var response = await menteeAct.GetAsync(MentorshipEndpoints.Sessions.GetSessions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        var sessionArray = sessions.EnumerateArray().ToList();
        var bookedSession = sessionArray.FirstOrDefault(s => s.GetProperty("id").GetInt32() == sessionId);
        Assert.True(bookedSession.ValueKind != JsonValueKind.Undefined);
        
        // Verify session is in Booked status
        var status = bookedSession.GetProperty("status").GetInt32();
        Assert.Equal(1, status);
        
    }

    #endregion

    #region Helper Methods

    private async Task SetupMentorAvailability(HttpClient mentorClient, DayOfWeek dayOfWeek, string startTime, string endTime)
    {
        var availabilityRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = dayOfWeek,
                    IsActive = true,
                    AvailabilityRanges = new[]
                    {
                        new { StartTime = startTime, EndTime = endTime }
                    }
                }
            }
        };

        var response = await mentorClient.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, availabilityRequest);
        response.EnsureSuccessStatusCode();
    }

    private async Task<string> GetMentorSlug(HttpClient mentorClient)
    {
        var response = await mentorClient.GetAsync(UsersEndpoints.GetCurrentUser);
        response.EnsureSuccessStatusCode();
        
        var userInfo = await response.Content.ReadFromJsonAsync<JsonElement>();
        return userInfo.GetProperty("slug").GetString()!;
    }

    private async Task<int> BookValidSession(HttpClient mentorClient, HttpClient menteeClient, DayOfWeek dayOfWeek, 
        string startTime, string endTime, string timeZoneId = "Africa/Tunis")
    {
        var targetDate = GetNextWeekday(dayOfWeek);
        var mentorSlug = await GetMentorSlug(mentorClient);

        var bookingRequest = new
        {
            MentorSlug = mentorSlug,
            Date = targetDate.ToString("yyyy-MM-dd"),
            StartTime = startTime,
            EndTime = endTime,
            TimeZoneId = timeZoneId,
            Note = "Test session for retrieval"
        };

        var response = await menteeClient.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("sessionId").GetInt32();
    }

    private static DateTime GetNextWeekday(DayOfWeek dayOfWeek)
    {
        var today = DateTime.UtcNow.Date;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7; // Next week to avoid past dates
        return today.AddDays(daysUntilTarget);
    }

    #endregion
}
