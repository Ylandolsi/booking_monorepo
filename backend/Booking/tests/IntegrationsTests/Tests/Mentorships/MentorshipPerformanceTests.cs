using System.Diagnostics;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

/// <summary>
/// Performance and stress tests for the mentorship system
/// Tests system behavior under load and with large datasets
/// </summary>
public class MentorshipPerformanceTests : MentorshipTestBase
{
    public MentorshipPerformanceTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task BulkSessionCreation_ShouldHandleMultipleSessionsEfficiently()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        var sessionCount = 20;
        var stopwatch = Stopwatch.StartNew();

        // Act - Create multiple sessions
        var sessionTasks = new List<Task<int>>();
        for (int i = 0; i < sessionCount; i++)
        {
            var scheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddDays(i % 5).AddHours(9 + i % 8);
            sessionTasks.Add(BookSession(
                menteeClient, 
                mentorData.UserSlug.ToString(), 
                scheduledAt, 
                60, 
                $"Bulk session {i + 1}"));
        }

        var sessionIds = await Task.WhenAll(sessionTasks);
        stopwatch.Stop();

        // Assert
        Assert.Equal(sessionCount, sessionIds.Length);
        Assert.All(sessionIds, sessionId => Assert.True(sessionId > 0));
        
        // Performance assertion - should complete within reasonable time
        Assert.True(stopwatch.ElapsedMilliseconds < 30000, 
            $"Bulk session creation took {stopwatch.ElapsedMilliseconds}ms, expected < 30000ms");

        // Verify all sessions are retrievable
        var mentorSessionsResponse = await mentorClient.GetAsync(MentorshipsEndpoints.GetMentorSessions);
        mentorSessionsResponse.EnsureSuccessStatusCode();
        
        var mentorSessions = await mentorSessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(mentorSessions);
        Assert.Equal(sessionCount, mentorSessions.Count);
    }

    [Fact]
    public async Task MultipleMentorManagement_ShouldScaleWithManyMentors()
    {
        // Arrange
        var mentorCount = 10;
        var stopwatch = Stopwatch.StartNew();

        // Act - Create multiple mentors concurrently
        var mentorTasks = new List<Task<HttpClient>>();
        for (int i = 0; i < mentorCount; i++)
        {
            var rate = 50.0m + (i * 10); // Varying rates
            var bio = $"Expert mentor #{i + 1} with specialized knowledge in area {i + 1}";
            mentorTasks.Add(CreateAuthenticatedMentorClient(hourlyRate: rate, bio: bio));
        }

        var mentorClients = await Task.WhenAll(mentorTasks);
        
        // Set availability for all mentors concurrently
        var availabilityTasks = mentorClients.Select(SetupMentorWeeklyAvailability).ToArray();
        var availabilityResults = await Task.WhenAll(availabilityTasks);
        
        stopwatch.Stop();

        // Assert
        Assert.Equal(mentorCount, mentorClients.Length);
        Assert.Equal(mentorCount, availabilityResults.Length);
        Assert.All(availabilityResults, result => Assert.Equal(5, result.Count)); // 5 weekday slots each

        // Performance assertion
        Assert.True(stopwatch.ElapsedMilliseconds < 45000, 
            $"Multiple mentor creation took {stopwatch.ElapsedMilliseconds}ms, expected < 45000ms");
    }

    [Fact]
    public async Task HighVolumeAvailabilityManagement_ShouldHandleManyTimeSlots()
    {
        // Arrange
        var mentorClient = await CreateAuthenticatedMentorClient();
        var stopwatch = Stopwatch.StartNew();

        // Act - Create availability for all days and multiple time slots per day
        var availabilityTasks = new List<Task<int>>();
        
        foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
        {
            // Morning slot
            availabilityTasks.Add(SetMentorAvailability(
                mentorClient, day, new TimeOnly(9, 0), new TimeOnly(12, 0)));
            
            // Afternoon slot
            availabilityTasks.Add(SetMentorAvailability(
                mentorClient, day, new TimeOnly(14, 0), new TimeOnly(17, 0)));
            
            // Evening slot (weekends only)
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
            {
                availabilityTasks.Add(SetMentorAvailability(
                    mentorClient, day, new TimeOnly(19, 0), new TimeOnly(21, 0)));
            }
        }

        var availabilityIds = await Task.WhenAll(availabilityTasks);
        stopwatch.Stop();

        // Assert
        var expectedSlots = (7 * 2) + 2; // 7 days * 2 slots + 2 weekend evening slots = 16
        Assert.Equal(expectedSlots, availabilityIds.Length);
        Assert.All(availabilityIds, id => Assert.True(id > 0));

        // Performance assertion
        Assert.True(stopwatch.ElapsedMilliseconds < 20000, 
            $"High volume availability creation took {stopwatch.ElapsedMilliseconds}ms, expected < 20000ms");
    }

    [Fact]
    public async Task ConcurrentSessionBooking_ShouldHandleSimultaneousRequests()
    {
        // Arrange
        var (mentorClient, _, mentorData, _) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        var menteeCount = 5;
        var sessionsPerMentee = 3;

        // Create multiple mentees
        var menteeClients = new List<HttpClient>();
        for (int i = 0; i < menteeCount; i++)
        {
            menteeClients.Add(await CreateAuthenticatedMenteeClient());
        }

        var stopwatch = Stopwatch.StartNew();

        // Act - Have all mentees book sessions concurrently
        var allBookingTasks = new List<Task<int>>();
        
        for (int menteeIndex = 0; menteeIndex < menteeCount; menteeIndex++)
        {
            var menteeClient = menteeClients[menteeIndex];
            
            for (int sessionIndex = 0; sessionIndex < sessionsPerMentee; sessionIndex++)
            {
                var scheduledAt = MentorshipTestData.Sessions.NextWeekMonday
                    .AddDays(sessionIndex % 5)
                    .AddHours(9 + ((menteeIndex * sessionsPerMentee + sessionIndex) % 8));

                allBookingTasks.Add(BookSession(
                    menteeClient, 
                    mentorData.UserSlug.ToString(), 
                    scheduledAt, 
                    60, 
                    $"Concurrent session from mentee {menteeIndex + 1}, session {sessionIndex + 1}"));
            }
        }

        var sessionIds = await Task.WhenAll(allBookingTasks);
        stopwatch.Stop();

        // Assert
        var expectedSessions = menteeCount * sessionsPerMentee;
        Assert.Equal(expectedSessions, sessionIds.Length);
        Assert.All(sessionIds, sessionId => Assert.True(sessionId > 0));

        // Verify no duplicate session IDs (all bookings should be unique)
        var uniqueSessionIds = sessionIds.Distinct().ToList();
        Assert.Equal(expectedSessions, uniqueSessionIds.Count);

        // Performance assertion
        Assert.True(stopwatch.ElapsedMilliseconds < 25000, 
            $"Concurrent session booking took {stopwatch.ElapsedMilliseconds}ms, expected < 25000ms");
    }

    [Fact]
    public async Task MassiveReviewCreation_ShouldHandleLargeVolumeOfReviews()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        // Create many sessions first
        var sessionCount = 15;
        var sessionIds = await BookMultipleSessionsForTesting(
            menteeClient, 
            mentorData.UserSlug.ToString(), 
            sessionCount);

        // Confirm all sessions
        foreach (var sessionId in sessionIds)
        {
            await ConfirmSessionAsMentor(mentorClient, sessionId);
        }

        var stopwatch = Stopwatch.StartNew();

        // Act - Create reviews for all sessions concurrently
        var reviewTasks = new List<Task<int>>();
        for (int i = 0; i < sessionIds.Count; i++)
        {
            var (rating, comment) = MentorshipTestData.Reviews.SampleReviews[i % MentorshipTestData.Reviews.SampleReviews.Length];
            reviewTasks.Add(AddSessionReview(
                menteeClient, 
                sessionIds[i], 
                rating, 
                $"{comment} - Review #{i + 1}"));
        }

        var reviewIds = await Task.WhenAll(reviewTasks);
        stopwatch.Stop();

        // Assert
        Assert.Equal(sessionCount, reviewIds.Length);
        Assert.All(reviewIds, reviewId => Assert.True(reviewId > 0));

        // Verify all reviews are retrievable
        var reviewsResponse = await _client.GetAsync(
            MentorshipsEndpoints.GetMentorReviews.Replace("{mentorSlug}", mentorData.UserSlug.ToString()));
        reviewsResponse.EnsureSuccessStatusCode();

        var reviews = await reviewsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
        Assert.NotNull(reviews);
        Assert.Equal(sessionCount, reviews.Count);

        // Performance assertion
        Assert.True(stopwatch.ElapsedMilliseconds < 20000, 
            $"Mass review creation took {stopwatch.ElapsedMilliseconds}ms, expected < 20000ms");
    }

    [Fact]
    public async Task ComplexMentorshipEcosystem_ShouldHandleRealisticScale()
    {
        // Arrange - Create a realistic mentorship ecosystem
        var mentorCount = 5;
        var menteeCount = 10;
        var sessionsPerPair = 2;

        var stopwatch = Stopwatch.StartNew();

        // Create mentors
        var mentorTasks = Enumerable.Range(0, mentorCount)
            .Select(i => CreateMentorAndLogin(
                50.0m + (i * 25), 
                $"Expert mentor in field {i + 1} with {5 + i} years experience"))
            .ToArray();

        var mentors = await Task.WhenAll(mentorTasks);

        // Create mentees
        var menteeTasks = Enumerable.Range(0, menteeCount)
            .Select(_ => CreateMenteeAndLogin())
            .ToArray();

        var mentees = await Task.WhenAll(menteeTasks);

        // Set availability for all mentors
        var availabilityTasks = mentors.Select(mentor => SetupMentorWeeklyAvailability(mentor.mentorClient)).ToArray();
        await Task.WhenAll(availabilityTasks);

        // Book sessions between mentees and mentors
        var allSessionTasks = new List<Task<int>>();
        for (int menteeIndex = 0; menteeIndex < menteeCount; menteeIndex++)
        {
            var mentorIndex = menteeIndex % mentorCount; // Distribute mentees across mentors
            var mentor = mentors[mentorIndex];
            var mentee = mentees[menteeIndex];

            for (int sessionIndex = 0; sessionIndex < sessionsPerPair; sessionIndex++)
            {
                var scheduledAt = MentorshipTestData.Sessions.NextWeekMonday
                    .AddDays(sessionIndex * 2)
                    .AddHours(9 + (menteeIndex % 8));

                allSessionTasks.Add(BookSession(
                    mentee.menteeClient, 
                    mentor.mentorData.UserSlug.ToString(), 
                    scheduledAt, 
                    60, 
                    $"Ecosystem session: Mentee {menteeIndex + 1} with Mentor {mentorIndex + 1}"));
            }
        }

        var allSessionIds = await Task.WhenAll(allSessionTasks);

        // Confirm half of the sessions (simulate realistic confirmation rate)
        var sessionsToConfirm = allSessionIds.Take(allSessionIds.Length / 2).ToArray();
        var confirmTasks = new List<Task>();
        
        foreach (var sessionId in sessionsToConfirm)
        {
            // Find the mentor for this session (simplified approach)
            var mentorClient = mentors[sessionId % mentorCount].mentorClient;
            confirmTasks.Add(ConfirmSessionAsMentor(mentorClient, sessionId));
        }

        await Task.WhenAll(confirmTasks);

        stopwatch.Stop();

        // Assert - Verify the ecosystem is functioning
        var expectedTotalSessions = menteeCount * sessionsPerPair;
        Assert.Equal(expectedTotalSessions, allSessionIds.Length);
        Assert.All(allSessionIds, sessionId => Assert.True(sessionId > 0));

        // Verify each mentor has sessions
        foreach (var mentor in mentors)
        {
            var mentorSessionsResponse = await mentor.mentorClient.GetAsync(MentorshipsEndpoints.GetMentorSessions);
            mentorSessionsResponse.EnsureSuccessStatusCode();
            
            var mentorSessions = await mentorSessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
            Assert.NotNull(mentorSessions);
            Assert.True(mentorSessions.Count > 0);
        }

        // Verify each mentee has sessions
        foreach (var mentee in mentees)
        {
            var menteeSessionsResponse = await mentee.menteeClient.GetAsync(MentorshipsEndpoints.GetMenteeSessions);
            menteeSessionsResponse.EnsureSuccessStatusCode();
            
            var menteeSessions = await menteeSessionsResponse.Content.ReadFromJsonAsync<List<dynamic>>();
            Assert.NotNull(menteeSessions);
            Assert.Equal(sessionsPerPair, menteeSessions.Count);
        }

        // Performance assertion - Complex ecosystem should be created efficiently
        Assert.True(stopwatch.ElapsedMilliseconds < 60000, 
            $"Complex mentorship ecosystem creation took {stopwatch.ElapsedMilliseconds}ms, expected < 60000ms");
    }

    [Fact]
    public async Task HighConcurrencyAvailabilityAccess_ShouldHandleSimultaneousReads()
    {
        // Arrange
        var (mentorData, mentorClient) = await CreateMentorAndLogin();
        await SetupMentorWeeklyAvailability(mentorClient);

        var concurrentRequestCount = 20;
        var stopwatch = Stopwatch.StartNew();

        // Act - Make many concurrent requests to get availability
        var availabilityTasks = Enumerable.Range(0, concurrentRequestCount)
            .Select(_ => _client.GetAsync(
                MentorshipsEndpoints.GetMentorAvailability.Replace("{mentorSlug}", mentorData.UserSlug.ToString())))
            .ToArray();

        var responses = await Task.WhenAll(availabilityTasks);
        stopwatch.Stop();

        // Assert
        Assert.All(responses, response => 
        {
            Assert.True(response.IsSuccessStatusCode);
        });

        // Verify response consistency
        var firstResponseContent = await responses[0].Content.ReadAsStringAsync();
        foreach (var response in responses.Skip(1))
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Equal(firstResponseContent, responseContent);
        }

        // Performance assertion
        Assert.True(stopwatch.ElapsedMilliseconds < 15000, 
            $"High concurrency availability access took {stopwatch.ElapsedMilliseconds}ms, expected < 15000ms");
    }

    [Fact]
    public async Task StressTestSessionManagement_ShouldHandleRapidOperations()
    {
        // Arrange
        var (mentorClient, menteeClient, mentorData, menteeData) = await CreateMentorMenteePair();
        await SetupMentorWeeklyAvailability(mentorClient);

        var operationCount = 10;
        var stopwatch = Stopwatch.StartNew();

        // Act - Perform rapid session operations
        var sessionIds = new List<int>();
        
        // Book sessions rapidly
        for (int i = 0; i < operationCount; i++)
        {
            var scheduledAt = MentorshipTestData.Sessions.NextWeekMonday.AddDays(i % 5).AddHours(9 + i % 8);
            var sessionId = await BookSession(
                menteeClient, 
                mentorData.UserSlug.ToString(), 
                scheduledAt, 
                60, 
                $"Stress test session {i + 1}");
            sessionIds.Add(sessionId);
        }

        // Confirm sessions rapidly
        var confirmTasks = sessionIds.Select(sessionId => 
            ConfirmSessionAsMentor(mentorClient, sessionId)).ToArray();
        await Task.WhenAll(confirmTasks);

        // Add reviews rapidly
        var reviewTasks = sessionIds.Select((sessionId, index) => 
            AddSessionReview(
                menteeClient, 
                sessionId, 
                4 + (index % 2), 
                $"Stress test review {index + 1}")).ToArray();
        var reviewIds = await Task.WhenAll(reviewTasks);

        stopwatch.Stop();

        // Assert
        Assert.Equal(operationCount, sessionIds.Count);
        Assert.Equal(operationCount, reviewIds.Length);
        Assert.All(sessionIds, sessionId => Assert.True(sessionId > 0));
        Assert.All(reviewIds, reviewId => Assert.True(reviewId > 0));

        // Performance assertion
        Assert.True(stopwatch.ElapsedMilliseconds < 30000, 
            $"Stress test session management took {stopwatch.ElapsedMilliseconds}ms, expected < 30000ms");
    }
}
