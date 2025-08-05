using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

/// <summary>
/// Tests for edge cases, error scenarios, and boundary conditions in the mentorship system
/// Covers validation, error handling, and system limits
/// </summary>
public class MentorshipEdgeCasesTests : MentorshipTestBase
{
    public MentorshipEdgeCasesTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task BookingConflicts_ShouldPreventOverlappingSessions()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        var baseTime = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10);

        // Book first session
        var session1Id = await BookSession(
            menteeClient, 
            mentorData.UserSlug.ToString(), 
            baseTime, 
            60, 
            "First session");

        // Try to book overlapping session (same mentor, overlapping time)
        var overlappingBookingPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = baseTime.AddMinutes(30), // Overlaps with first session
            DurationMinutes = 60,
            Note = "Overlapping session attempt"
        };

        // Act
        var overlappingResponse = await menteeClient.PostAsJsonAsync(
            MentorshipsEndpoints.BookSession, 
            overlappingBookingPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, overlappingResponse.StatusCode);
    }

    [Fact]
    public async Task AvailabilityConflicts_ShouldPreventOverlappingTimeSlots()
    {
        // Arrange
        var mentorClient = await CreateAuthenticatedMentorClient();

        // Set initial availability
        var initialAvailability = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "17:00"
        };

        var initialResponse = await mentorClient.PostAsJsonAsync(
            MentorshipsEndpoints.SetAvailability, 
            initialAvailability);
        initialResponse.EnsureSuccessStatusCode();

        // Try to set overlapping availability
        var overlappingAvailability = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "12:00", // Overlaps with existing 09:00-17:00
            EndTime = "20:00"
        };

        // Act
        var overlappingResponse = await mentorClient.PostAsJsonAsync(
            MentorshipsEndpoints.SetAvailability, 
            overlappingAvailability);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, overlappingResponse.StatusCode);
    }

    [Fact]
    public async Task InvalidSessionDurations_ShouldReturnBadRequest()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        // Test various invalid durations
        var invalidDurations = new[] { -30, 0, 10, 15, 300, 1000 };

        foreach (var invalidDuration in invalidDurations)
        {
            var bookingPayload = new
            {
                MentorSlug = mentorData.UserSlug.ToString(),
                ScheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10 + Array.IndexOf(invalidDurations, invalidDuration)),
                DurationMinutes = invalidDuration,
                Note = $"Invalid duration test: {invalidDuration}"
            };

            // Act
            var response = await menteeClient.PostAsJsonAsync(
                MentorshipsEndpoints.BookSession, 
                bookingPayload);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async Task PastDateBooking_ShouldReturnBadRequest()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        var pastDates = new[]
        {
            DateTime.UtcNow.AddHours(-1),
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddMinutes(-30)
        };

        foreach (var pastDate in pastDates)
        {
            var bookingPayload = new
            {
                MentorSlug = mentorData.UserSlug.ToString(),
                ScheduledAt = pastDate,
                DurationMinutes = 60,
                Note = "Past date booking attempt"
            };

            // Act
            var response = await menteeClient.PostAsJsonAsync(
                MentorshipsEndpoints.BookSession, 
                bookingPayload);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async Task InvalidHourlyRates_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidRates = new[] { -10.0m, 0.0m, -1.0m, -100.5m };

        foreach (var invalidRate in invalidRates)
        {
            var userData = await CreateUserAndLogin();

            var becomeMentorPayload = new
            {
                HourlyRate = invalidRate,
                Bio = "Test mentor bio"
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                MentorshipsEndpoints.BecomeMentor, 
                becomeMentorPayload);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async Task EmptyOrNullBio_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidBios = new[] { "", null, "   ", "\t\n" };

        foreach (var invalidBio in invalidBios)
        {
            var userData = await CreateUserAndLogin();

            var becomeMentorPayload = new
            {
                HourlyRate = 50.0m,
                Bio = invalidBio
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                MentorshipsEndpoints.BecomeMentor, 
                becomeMentorPayload);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async Task InvalidTimeRanges_ShouldReturnBadRequest()
    {
        // Arrange
        var mentorClient = await CreateAuthenticatedMentorClient();

        var invalidTimeRanges = new[]
        {
            new { StartTime = "17:00", EndTime = "09:00" }, // End before start
            new { StartTime = "12:00", EndTime = "12:00" }, // Same time
            new { StartTime = "25:00", EndTime = "26:00" }, // Invalid time format
            new { StartTime = "09:30", EndTime = "09:45" }  // Too short duration
        };

        foreach (var timeRange in invalidTimeRanges)
        {
            var availabilityPayload = new
            {
                DayOfWeek = DayOfWeek.Monday,
                StartTime = timeRange.StartTime,
                EndTime = timeRange.EndTime
            };

            // Act
            var response = await mentorClient.PostAsJsonAsync(
                MentorshipsEndpoints.SetAvailability, 
                availabilityPayload);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public async Task InvalidReviewRatings_ShouldReturnBadRequest(int invalidRating)
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        var sessionId = await BookSession(
            menteeClient, 
            mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        await ConfirmSessionAsMentor(mentorClient, sessionId);

        var reviewPayload = new
        {
            SessionId = sessionId,
            Rating = invalidRating,
            Comment = "Test review with invalid rating"
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(
            MentorshipsEndpoints.AddReview, 
            reviewPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookingOutsideAvailability_ShouldReturnBadRequest()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();

        // Set availability only for Monday 9-17
        await SetMentorAvailability(mentorClient, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));

        var outsideAvailabilityTimes = new[]
        {
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(8),   // Before availability
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(18),  // After availability
            MentorshipTestData.Sessions.NextWeekTuesday.AddHours(10), // Different day
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(16.5) // Would end after availability
        };

        foreach (var scheduledTime in outsideAvailabilityTimes)
        {
            var bookingPayload = new
            {
                MentorSlug = mentorData.UserSlug.ToString(),
                ScheduledAt = scheduledTime,
                DurationMinutes = 60,
                Note = "Outside availability test"
            };

            // Act
            var response = await menteeClient.PostAsJsonAsync(
                MentorshipsEndpoints.BookSession, 
                bookingPayload);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async Task NonexistentMentorBooking_ShouldReturnNotFound()
    {
        // Arrange
        var menteeClient = await CreateAuthenticatedMenteeClient();

        var bookingPayload = new
        {
            MentorSlug = "nonexistent-mentor-slug",
            ScheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10),
            DurationMinutes = 60,
            Note = "Booking with nonexistent mentor"
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(
            MentorshipsEndpoints.BookSession, 
            bookingPayload);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DoubleBecomeMentor_ShouldReturnBadRequest()
    {
        // Arrange
        var mentorClient = await CreateAuthenticatedMentorClient(); // Already a mentor

        var secondBecomeMentorPayload = new
        {
            HourlyRate = 100.0m,
            Bio = "Trying to become mentor again"
        };

        // Act
        var response = await mentorClient.PostAsJsonAsync(
            MentorshipsEndpoints.BecomeMentor, 
            secondBecomeMentorPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CancelNonexistentSession_ShouldReturnNotFound()
    {
        // Arrange
        var menteeClient = await CreateAuthenticatedMenteeClient();

        var cancelPayload = new
        {
            CancellationReason = "Testing nonexistent session cancellation"
        };

        // Act
        var response = await menteeClient.PutAsJsonAsync(
            MentorshipsEndpoints.CancelSession.Replace("{sessionId}", "99999"),
            cancelPayload);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateNonexistentAvailability_ShouldReturnNotFound()
    {
        // Arrange
        var mentorClient = await CreateAuthenticatedMentorClient();

        var updatePayload = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "10:00",
            EndTime = "16:00"
        };

        // Act
        var response = await mentorClient.PutAsJsonAsync(
            MentorshipsEndpoints.UpdateAvailability.Replace("{availabilityId}", "99999"),
            updatePayload);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ReviewNonexistentSession_ShouldReturnNotFound()
    {
        // Arrange
        var menteeClient = await CreateAuthenticatedMenteeClient();

        var reviewPayload = new
        {
            SessionId = 99999,
            Rating = 5,
            Comment = "Review for nonexistent session"
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(
            MentorshipsEndpoints.AddReview, 
            reviewPayload);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ExcessivelyLongNote_ShouldReturnBadRequest()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        var excessivelyLongNote = new string('A', 10000); // 10,000 characters

        var bookingPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10),
            DurationMinutes = 60,
            Note = excessivelyLongNote
        };

        // Act
        var response = await menteeClient.PostAsJsonAsync(
            MentorshipsEndpoints.BookSession, 
            bookingPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task InvalidGoogleMeetUrl_ShouldReturnBadRequest()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        var sessionId = await BookSession(
            menteeClient, 
            mentorData.UserSlug.ToString(), 
            MentorshipTestData.Sessions.NextWeekMonday.AddHours(10));

        var invalidUrls = new[]
        {
            "not-a-url",
            "http://invalid-meet-url.com",
            "https://zoom.us/invalid",
            "",
            "meet.google.com/without-protocol"
        };

        foreach (var invalidUrl in invalidUrls)
        {
            var confirmPayload = new
            {
                GoogleMeetUrl = invalidUrl
            };

            // Act
            var response = await mentorClient.PutAsJsonAsync(
                MentorshipsEndpoints.ConfirmSession.Replace("{sessionId}", sessionId.ToString()),
                confirmPayload);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async Task MentorshipRelationshipWithSelf_ShouldReturnBadRequest()
    {
        // Arrange - Create a user who is both mentor and trying to be mentee
        var mentorClient = await CreateAuthenticatedMentorClient();

        // Try to request mentorship relationship with themselves
        var (mentorData, _) = await CreateMentorAndLogin(); // Get mentor data
        
        var relationshipPayload = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            Message = "Trying to mentor myself"
        };

        // Act
        var response = await mentorClient.PostAsJsonAsync(
            MentorshipsEndpoints.RequestMentorship, 
            relationshipPayload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ConcurrentBookingConflict_ShouldPreventDoubleBooking()
    {
        // Arrange
        var (mentorClient, menteeClient1, mentorData, _) = await CreateMentorMenteePair();
        var menteeClient2 = await CreateAuthenticatedMenteeClient();
        
        await SetupMentorWeeklyAvailability(mentorClient);

        var scheduledTime = MentorshipTestData.Sessions.NextWeekMonday.AddHours(10);

        var bookingPayload1 = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = scheduledTime,
            DurationMinutes = 60,
            Note = "First concurrent booking"
        };

        var bookingPayload2 = new
        {
            MentorSlug = mentorData.UserSlug.ToString(),
            ScheduledAt = scheduledTime,
            DurationMinutes = 60,
            Note = "Second concurrent booking"
        };

        // Act - Try to book same time slot concurrently
        var booking1Task = menteeClient1.PostAsJsonAsync(MentorshipsEndpoints.BookSession, bookingPayload1);
        var booking2Task = menteeClient2.PostAsJsonAsync(MentorshipsEndpoints.BookSession, bookingPayload2);

        var responses = await Task.WhenAll(booking1Task, booking2Task);

        // Assert - Only one should succeed
        var successCount = responses.Count(r => r.IsSuccessStatusCode);
        var conflictCount = responses.Count(r => r.StatusCode == HttpStatusCode.BadRequest || r.StatusCode == HttpStatusCode.Conflict);

        Assert.Equal(1, successCount);
        Assert.Equal(1, conflictCount);
    }
}
