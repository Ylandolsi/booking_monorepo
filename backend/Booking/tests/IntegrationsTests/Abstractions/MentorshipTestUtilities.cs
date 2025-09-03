using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;

namespace IntegrationsTests.Abstractions;

/// <summary>
/// Shared utilities for mentorship-related integration tests.
/// Provides common helper methods to reduce code duplication across test files.
/// </summary>
public static class MentorshipTestUtilities
{
    #region Date and Time Utilities

    /// <summary>
    /// Gets the next occurrence of a specific weekday from today
    /// </summary>
    /// <param name="dayOfWeek">The target day of the week</param>
    /// <returns>The next occurrence of the specified day</returns>
    public static DateTime GetNextWeekday(DayOfWeek dayOfWeek)
    {
        var today = DateTime.UtcNow.Date;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7; // Next week to avoid past dates
        return today.AddDays(daysUntilTarget);
    }

    /// <summary>
    /// Gets the next Monday from today
    /// </summary>
    public static DateTime GetNextMonday()
    {
        return GetNextWeekday(DayOfWeek.Monday);
    }

    /// <summary>
    /// Gets the next Sunday from today
    /// </summary>
    public static DateTime GetNextSunday()
    {
        return GetNextWeekday(DayOfWeek.Sunday);
    }

    /// <summary>
    /// Gets the next occurrence of a specific weekday with a specific time
    /// </summary>
    /// <param name="dayOfWeek">The target day of the week</param>
    /// <param name="time">The specific time for that day</param>
    /// <returns>The next occurrence of the specified day and time</returns>
    public static DateTime GetNextWeekday(DayOfWeek dayOfWeek, TimeOnly time)
    {
        var targetDate = GetNextWeekday(dayOfWeek);
        return targetDate.Add(time.ToTimeSpan());
    }

    #endregion

    #region Mentor and User Utilities

    /// <summary>
    /// Retrieves the slug for the current authenticated mentor
    /// </summary>
    /// <param name="mentorClient">The authenticated mentor's HTTP client</param>
    /// <returns>The mentor's slug</returns>
    public static async Task<string> GetMentorSlug(HttpClient mentorClient)
    {
        var response = await mentorClient.GetAsync(UsersEndpoints.GetCurrentUser);
        response.EnsureSuccessStatusCode();
        
        var userInfo = await response.Content.ReadFromJsonAsync<JsonElement>();
        return userInfo.GetProperty("slug").GetString()!;
    }

    /// <summary>
    /// Retrieves the current user information for an authenticated client
    /// </summary>
    /// <param name="userClient">The authenticated user's HTTP client</param>
    /// <returns>The user information as JsonElement</returns>
    public static async Task<JsonElement> GetCurrentUserInfo(HttpClient userClient)
    {
        var response = await userClient.GetAsync(UsersEndpoints.GetCurrentUser);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    #endregion

    #region Availability Management

    /// <summary>
    /// Sets up mentor availability for a specific day of the week
    /// </summary>
    /// <param name="mentorClient">The authenticated mentor's HTTP client</param>
    /// <param name="dayOfWeek">The day to set availability for</param>
    /// <param name="startTime">Start time in HH:mm format</param>
    /// <param name="endTime">End time in HH:mm format</param>
    public static async Task SetupMentorAvailability(HttpClient mentorClient, DayOfWeek dayOfWeek, string startTime, string endTime)
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

    /// <summary>
    /// Sets up mentor availability for multiple days with the same time range
    /// </summary>
    /// <param name="mentorClient">The authenticated mentor's HTTP client</param>
    /// <param name="daysOfWeek">The days to set availability for</param>
    /// <param name="startTime">Start time in HH:mm format</param>
    /// <param name="endTime">End time in HH:mm format</param>
    public static async Task SetupMentorAvailability(HttpClient mentorClient, DayOfWeek[] daysOfWeek, string startTime, string endTime)
    {
        var dayAvailabilities = daysOfWeek.Select(day => new
        {
            DayOfWeek = day,
            IsActive = true,
            AvailabilityRanges = new[]
            {
                new { StartTime = startTime, EndTime = endTime }
            }
        }).ToArray();

        var availabilityRequest = new
        {
            DayAvailabilities = dayAvailabilities
        };

        var response = await mentorClient.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, availabilityRequest);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Sets up default weekday availability (Monday-Friday, 9 AM - 5 PM)
    /// </summary>
    /// <param name="mentorClient">The authenticated mentor's HTTP client</param>
    public static async Task SetupDefaultWeekdayAvailability(HttpClient mentorClient)
    {
        var weekdays = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        await SetupMentorAvailability(mentorClient, weekdays, "09:00", "17:00");
    }

    /// <summary>
    /// Sets up mentor availability with custom ranges for a specific day
    /// </summary>
    /// <param name="mentorClient">The authenticated mentor's HTTP client</param>
    /// <param name="dayOfWeek">The day to set availability for</param>
    /// <param name="ranges">Array of time ranges (startTime, endTime)</param>
    public static async Task SetupMentorAvailabilityWithRanges(HttpClient mentorClient, DayOfWeek dayOfWeek, (string startTime, string endTime)[] ranges)
    {
        var availabilityRanges = ranges.Select(range => new
        {
            StartTime = range.startTime,
            EndTime = range.endTime
        }).ToArray();

        var availabilityRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = dayOfWeek,
                    IsActive = true,
                    AvailabilityRanges = availabilityRanges
                }
            }
        };

        var response = await mentorClient.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, availabilityRequest);
        response.EnsureSuccessStatusCode();
    }

    #endregion

    #region Session Booking Utilities

    /// <summary>
    /// Books a session and returns the session ID
    /// </summary>
    /// <param name="mentorClient">The mentor's HTTP client (to get mentor slug)</param>
    /// <param name="menteeClient">The mentee's HTTP client (to book the session)</param>
    /// <param name="dayOfWeek">The day to book</param>
    /// <param name="startTime">Start time in HH:mm format</param>
    /// <param name="endTime">End time in HH:mm format</param>
    /// <param name="timeZoneId">Timezone for the session (default: Africa/Tunis)</param>
    /// <param name="note">Optional note for the session</param>
    /// <returns>The session ID of the booked session</returns>
    public static async Task<int> BookValidSession(
        HttpClient mentorClient, 
        HttpClient menteeClient, 
        DayOfWeek dayOfWeek, 
        string startTime, 
        string endTime, 
        string timeZoneId = "Africa/Tunis", 
        string? note = null)
    {
        var mentorSlug = await GetMentorSlug(mentorClient);
        var targetDate = GetNextWeekday(dayOfWeek);

        // Store initial session count to identify the new session
        var initialSessionsResponse = await menteeClient.GetAsync(MentorshipEndpoints.Sessions.GetSessions);
        initialSessionsResponse.EnsureSuccessStatusCode();
        var initialSessions = await initialSessionsResponse.Content.ReadFromJsonAsync<JsonElement>();
        var initialCount = initialSessions.EnumerateArray().Count();

        var bookingRequest = new
        {
            MentorSlug = mentorSlug,
            Date = targetDate.ToString("yyyy-MM-dd"),
            StartTime = startTime,
            EndTime = endTime,
            TimeZoneId = timeZoneId,
            Note = note ?? "Test session"
        };

        var response = await menteeClient.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        // Verify we got a payUrl (indicating successful booking)
        if (!result.TryGetProperty("payUrl", out var payUrl) || string.IsNullOrEmpty(payUrl.GetString()))
        {
            throw new InvalidOperationException("Session booking did not return a valid payUrl");
        }

        // Get the newly created session by finding the session that wasn't there before
        var newSessionsResponse = await menteeClient.GetAsync(MentorshipEndpoints.Sessions.GetSessions);
        newSessionsResponse.EnsureSuccessStatusCode();
        var newSessions = await newSessionsResponse.Content.ReadFromJsonAsync<JsonElement>();
        var newSessionsArray = newSessions.EnumerateArray().ToList();
        
        // Should have one more session now
        if (newSessionsArray.Count != initialCount + 1)
        {
            throw new InvalidOperationException($"Expected {initialCount + 1} sessions after booking, but found {newSessionsArray.Count}");
        }
        
        // Find the newest session (should be the one with the highest ID)
        var newSession = newSessionsArray
            .OrderByDescending(s => s.GetProperty("id").GetInt32())
            .FirstOrDefault();
            
        if (newSession.ValueKind == JsonValueKind.Undefined)
        {
            throw new InvalidOperationException("Could not find the newly created session");
        }
        
        return newSession.GetProperty("id").GetInt32();
    }

    /// <summary>
    /// Creates a basic booking request object
    /// </summary>
    /// <param name="mentorSlug">The mentor's slug</param>
    /// <param name="date">The date for the session</param>
    /// <param name="startTime">Start time in HH:mm format</param>
    /// <param name="endTime">End time in HH:mm format</param>
    /// <param name="timeZoneId">Timezone for the session</param>
    /// <param name="note">Optional note for the session</param>
    /// <returns>An anonymous object representing the booking request</returns>
    public static object CreateBookingRequest(
        string mentorSlug, 
        string date, 
        string startTime, 
        string endTime, 
        string timeZoneId = "Africa/Tunis", 
        string? note = null)
    {
        var request = new
        {
            MentorSlug = mentorSlug,
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            TimeZoneId = timeZoneId
        };

        // If note is provided, create an object that includes it
        if (!string.IsNullOrEmpty(note))
        {
            return new
            {
                MentorSlug = mentorSlug,
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                TimeZoneId = timeZoneId,
                Note = note
            };
        }

        return request;
    }

    #endregion

    #region Bulk Operations

    /// <summary>
    /// Creates multiple mentors with default settings
    /// </summary>
    /// <param name="testBase">The test base instance for creating mentors</param>
    /// <param name="count">Number of mentors to create</param>
    /// <param name="basePrefix">Base prefix for mentor usernames</param>
    /// <param name="hourlyRate">Hourly rate for mentors</param>
    /// <param name="bufferTimeMinutes">Buffer time in minutes</param>
    /// <returns>List of mentor client tuples</returns>
    public static async Task<List<(HttpClient arrange, HttpClient act)>> CreateMultipleMentors(
        MentorshipTestBase testBase, 
        int count, 
        string basePrefix = "mentor", 
        decimal hourlyRate = 75.0m, 
        int bufferTimeMinutes = 15)
    {
        var mentors = new List<(HttpClient arrange, HttpClient act)>();
        
        for (int i = 0; i < count; i++)
        {
            var mentor = await testBase.CreateMentor($"{basePrefix}_{i}", hourlyRate, bufferTimeMinutes);
            mentors.Add(mentor);
        }
        
        return mentors;
    }

    /// <summary>
    /// Creates multiple mentees
    /// </summary>
    /// <param name="testBase">The test base instance for creating mentees</param>
    /// <param name="count">Number of mentees to create</param>
    /// <param name="basePrefix">Base prefix for mentee usernames</param>
    /// <returns>List of mentee client tuples</returns>
    public static async Task<List<(HttpClient arrange, HttpClient act)>> CreateMultipleMentees(
        MentorshipTestBase testBase, 
        int count, 
        string basePrefix = "mentee")
    {
        var mentees = new List<(HttpClient arrange, HttpClient act)>();
        
        for (int i = 0; i < count; i++)
        {
            var mentee = await testBase.CreateMentee($"{basePrefix}_{i}");
            mentees.Add(mentee);
        }
        
        return mentees;
    }

    #endregion

    #region Test Data Generation

    /// <summary>
    /// Generates test data for availability with mixed active/inactive days
    /// </summary>
    /// <returns>An object representing bulk availability data</returns>
    public static object CreateMixedAvailabilityData()
    {
        return new
        {
            DayAvailabilities = new object[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Monday, IsActive = true,
                    AvailabilityRanges = new[] { new { StartTime = "09:00", EndTime = "17:00" } }
                },
                new
                {
                    DayOfWeek = DayOfWeek.Wednesday, IsActive = true,
                    AvailabilityRanges = new[] { new { StartTime = "10:00", EndTime = "16:00" } }
                },
                new
                {
                    DayOfWeek = DayOfWeek.Thursday, IsActive = true,
                    AvailabilityRanges = new[] { new { StartTime = "08:00", EndTime = "14:00" } }
                },
                new
                {
                    DayOfWeek = DayOfWeek.Friday, IsActive = false,
                    AvailabilityRanges = Array.Empty<object>()
                }
            }
        };
    }

    /// <summary>
    /// Creates invalid booking request data for testing error scenarios
    /// </summary>
    /// <param name="mentorSlug">The mentor slug to use</param>
    /// <returns>Array of invalid booking requests</returns>
    public static object[] CreateInvalidBookingRequests(string mentorSlug)
    {
        return new object[]
        {
            // Invalid date formats
            new { MentorSlug = mentorSlug, Date = "2025-13-45", StartTime = "10:00", EndTime = "11:00", TimeZoneId = "UTC" },
            new { MentorSlug = mentorSlug, Date = "invalid-date", StartTime = "10:00", EndTime = "11:00", TimeZoneId = "UTC" },
            
            // Invalid time formats
            new { MentorSlug = mentorSlug, Date = "2025-12-25", StartTime = "25:00", EndTime = "26:00", TimeZoneId = "UTC" },
            new { MentorSlug = mentorSlug, Date = "2025-12-25", StartTime = "invalid", EndTime = "11:00", TimeZoneId = "UTC" },
            
            // Invalid timezones
            new { MentorSlug = mentorSlug, Date = "2025-12-25", StartTime = "10:00", EndTime = "11:00", TimeZoneId = "Invalid/Zone" },
            new { MentorSlug = mentorSlug, Date = "2025-12-25", StartTime = "10:00", EndTime = "11:00", TimeZoneId = "" },
            
            // Missing required fields
            new { MentorSlug = "", Date = "2025-12-25", StartTime = "10:00", EndTime = "11:00", TimeZoneId = "UTC" },
            new { MentorSlug = mentorSlug, Date = "", StartTime = "10:00", EndTime = "11:00", TimeZoneId = "UTC" },
        };
    }

    #endregion

    #region Assertion Helpers

    /// <summary>
    /// Verifies that a response contains a valid session booking result
    /// </summary>
    /// <param name="response">The HTTP response from booking</param>
    /// <returns>The parsed JSON element</returns>
    public static async Task<JsonElement> VerifySuccessfulBooking(HttpResponseMessage response)
    {
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Expected OK response but got {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        if (!result.TryGetProperty("payUrl", out var payUrl) || string.IsNullOrEmpty(payUrl.GetString()))
        {
            throw new InvalidOperationException("Successful booking should contain a payUrl");
        }

        return result;
    }

    /// <summary>
    /// Verifies that a sessions list response contains expected data
    /// </summary>
    /// <param name="response">The HTTP response</param>
    /// <param name="expectedMinCount">Minimum expected number of sessions</param>
    /// <returns>The sessions array as a list</returns>
    public static async Task<List<JsonElement>> VerifySessionsList(HttpResponseMessage response, int expectedMinCount = 0)
    {
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Expected OK response but got {response.StatusCode}");
        }

        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        if (sessions.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Sessions response should be an array");
        }

        var sessionsList = sessions.EnumerateArray().ToList();
        
        if (sessionsList.Count < expectedMinCount)
        {
            throw new InvalidOperationException($"Expected at least {expectedMinCount} sessions but got {sessionsList.Count}");
        }

        return sessionsList;
    }

    #endregion

    #region Constants

    /// <summary>
    /// Default timezone used in tests
    /// </summary>
    public const string DefaultTimeZone = "Africa/Tunis";

    /// <summary>
    /// Common timezone IDs for testing
    /// </summary>
    public static class TimeZones
    {
        public const string Tunisia = "Africa/Tunis";
        public const string UTC = "UTC";
        public const string Tokyo = "Asia/Tokyo";
        public const string LosAngeles = "America/Los_Angeles";
        public const string Paris = "Europe/Paris";
        public const string NewYork = "America/New_York";
    }

    /// <summary>
    /// Common time formats used in tests
    /// </summary>
    public static class TimeFormats
    {
        public const string Morning9AM = "09:00";
        public const string Morning10AM = "10:00";
        public const string Morning11AM = "11:00";
        public const string Noon = "12:00";
        public const string Afternoon1PM = "13:00";
        public const string Afternoon2PM = "14:00";
        public const string Afternoon3PM = "15:00";
        public const string Afternoon4PM = "16:00";
        public const string Afternoon5PM = "17:00";
        public const string Evening6PM = "18:00";
    }

    #endregion
}