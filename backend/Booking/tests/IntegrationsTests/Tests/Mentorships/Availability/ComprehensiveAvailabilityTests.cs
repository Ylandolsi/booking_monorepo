/*
using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;
using Booking.Modules.Mentorships.Features.Availability.Get.PerDay;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Availability;

/// <summary>
/// Comprehensive test suite for availability endpoints with edge cases and scenarios
/// </summary>
public class ComprehensiveAvailabilityTests : EnhancedMentorshipTestBase
{
    public ComprehensiveAvailabilityTests(IntegrationTestsWebAppFactory factory) : base(factory) { }

    #region Monthly Availability Advanced Tests

    [Fact]
    public async Task GetMentorAvailabilityByMonth_ShouldReturnCorrectSlots_WithComplexSchedule()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("complex_mentor", 80.0m);
        
        // Set complex availability pattern
        var complexAvailability = new[]
        {
            new { DayOfWeek = DayOfWeek.Monday, StartTime = "09:00", EndTime = "12:00" },
            new { DayOfWeek = DayOfWeek.Monday, StartTime = "14:00", EndTime = "18:00" },
            new { DayOfWeek = DayOfWeek.Wednesday, StartTime = "08:00", EndTime = "10:00" },
            new { DayOfWeek = DayOfWeek.Friday, StartTime = "15:00", EndTime = "20:00" }
        };

        foreach (var slot in complexAvailability)
        {
            await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, slot);
        }

        var currentDate = DateTime.Now;

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/complex_mentor/availability/month/{currentDate.Year}/{currentDate.Month}");

        // Assert
        response.EnsureSuccessStatusCode();
        var monthlyAvailability = await response.Content.ReadFromJsonAsync<MonthlyAvailabilityResponse>();
        Assert.NotNull(monthlyAvailability);
        
        // Verify Monday has split schedule
        var mondaySlots = monthlyAvailability.Days
            .Where(d => d.Date.DayOfWeek == DayOfWeek.Monday)
            .SelectMany(d => d.TimeSlots)
            .Count();
        Assert.True(mondaySlots > 0, "Monday should have available slots");
    }

    [Fact]
    public async Task GetMentorAvailabilityByMonth_ShouldHandleLeapYear_February()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("leap_year_mentor", 75.0m);
        
        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Saturday,
            StartTime = "10:00",
            EndTime = "16:00"
        });

        var leapYear = 2024; // Known leap year

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/leap_year_mentor/availability/month/{leapYear}/2");

        // Assert
        response.EnsureSuccessStatusCode();
        var monthlyAvailability = await response.Content.ReadFromJsonAsync<MonthlyAvailabilityResponse>();
        Assert.NotNull(monthlyAvailability);
        Assert.Equal(29, monthlyAvailability.Days.Count(d => d.Date.Month == 2)); // February in leap year
    }

    [Fact]
    public async Task GetMentorAvailabilityByMonth_ShouldFilterBookedSlots_WhenIncludeBookedSlotsIsFalse()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("filter_booked_mentor", 90.0m);
        var (menteeArrange, menteeAct) = await CreateMentee("filter_booked_mentee");

        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Tuesday,
            StartTime = "09:00",
            EndTime = "17:00"
        });

        // Book a session
        var nextTuesday = GetNextWeekday(DayOfWeek.Tuesday, new TimeOnly(10, 0));
        await menteeAct.PostAsJsonAsync(MentorshipsEndpoints.BookSession, new
        {
            MentorId = 1,
            StartDateTime = nextTuesday,
            DurationMinutes = 60,
            Note = "Booked session"
        });

        // Act - Get availability without booked slots
        var response = await menteeAct.GetAsync(
            $"/mentorships/mentors/filter_booked_mentor/availability/month/{nextTuesday.Year}/{nextTuesday.Month}?includeBookedSlots=false");

        // Assert
        response.EnsureSuccessStatusCode();
        var monthlyAvailability = await response.Content.ReadFromJsonAsync<MonthlyAvailabilityResponse>();
        Assert.NotNull(monthlyAvailability);
        
        var bookedDay = monthlyAvailability.Days.FirstOrDefault(d => d.Date.Date == nextTuesday.Date);
        Assert.NotNull(bookedDay);
        Assert.True(bookedDay.TimeSlots.All(ts => !ts.IsBooked), "No booked slots should be included");
    }

    [Fact]
    public async Task GetMentorAvailabilityByMonth_ShouldHandleInvalidMonth_ReturnBadRequest()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("invalid_month_mentor", 60.0m);

        // Act - Invalid month (13)
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/invalid_month_mentor/availability/month/2024/13");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Daily Availability Advanced Tests

    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldCalculateSlots_WithBufferTime()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("buffer_mentor", 70.0m);
        
        // Become mentor with 45-minute buffer
        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.UpdateMentorProfile, new
        {
            HourlyRate = 70.0m,
            BufferTimeMinutes = 45
        });

        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Thursday,
            StartTime = "09:00",
            EndTime = "13:00" // 4 hours
        });

        var nextThursday = GetNextWeekday(DayOfWeek.Thursday, new TimeOnly(9, 0));

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/buffer_mentor/availability/day/{nextThursday:yyyy-MM-dd}");

        // Assert
        response.EnsureSuccessStatusCode();
        var dailyAvailability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(dailyAvailability);
        
        // With 45-minute buffer, slots should be spaced accordingly
        Assert.True(dailyAvailability.TimeSlots.Count > 0);
        Assert.True(dailyAvailability.Summary.TotalSlots > 0);
        Assert.Equal(100.0m, dailyAvailability.Summary.AvailabilityPercentage); // All available initially
    }

    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldHandleOverlappingSessions_WithBufferTime()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("overlap_mentor", 85.0m);
        var (mentee1Arrange, mentee1Act) = await CreateMentee("mentee1");
        var (mentee2Arrange, mentee2Act) = await CreateMentee("mentee2");

        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Wednesday,
            StartTime = "08:00",
            EndTime = "18:00"
        });

        var nextWednesday = GetNextWeekday(DayOfWeek.Wednesday, new TimeOnly(8, 0));

        // Book two sessions with overlap consideration
        await mentee1Act.PostAsJsonAsync(MentorshipsEndpoints.BookSession, new
        {
            MentorId = 1,
            StartDateTime = nextWednesday.AddHours(2), // 10:00
            DurationMinutes = 60,
            Note = "First session"
        });

        await mentee2Act.PostAsJsonAsync(MentorshipsEndpoints.BookSession, new
        {
            MentorId = 1,
            StartDateTime = nextWednesday.AddHours(4), // 12:00
            DurationMinutes = 90,
            Note = "Second session"
        });

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/overlap_mentor/availability/day/{nextWednesday:yyyy-MM-dd}");

        // Assert
        response.EnsureSuccessStatusCode();
        var dailyAvailability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(dailyAvailability);
        
        var bookedSlots = dailyAvailability.TimeSlots.Count(ts => ts.IsBooked);
        Assert.True(bookedSlots > 0, "Should have booked slots");
        Assert.True(dailyAvailability.Summary.BookedSlots > 0);
        Assert.True(dailyAvailability.Summary.AvailabilityPercentage < 100.0m);
    }

    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldHandlePastDate_ReturnEmptyOrError()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("past_date_mentor", 55.0m);
        
        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "17:00"
        });

        var pastDate = DateTime.Now.AddDays(-7); // Last week

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/past_date_mentor/availability/day/{pastDate:yyyy-MM-dd}");

        // Assert
        response.EnsureSuccessStatusCode();
        var dailyAvailability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(dailyAvailability);
        // Past dates should return availability but might be handled differently in business logic
    }

    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldHandleNonExistentMentor_ReturnNotFound()
    {
        // Arrange
        var nonExistentMentorSlug = "non-existent-mentor";
        var today = DateTime.Now;

        // Act
        var (publicArrange, publicAct) = GetClientsForUser("public_user");
        var response = await publicAct.GetAsync(
            $"/mentorships/mentors/{nonExistentMentorSlug}/availability/day/{today:yyyy-MM-dd}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Edge Cases and Error Scenarios

    [Fact]
    public async Task AvailabilityEndpoints_ShouldHandleConcurrentRequests_Gracefully()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("concurrent_mentor", 95.0m);
        
        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Friday,
            StartTime = "09:00",
            EndTime = "17:00"
        });

        var nextFriday = GetNextWeekday(DayOfWeek.Friday, new TimeOnly(9, 0));

        // Act - Make concurrent requests
        var tasks = Enumerable.Range(0, 5).Select(async i =>
        {
            var (clientArrange, clientAct) = GetClientsForUser($"concurrent_user_{i}");
            return await clientAct.GetAsync(
                $"/mentorships/mentors/concurrent_mentor/availability/day/{nextFriday:yyyy-MM-dd}");
        });

        var responses = await Task.WhenAll(tasks);

        // Assert
        Assert.True(responses.All(r => r.IsSuccessStatusCode), "All concurrent requests should succeed");
    }

    [Fact]
    public async Task AvailabilityEndpoints_ShouldHandleSpecialCharacters_InMentorSlug()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("special-mentor_123", 65.0m);
        
        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Saturday,
            StartTime = "10:00",
            EndTime = "16:00"
        });

        var today = DateTime.Now;

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/special-mentor_123/availability/month/{today.Year}/{today.Month}");

        // Assert
        response.EnsureSuccessStatusCode();
        var monthlyAvailability = await response.Content.ReadFromJsonAsync<MonthlyAvailabilityResponse>();
        Assert.NotNull(monthlyAvailability);
        Assert.Equal("special-mentor_123", monthlyAvailability.MentorSlug);
    }

    [Fact]
    public async Task AvailabilityEndpoints_ShouldHandleTimeZoneBoundaries_Correctly()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("timezone_mentor", 100.0m);
        
        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Sunday,
            StartTime = "00:00", // Midnight
            EndTime = "23:30"   // Almost midnight next day
        });

        var nextSunday = GetNextWeekday(DayOfWeek.Sunday, new TimeOnly(0, 0));

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/timezone_mentor/availability/day/{nextSunday:yyyy-MM-dd}");

        // Assert
        response.EnsureSuccessStatusCode();
        var dailyAvailability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(dailyAvailability);
        Assert.True(dailyAvailability.TimeSlots.Count > 40, "Should have many slots for almost full day");
    }

    [Fact]
    public async Task GetMentorAvailabilityByMonth_ShouldReturnEmptyDays_ForInactiveMentor()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("inactive_mentor", 50.0m);
        
        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "17:00"
        });

        // Deactivate mentor
        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.DeactivateMentor, new { });

        var currentDate = DateTime.Now;

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/inactive_mentor/availability/month/{currentDate.Year}/{currentDate.Month}");

        // Assert
        // Depending on business logic, this might return 404 or empty availability
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var monthlyAvailability = await response.Content.ReadFromJsonAsync<MonthlyAvailabilityResponse>();
            Assert.NotNull(monthlyAvailability);
            Assert.True(monthlyAvailability.Days.All(d => !d.HasAvailability), 
                "Inactive mentor should have no availability");
        }
        else
        {
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    #endregion

    #region Performance and Load Tests

    [Fact]
    public async Task AvailabilityEndpoints_ShouldHandleLargeMonth_WithManySlots()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("busy_mentor", 120.0m);
        
        // Set availability for every day with multiple slots
        foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
        {
            var slots = new[]
            {
                new { DayOfWeek = day, StartTime = "06:00", EndTime = "10:00" },
                new { DayOfWeek = day, StartTime = "12:00", EndTime = "16:00" },
                new { DayOfWeek = day, StartTime = "18:00", EndTime = "22:00" }
            };

            foreach (var slot in slots)
            {
                await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, slot);
            }
        }

        var currentDate = DateTime.Now;

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/busy_mentor/availability/month/{currentDate.Year}/{currentDate.Month}");

        // Assert
        response.EnsureSuccessStatusCode();
        var monthlyAvailability = await response.Content.ReadFromJsonAsync<MonthlyAvailabilityResponse>();
        Assert.NotNull(monthlyAvailability);
        
        var totalSlots = monthlyAvailability.Days.Sum(d => d.TimeSlots.Count);
        Assert.True(totalSlots > 500, "Should have many slots for busy mentor");
    }

    #endregion

    #region Business Logic Validation

    [Fact]
    public async Task AvailabilitySlots_ShouldRespectMinimumDuration_Requirements()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("duration_mentor", 80.0m);
        
        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Tuesday,
            StartTime = "09:00",
            EndTime = "09:30" // Exactly 30 minutes
        });

        var nextTuesday = GetNextWeekday(DayOfWeek.Tuesday, new TimeOnly(9, 0));

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/duration_mentor/availability/day/{nextTuesday:yyyy-MM-dd}");

        // Assert
        response.EnsureSuccessStatusCode();
        var dailyAvailability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(dailyAvailability);
        Assert.Equal(1, dailyAvailability.TimeSlots.Count); // Exactly one 30-minute slot
    }

    [Fact]
    public async Task AvailabilityCalculation_ShouldHandleBufferTimeCorrectly_BetweenSessions()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("buffer_calc_mentor", 110.0m);
        var (mentee1Arrange, mentee1Act) = await CreateMentee("buffer_mentee1");
        var (mentee2Arrange, mentee2Act) = await CreateMentee("buffer_mentee2");

        // Set 30-minute buffer time
        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.UpdateMentorProfile, new
        {
            HourlyRate = 110.0m,
            BufferTimeMinutes = 30
        });

        await mentorAct.PostAsJsonAsync(MentorshipsEndpoints.SetAvailability, new
        {
            DayOfWeek = DayOfWeek.Thursday,
            StartTime = "08:00",
            EndTime = "16:00"
        });

        var nextThursday = GetNextWeekday(DayOfWeek.Thursday, new TimeOnly(8, 0));

        // Book session from 10:00-11:00
        await mentee1Act.PostAsJsonAsync(MentorshipsEndpoints.BookSession, new
        {
            MentorId = 1,
            StartDateTime = nextThursday.AddHours(2), // 10:00
            DurationMinutes = 60,
            Note = "Buffer test session"
        });

        // Act
        var response = await mentorAct.GetAsync(
            $"/mentorships/mentors/buffer_calc_mentor/availability/day/{nextThursday:yyyy-MM-dd}");

        // Assert
        response.EnsureSuccessStatusCode();
        var dailyAvailability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(dailyAvailability);
        
        // Verify buffer time is respected
        // 9:30-10:00 should be unavailable (buffer before session)
        // 10:00-11:00 should be unavailable (session time)
        // 11:00-11:30 should be unavailable (buffer after session)
        var unavailableSlots = dailyAvailability.TimeSlots.Count(ts => !ts.IsAvailable);
        Assert.True(unavailableSlots >= 3, "Buffer time should make adjacent slots unavailable");
    }

    #endregion

    #region Helper Methods

    private static DateTime GetNextWeekday(DayOfWeek dayOfWeek, TimeOnly time)
    {
        var today = DateTime.Now.Date;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7; // Next week
        
        var targetDate = today.AddDays(daysUntilTarget);
        return targetDate.Add(time.ToTimeSpan());
    }

    #endregion
}
*/
