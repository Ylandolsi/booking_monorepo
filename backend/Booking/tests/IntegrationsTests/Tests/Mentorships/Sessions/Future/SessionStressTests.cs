/*
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Sessions;

public class SessionStressTests : MentorshipTestBase
{
    public SessionStressTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Concurrent Booking Tests

    [Fact]
    public async Task BookSession_ShouldHandleConcurrentBookingAttempts_OnSameSlot()
    {
        // Arrange - Test race condition when multiple users try to book the same slot
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_concurrent");
        var mentees = new List<(HttpClient arrange, HttpClient act)>();
        
        // Create 10 mentees
        for (int i = 0; i < 10; i++)
        {
            mentees.Add(await CreateMentee($"mentee_concurrent_{i}"));
        }

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var mentorSlug = await GetMentorSlug(mentorArrange);
        var nextMonday = GetNextWeekday(DayOfWeek.Monday);

        var bookingRequest = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis",
            Note = "Concurrent booking test"
        };

        var successCount = 0;
        var conflictCount = 0;
        var results = new ConcurrentBag<(int menteeIndex, HttpStatusCode statusCode)>();

        // Act - All mentees try to book the same slot simultaneously
        var tasks = mentees.Select(async (mentee, index) =>
        {
            try
            {
                var response = await mentee.act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
                results.Add((index, response.StatusCode));
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Interlocked.Increment(ref successCount);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Interlocked.Increment(ref conflictCount);
                }
            }
            catch (Exception ex)
            {
                // Log exception but don't fail the test
                Console.WriteLine($"Mentee {index} failed with exception: {ex.Message}");
            }
        }).ToArray();

        await Task.WhenAll(tasks);

        // Assert - Only one booking should succeed, others should fail with conflict
        Assert.Equal(1, successCount);
        Assert.True(conflictCount >= 8); // At least 8 should fail due to conflict
        Assert.Equal(10, results.Count); // All requests should complete
    }

    [Fact]
    public async Task BookSession_ShouldHandleConcurrentBookings_DifferentSlots()
    {
        // Arrange - Test concurrent bookings for different time slots
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_diff_slots");
        var mentees = new List<(HttpClient arrange, HttpClient act)>();
        
        // Create 5 mentees
        for (int i = 0; i < 5; i++)
        {
            mentees.Add(await CreateMentee($"mentee_diff_slots_{i}"));
        }

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var mentorSlug = await GetMentorSlug(mentorArrange);
        var nextMonday = GetNextWeekday(DayOfWeek.Monday);

        var successCount = 0;
        var results = new ConcurrentBag<(int menteeIndex, HttpStatusCode statusCode)>();

        // Act - Each mentee books a different time slot
        var tasks = mentees.Select(async (mentee, index) =>
        {
            var startHour = 10 + index; // 10:00, 11:00, 12:00, 13:00, 14:00
            var bookingRequest = new
            {
                MentorSlug = mentorSlug,
                Date = nextMonday.ToString("yyyy-MM-dd"),
                StartTime = $"{startHour:D2}:00",
                EndTime = $"{startHour + 1:D2}:00",
                TimeZoneId = "Africa/Tunis",
                Note = $"Concurrent booking {index}"
            };

            try
            {
                var response = await mentee.act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
                results.Add((index, response.StatusCode));
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Interlocked.Increment(ref successCount);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mentee {index} failed with exception: {ex.Message}");
            }
        }).ToArray();

        await Task.WhenAll(tasks);

        // Assert - All bookings should succeed since they're for different slots
        Assert.Equal(5, successCount);
        Assert.Equal(5, results.Count);
        Assert.All(results, result => Assert.Equal(HttpStatusCode.OK, result.statusCode));
    }

    [Fact]
    public async Task BookSession_ShouldHandleHighVolume_SequentialBookings()
    {
        // Arrange - Test booking many sessions sequentially
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_high_volume");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_high_volume");

        // Set availability for the entire week
        var daysOfWeek = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        foreach (var day in daysOfWeek)
        {
            await SetupMentorAvailability(mentorArrange, day, "09:00", "17:00");
        }

        var mentorSlug = await GetMentorSlug(mentorArrange);
        var successfulBookings = new List<int>();

        // Act - Book 20 sessions across the week
        for (int i = 0; i < 20; i++)
        {
            var dayIndex = i % 5; // Cycle through weekdays
            var hour = 9 + (i / 5); // 9, 10, 11, 12 for each day
            var targetDate = GetNextWeekday(daysOfWeek[dayIndex]);

            var bookingRequest = new
            {
                MentorSlug = mentorSlug,
                Date = targetDate.ToString("yyyy-MM-dd"),
                StartTime = $"{hour:D2}:00",
                EndTime = $"{hour + 1:D2}:00",
                TimeZoneId = "Africa/Tunis",
                Note = $"High volume booking {i}"
            };

            try
            {
                var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    successfulBookings.Add(result.GetProperty("sessionId").GetInt32());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Booking {i} failed with exception: {ex.Message}");
            }
        }

        // Assert - Most bookings should succeed (accounting for potential conflicts)
        Assert.True(successfulBookings.Count >= 15); // At least 75% success rate
        Assert.Equal(successfulBookings.Count, successfulBookings.Distinct().Count()); // All session IDs should be unique
    }

    #endregion

    #region Load Testing

    [Fact]
    public async Task BookSession_ShouldMaintainPerformance_UnderLoad()
    {
        // Arrange - Test system performance under moderate load
        var mentors = new List<(HttpClient arrange, HttpClient act)>();
        var mentees = new List<(HttpClient arrange, HttpClient act)>();

        // Create 3 mentors and 10 mentees
        for (int i = 0; i < 3; i++)
        {
            mentors.Add(await CreateMentor($"mentor_load_{i}"));
        }

        for (int i = 0; i < 10; i++)
        {
            mentees.Add(await CreateMentee($"mentee_load_{i}"));
        }

        // Set up availability for all mentors
        foreach (var (mentorArrange, _) in mentors)
        {
            await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        }

        var mentorSlugs = new List<string>();
        foreach (var (mentorArrange, _) in mentors)
        {
            mentorSlugs.Add(await GetMentorSlug(mentorArrange));
        }

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        var random = new Random();
        var results = new ConcurrentBag<(TimeSpan duration, HttpStatusCode statusCode)>();

        // Act - 50 booking attempts with random mentor-mentee combinations
        var tasks = Enumerable.Range(0, 50).Select(async i =>
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                var mentorIndex = random.Next(mentors.Count);
                var menteeIndex = random.Next(mentees.Count);
                var hour = 10 + (i % 6); // Hours 10-15

                var bookingRequest = new
                {
                    MentorSlug = mentorSlugs[mentorIndex],
                    Date = nextMonday.ToString("yyyy-MM-dd"),
                    StartTime = $"{hour:D2}:{(i % 2) * 30:D2}", // :00 or :30
                    EndTime = $"{hour + 1:D2}:{(i % 2) * 30:D2}",
                    TimeZoneId = "Africa/Tunis",
                    Note = $"Load test booking {i}"
                };

                var response = await mentees[menteeIndex].act.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
                stopwatch.Stop();
                
                results.Add((stopwatch.Elapsed, response.StatusCode));
            }
            catch (Exception)
            {
                stopwatch.Stop();
                results.Add((stopwatch.Elapsed, HttpStatusCode.InternalServerError));
            }
        }).ToArray();

        await Task.WhenAll(tasks);

        // Assert - Performance metrics
        var resultsList = results.ToList();
        Assert.Equal(50, resultsList.Count);

        var successfulRequests = resultsList.Where(r => r.statusCode == HttpStatusCode.OK).ToList();
        var avgResponseTime = resultsList.Average(r => r.duration.TotalMilliseconds);
        var maxResponseTime = resultsList.Max(r => r.duration.TotalMilliseconds);

        // Performance assertions
        Assert.True(avgResponseTime < 5000, $"Average response time too high: {avgResponseTime}ms");
        Assert.True(maxResponseTime < 10000, $"Max response time too high: {maxResponseTime}ms");
        Assert.True(successfulRequests.Count > 0, "No successful bookings under load");
    }

    #endregion

    #region Memory and Resource Tests

    [Fact]
    public async Task BookSession_ShouldNotLeakMemory_WithManyAttempts()
    {
        // Arrange - Test for memory leaks with many booking attempts
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_memory");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_memory");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "10:00");
        var mentorSlug = await GetMentorSlug(mentorArrange);
        var nextMonday = GetNextWeekday(DayOfWeek.Monday);

        var bookingRequest = new
        {
            MentorSlug = mentorSlug,
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis"
        };

        // First booking should succeed
        var firstResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);

        var initialMemory = GC.GetTotalMemory(true);

        // Act - Make 1000 failed booking attempts
        for (int i = 0; i < 1000; i++)
        {
            try
            {
                await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
            }
            catch
            {
                // Ignore exceptions for this test
            }

            // Force garbage collection every 100 iterations
            if (i % 100 == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        var finalMemory = GC.GetTotalMemory(true);
        var memoryIncrease = finalMemory - initialMemory;

        // Assert - Memory increase should be reasonable (less than 10MB)
        Assert.True(memoryIncrease < 10 * 1024 * 1024, 
            $"Memory increased by {memoryIncrease / 1024 / 1024}MB, possible memory leak");
    }

    #endregion

    #region Edge Case Stress Tests

    [Fact]
    public async Task BookSession_ShouldHandleInvalidInputs_Gracefully()
    {
        // Arrange - Test system resilience with various invalid inputs
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_invalid");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_invalid");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var mentorSlug = await GetMentorSlug(mentorArrange);

        var invalidRequests = new[]
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

        var results = new List<HttpStatusCode>();

        // Act - Send all invalid requests
        foreach (var request in invalidRequests)
        {
            try
            {
                var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, request);
                results.Add(response.StatusCode);
            }
            catch (Exception)
            {
                results.Add(HttpStatusCode.InternalServerError);
            }
        }

        // Assert - All should return appropriate error status codes
        Assert.All(results, statusCode => 
            Assert.True(statusCode == HttpStatusCode.BadRequest || 
                       statusCode == HttpStatusCode.UnprocessableEntity ||
                       statusCode == HttpStatusCode.InternalServerError));
    }

    [Fact]
    public async Task BookSession_ShouldHandleLargePayloads_Appropriately()
    {
        // Arrange - Test with unusually large note fields
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_large_payload");
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_large_payload");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");
        var nextMonday = GetNextWeekday(DayOfWeek.Monday);

        var largeNote = new string('A', 10000); // 10KB note

        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis",
            Note = largeNote
        };

        // Act
        var response = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert - Should either succeed or fail gracefully with validation error
        Assert.True(response.StatusCode == HttpStatusCode.OK || 
                   response.StatusCode == HttpStatusCode.BadRequest ||
                   response.StatusCode == HttpStatusCode.RequestEntityTooLarge);
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

    private static DateTime GetNextWeekday(DayOfWeek dayOfWeek)
    {
        var today = DateTime.UtcNow.Date;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7; // Next week to avoid past dates
        return today.AddDays(daysUntilTarget);
    }

    #endregion
}
*/
