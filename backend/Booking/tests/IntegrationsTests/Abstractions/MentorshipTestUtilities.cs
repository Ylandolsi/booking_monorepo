using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using Booking.Modules.Mentorships.Domain.Enums;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Persistence;
using Booking.Modules.Mentorships.BackgroundJobs.Escrow;
using IntegrationsTests.Abstractions.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Xunit;

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
    /// Retrieves the slug for the current authenticated user
    /// </summary>
    /// <param name="userClient">The authenticated user's HTTP client</param>
    /// <returns>The user's slug</returns>
    public static async Task<string> GetUserSlug(HttpClient userClient)
    {
        var response = await userClient.GetAsync(UsersEndpoints.GetCurrentUser);
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
    public static async Task SetupMentorAvailability(HttpClient mentorClient, DayOfWeek dayOfWeek, string startTime,
        string endTime)
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

        var response =
            await mentorClient.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, availabilityRequest);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Sets up mentor availability for multiple days with the same time range
    /// </summary>
    /// <param name="mentorClient">The authenticated mentor's HTTP client</param>
    /// <param name="daysOfWeek">The days to set availability for</param>
    /// <param name="startTime">Start time in HH:mm format</param>
    /// <param name="endTime">End time in HH:mm format</param>
    public static async Task SetupMentorAvailability(HttpClient mentorClient, DayOfWeek[] daysOfWeek, string startTime,
        string endTime)
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

        var response =
            await mentorClient.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, availabilityRequest);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Sets up default weekday availability (Monday-Friday, 9 AM - 5 PM)
    /// </summary>
    /// <param name="mentorClient">The authenticated mentor's HTTP client</param>
    public static async Task SetupDefaultWeekdayAvailability(HttpClient mentorClient)
    {
        var weekdays = new[]
            { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        await SetupMentorAvailability(mentorClient, weekdays, "09:00", "17:00");
    }

    /// <summary>
    /// Sets up mentor availability with custom ranges for a specific day
    /// </summary>
    /// <param name="mentorClient">The authenticated mentor's HTTP client</param>
    /// <param name="dayOfWeek">The day to set availability for</param>
    /// <param name="ranges">Array of time ranges (startTime, endTime)</param>
    public static async Task SetupMentorAvailabilityWithRanges(HttpClient mentorClient, DayOfWeek dayOfWeek,
        (string startTime, string endTime)[] ranges)
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

        var response =
            await mentorClient.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, availabilityRequest);
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
        var mentorSlug = await GetUserSlug(mentorClient);
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
            throw new InvalidOperationException(
                $"Expected {initialCount + 1} sessions after booking, but found {newSessionsArray.Count}");
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

    public static async Task<int> GetLatestSessionId(HttpClient client)
    {
        var response = await client.GetAsync(MentorshipEndpoints.Sessions.GetSessions);
        response.EnsureSuccessStatusCode();

        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        var sessionsArray = sessions.EnumerateArray().ToList();

        if (sessionsArray.Count == 0)
            throw new InvalidOperationException("No sessions found");

        // Get the most recent session (assuming they're ordered by creation time)
        var latestSession = sessionsArray.First();
        return latestSession.GetProperty("id").GetInt32();
    }

    public static async Task VerifySessionStatus(IntegrationTestsWebAppFactory factory, int sessionId,
        SessionStatus expectedStatus)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var session = await dbContext.Sessions.FindAsync(sessionId);
        Assert.NotNull(session);
        Assert.Equal(expectedStatus, session.Status);
    }

    public static async Task VerifySessionHasMeetingLink(IntegrationTestsWebAppFactory factory, int sessionId)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var session = await dbContext.Sessions.FindAsync(sessionId);
        Assert.NotNull(session);
        Assert.False(string.IsNullOrEmpty(session.GoogleMeetLink?.Value));
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
            new
            {
                MentorSlug = mentorSlug, Date = "2025-13-45", StartTime = "10:00", EndTime = "11:00", TimeZoneId = "UTC"
            },
            new
            {
                MentorSlug = mentorSlug, Date = "invalid-date", StartTime = "10:00", EndTime = "11:00",
                TimeZoneId = "UTC"
            },

            // Invalid time formats
            new
            {
                MentorSlug = mentorSlug, Date = "2025-12-25", StartTime = "25:00", EndTime = "26:00", TimeZoneId = "UTC"
            },
            new
            {
                MentorSlug = mentorSlug, Date = "2025-12-25", StartTime = "invalid", EndTime = "11:00",
                TimeZoneId = "UTC"
            },

            // Invalid timezones
            new
            {
                MentorSlug = mentorSlug, Date = "2025-12-25", StartTime = "10:00", EndTime = "11:00",
                TimeZoneId = "Invalid/Zone"
            },
            new
            {
                MentorSlug = mentorSlug, Date = "2025-12-25", StartTime = "10:00", EndTime = "11:00", TimeZoneId = ""
            },

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
    public static async Task<List<JsonElement>> VerifySessionsList(HttpResponseMessage response,
        int expectedMinCount = 0)
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
            throw new InvalidOperationException(
                $"Expected at least {expectedMinCount} sessions but got {sessionsList.Count}");
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

    #region Payment

    public static async Task<dynamic> CompletePaymentViaMockKonnect(string paymentRef, HttpClient client)
    {
        var paymentRequest = new { paymentMethod = "card" };

        var response = await client.PostAsJsonAsync($"process-payment/{paymentRef}", paymentRequest);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();

        return new
        {
            success = result.TryGetProperty("success", out var success) && success.GetBoolean(),
            error = result.TryGetProperty("error", out var error) ? error.GetString() : null
        };
    }

    public static async Task<dynamic> CompletePaymentWithWallet(string paymentRef, string walletId, HttpClient client)
    {
        var paymentRequest = new { paymentMethod = "wallet", walletId = walletId };

        var response = await client.PostAsJsonAsync($"process-payment/{paymentRef}", paymentRequest);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();

        return new
        {
            success = result.TryGetProperty("success", out var success) && success.GetBoolean(),
            error = result.TryGetProperty("error", out var error) ? error.GetString() : null
        };
    }

    public static async Task<dynamic> SimulatePaymentFailure(string paymentRef, HttpClient client)
    {
        var paymentRequest = new { paymentMethod = "fail" };

        var response = await client.PostAsJsonAsync($"process-payment/{paymentRef}", paymentRequest);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();

        return new
        {
            success = result.TryGetProperty("success", out var success) && success.GetBoolean(),
            error = result.TryGetProperty("error", out var error) ? error.GetString() : null
        };
    }

    public static async Task ChargeTestWallet(string walletId, int amount, HttpClient client)
    {
        var chargeRequest = new { Amount = amount };

        await client.PostAsJsonAsync($"wallets/{walletId}/charge", chargeRequest);
    }

    public static async Task<dynamic> GetPaymentDetails(string paymentRef, HttpClient client)
    {
        var response = await client.GetAsync($"payments/{paymentRef}");
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();

        return new
        {
            status = result.TryGetProperty("status", out var status) ? status.GetString() : null,
            amount = result.TryGetProperty("amount", out var amount) ? amount.GetInt32() : 0
        };
    }

    /// <summary>
    /// Extracts payment reference from payment URL
    /// </summary>
    /// <param name="payUrl">The payment URL</param>
    /// <returns>Payment reference string</returns>
    public static string ExtractPaymentRefFromUrl(string payUrl)
    {
        // Extract payment reference from URL like: https://localhostpay/PAY_abc123
        var parts = payUrl.Split('/');
        return parts.LastOrDefault() ?? string.Empty;
    }

    #endregion

    #region Escrow and Payout Utilities

    /// <summary>
    /// Books a session and completes the full flow including payment
    /// </summary>
    /// <param name="mentorClient">The mentor's HTTP client</param>
    /// <param name="menteeClient">The mentee's HTTP client</param>
    /// <param name="targetDate">The date for the session</param>
    /// <param name="startTime">Start time in HH:mm format</param>
    /// <param name="endTime">End time in HH:mm format</param>
    /// <param name="timeZoneId">Timezone for the session</param>
    /// <returns>The session ID of the completed session</returns>
    public static async Task<int> BookAndCompleteSession(
        HttpClient mentorClient,
        HttpClient menteeClient,
        DateTime targetDate,
        string startTime = "10:00",
        string endTime = "11:00",
        string timeZoneId = DefaultTimeZone)
    {
        var mentorSlug = await GetUserSlug(mentorClient);

        var bookingRequest = CreateBookingRequest(
            mentorSlug,
            targetDate.ToString("yyyy-MM-dd"),
            startTime,
            endTime,
            timeZoneId,
            "Test session for escrow flow"
        );

        // Book the session
        var response = await menteeClient.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(result.GetProperty("payUrl").GetString()!);

        await CompletePaymentViaMockKonnect(paymentRef, menteeClient);

        // Wait for webhook processing
        await Task.Delay(2000);

        // Get the session ID
        var sessionId = await GetLatestSessionId(menteeClient);

        return sessionId;
    }


    /// <summary>
    /// Marks a session as completed by both mentor and mentee
    /// </summary>
    /// <param name="sessionId">The session ID</param>
    /// <param name="mentorClient">Mentor's HTTP client</param>
    /// <param name="menteeClient">Mentee's HTTP client</param>
    public static async Task MarkSessionAsCompleted(int sessionId, HttpClient mentorClient, HttpClient menteeClient)
    {
        // Note: These endpoints might not exist yet - placeholder for when session completion is implemented
        // For now, we'll simulate completion by directly updating the database
        // await mentorClient.PostAsJsonAsync($"/mentorships/sessions/{sessionId}/complete", new { });
        // await menteeClient.PostAsJsonAsync($"/mentorships/sessions/{sessionId}/complete", new { });

        // TODO: Replace with actual endpoint calls when implemented
        await Task.CompletedTask;
    }

    /// <summary>
    /// Creates an admin authentication client
    /// </summary>
    /// <param name="factory">The test factory</param>
    /// <returns>Authenticated admin HTTP client</returns>
    public static async Task<HttpClient> CreateAdminClient(IntegrationTestsWebAppFactory factory)
    {
        var client = factory.CreateClient();

        // Login as admin
        var loginRequest = new
        {
            Email = "ylandol66@gmail.com",
            Password = "Password123!"
        };

        var response = await client.PostAsJsonAsync(UsersEndpoints.Login, loginRequest);
        response.EnsureSuccessStatusCode();

        return client;
    }

    /// <summary>
    /// Gets the mentor ID from the HTTP client
    /// </summary>
    /// <param name="mentorClient">The mentor's HTTP client</param>
    /// <returns>Mentor ID</returns>
    public static async Task<string> GetMentorId(HttpClient mentorClient)
    {
        var userInfo = await GetCurrentUserInfo(mentorClient);
        return userInfo.GetProperty("id").GetString()!;
    }

    /// <summary>
    /// Creates a payout request for a mentor
    /// </summary>
    /// <param name="mentorClient">Mentor's HTTP client</param>
    /// <param name="amount">Amount to request payout for</param>
    /// <returns>Payout request response</returns>
    public static async Task<HttpResponseMessage> RequestPayout(HttpClient mentorClient, decimal amount)
    {
        // integrate with konenct if not integrated 

        await mentorClient.PostAsJsonAsync(UsersEndpoints.InegrateKonnect, new
        {
            KonnectWalletId = "connect-with-konnect",
        });


        var payoutRequest = new
        {
            Amount = amount
        };

        return await mentorClient.PostAsJsonAsync(MentorshipEndpoints.Payment.Payout, payoutRequest);
    }

    /// <summary>
    /// Links a Konnect wallet to a mentor account
    /// </summary>
    /// <param name="mentorClient">Mentor's HTTP client</param>
    /// <param name="walletId">Konnect wallet ID</param>
    /// <returns>Response from wallet linking</returns>
    public static async Task<HttpResponseMessage> LinkKonnectWallet(HttpClient mentorClient, string walletId)
    {
        var linkRequest = new
        {
            KonnectWalletId = walletId
        };

        return await mentorClient.PostAsJsonAsync(UsersEndpoints.InegrateKonnect, linkRequest);
    }

    #endregion

    #region Database Verification Utilities

    /// <summary>
    /// Verifies that an escrow record is created for a session
    /// </summary>
    /// <param name="factory">Test factory for database access</param>
    /// <param name="sessionId">Session ID</param>
    /// <param name="expectedAmount">Expected escrow amount</param>
    public static async Task VerifyEscrowCreated(IntegrationTestsWebAppFactory factory, int sessionId,
        decimal expectedAmount)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.NotNull(escrow);
        Assert.Equal(expectedAmount, escrow.Price);
    }

    /// <summary>
    /// Verifies that an escrow is released after session completion
    /// </summary>
    /// <param name="factory">Test factory for database access</param>
    /// <param name="sessionId">Session ID</param>
    public static async Task VerifyEscrowReleased(IntegrationTestsWebAppFactory factory, int sessionId)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.NotNull(escrow);
        Assert.Equal(EscrowState.Released, escrow.State);
    }

    /// <summary>
    /// Verifies that a payout request is created
    /// </summary>
    /// <param name="factory">Test factory for database access</param>
    /// <param name="mentorId">Mentor ID</param>
    /// <param name="expectedAmount">Expected payout amount</param>
    public static async Task VerifyPayoutRequestCreated(IntegrationTestsWebAppFactory factory, string mentorId,
        decimal expectedAmount)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var payout = await dbContext.Payouts
            .Where(p => p.UserId.ToString() == mentorId && p.Amount == expectedAmount)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync();

        Assert.NotNull(payout);
        Assert.Equal(PayoutStatus.Pending, payout.Status);
    }


    /// <summary>
    /// Gets yser balance from database
    /// </summary>
    /// <param name="factory">Test factory for database access</param>
    /// <param name="userId">User ID</param>
    /// <returns>Current mentor balance</returns>
    public static async Task<decimal> GetUserBalance(IntegrationTestsWebAppFactory factory, string userId)
    {
        var wallet = await GetUserWallet(factory, userId);
        return wallet.Balance;
    }

    public static async Task<Wallet> GetUserWallet(IntegrationTestsWebAppFactory factory, string userId)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var wallet = await dbContext.Wallets.FirstOrDefaultAsync(m => m.UserId.ToString() == userId);
        return wallet;
    }


    public static async Task VerifyNoEscrowCreated(IntegrationTestsWebAppFactory factory, int sessionId)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.Null(escrow?.Price);
    }

    public static async Task<decimal> GetEscrowAmount(IntegrationTestsWebAppFactory factory, int sessionId)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        return escrow?.Price ?? 0;
    }

    #endregion

    #region Background Job Utilities

    /// <summary>
    /// Triggers the escrow processing background job
    /// </summary>
    /// <param name="factory">Test factory for background job access</param>
    public static async Task TriggerEscrowJob(IntegrationTestsWebAppFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var backgroundJobClient = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();

        // Trigger the escrow processing job
        backgroundJobClient.Enqueue<EscrowJob>(job => job.ExecuteAsync(null));

        // Wait for job processing
        await Task.Delay(4000);
    }

    /// <summary>
    /// Simulates time passage for escrow release (24-hour rule)
    /// </summary>
    /// <param name="factory">Test factory for database access</param>
    /// <param name="sessionId">Session ID</param>
    public static async Task SimulateEscrowTimePassage(IntegrationTestsWebAppFactory factory, int sessionId)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var session = await dbContext.Sessions.FindAsync(sessionId);
        if (session != null)
        {
            // Note: CompletedAt property and MarkAsCompleted method might not exist yet
            // This is a placeholder for when session completion tracking is implemented
            // session.MarkAsCompleted();
            await dbContext.SaveChangesAsync();
        }
    }

    #endregion
}