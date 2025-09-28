/*using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Mentorships.Domain.Enums;
using Booking.Modules.Mentorships.Persistence;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationsTests.Tests.Mentorships.Sessions;

public class SessionWebhookTests : MentorshipTestBase
{
    public SessionWebhookTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task PaymentWebhook_ShouldConfirmSession_WhenPaymentCompleted()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_webhook", 90.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_webhook");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis"
        };

        // Book session
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);
        var sessionId = await GetLatestSessionId(menteeAct);

        // Act - Simulate webhook call directly
        var webhookPayload = new
        {
            paymentRef = paymentRef,
            status = "completed",
            amount = 9000, // $90 in cents
            orderId = sessionId.ToString()
        };

        var client = Factory.CreateClient();
        var webhookResponse = await client.PostAsJsonAsync(MentorshipEndpoints.Payment.Webhook, webhookPayload);

        // Assert
        Assert.Equal(HttpStatusCode.OK, webhookResponse.StatusCode);

        // Wait for background job processing
        await Task.Delay(3000);

        await VerifySessionStatus(sessionId, SessionStatus.Confirmed);
        await VerifySessionHasMeetingLink(sessionId);
        await VerifyEscrowCreated(sessionId);
    }

    [Fact]
    public async Task PaymentWebhook_ShouldHandleFailedPayment()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_webhook_fail", 70.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_webhook_fail");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "09:00", "17:00");

        var nextTuesday = GetNextWeekday(DayOfWeek.Tuesday);
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextTuesday.ToString("yyyy-MM-dd"),
            StartTime = "14:00",
            EndTime = "15:00",
            TimeZoneId = "Africa/Tunis"
        };

        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);
        var sessionId = await GetLatestSessionId(menteeAct);

        // Act - Simulate failed payment webhook
        var webhookPayload = new
        {
            paymentRef = paymentRef,
            status = "failed",
            amount = 7000,
            orderId = sessionId.ToString()
        };

        var client = Factory.CreateClient();
        var webhookResponse = await client.PostAsJsonAsync(MentorshipEndpoints.Payment.Webhook, webhookPayload);

        // Assert
        Assert.Equal(HttpStatusCode.OK, webhookResponse.StatusCode);

        await Task.Delay(1000);

        // Session should remain in WaitingForPayment status
        await VerifySessionStatus(sessionId, SessionStatus.WaitingForPayment);
        await VerifyNoEscrowCreated(sessionId);
    }

    [Fact]
    public async Task PaymentWebhook_ShouldHandleDuplicateWebhooks_Gracefully()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_duplicate", 80.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_duplicate");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Wednesday, "09:00", "17:00");

        var nextWednesday = GetNextWeekday(DayOfWeek.Wednesday);
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextWednesday.ToString("yyyy-MM-dd"),
            StartTime = "11:00",
            EndTime = "12:00",
            TimeZoneId = "Africa/Tunis"
        };

        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);
        var sessionId = await GetLatestSessionId(menteeAct);

        var webhookPayload = new
        {
            paymentRef = paymentRef,
            status = "completed",
            amount = 8000,
            orderId = sessionId.ToString()
        };

        var client = Factory.CreateClient();

        // Act - Send the same webhook multiple times
        var firstWebhookResponse = await client.PostAsJsonAsync(MentorshipEndpoints.Payment.Webhook, webhookPayload);
        await Task.Delay(1000);
        var secondWebhookResponse = await client.PostAsJsonAsync(MentorshipEndpoints.Payment.Webhook, webhookPayload);
        var thirdWebhookResponse = await client.PostAsJsonAsync(MentorshipEndpoints.Payment.Webhook, webhookPayload);

        // Assert - All webhooks should be accepted
        Assert.Equal(HttpStatusCode.OK, firstWebhookResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, secondWebhookResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, thirdWebhookResponse.StatusCode);

        await Task.Delay(2000);

        // Session should be confirmed only once
        await VerifySessionStatus(sessionId, SessionStatus.Confirmed);

        // Should have only one escrow record
        var escrowCount = await GetEscrowCountForSession(sessionId);
        Assert.Equal(1, escrowCount);
    }

    [Fact]
    public async Task PaymentWebhook_ShouldHandleInvalidPaymentRef()
    {
        // Act
        var invalidWebhookPayload = new
        {
            paymentRef = "INVALID_PAYMENT_REF",
            status = "completed",
            amount = 5000,
            orderId = "999"
        };

        var client = Factory.CreateClient();
        var webhookResponse = await client.PostAsJsonAsync(MentorshipEndpoints.Payment.Webhook, invalidWebhookPayload);

        // Assert - Should handle gracefully (might return 404 or 400)
        Assert.True(webhookResponse.StatusCode == HttpStatusCode.BadRequest ||
                   webhookResponse.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PaymentWebhook_ShouldCreateCorrectMeetingDetails()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_meeting", 120.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_meeting");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Thursday, "09:00", "17:00");

        var nextThursday = GetNextWeekday(DayOfWeek.Thursday);
        var sessionStartTime = "15:00";
        var sessionEndTime = "16:00";

        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextThursday.ToString("yyyy-MM-dd"),
            StartTime = sessionStartTime,
            EndTime = sessionEndTime,
            TimeZoneId = "Africa/Tunis",
            Note = "Test meeting creation"
        };

        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);
        var sessionId = await GetLatestSessionId(menteeAct);

        // Act
        var webhookPayload = new
        {
            paymentRef = paymentRef,
            status = "completed",
            amount = 12000,
            orderId = sessionId.ToString()
        };

        var client = Factory.CreateClient();
        await client.PostAsJsonAsync(MentorshipEndpoints.Payment.Webhook, webhookPayload);

        await Task.Delay(3000);

        // Assert
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var session = await dbContext.Sessions.FindAsync(sessionId);
        Assert.NotNull(session);
        Assert.Equal(SessionStatus.Confirmed, session.Status);

        // Verify meeting link is present and has correct format
        Assert.NotNull(session.MeetLink);
        Assert.False(string.IsNullOrEmpty(session.MeetLink.Value));

        // Meeting link should be a Google Meet link or placeholder
        Assert.True(session.MeetLink.Value.Contains("meet.google.com") ||
                   session.MeetLink.Value.Contains("placeholder"));

        // Verify session time matches booking request
        var expectedStartTime = DateTime.Parse($"{nextThursday:yyyy-MM-dd} {sessionStartTime}:00");
        // Note: You might need to adjust for timezone conversion here
    }

    [Fact]
    public async Task PaymentWebhook_ShouldCalculateEscrowCorrectly()
    {
        // Test various session prices to ensure escrow calculation is correct
        var testCases = new[]
        {
            new { SessionPrice = 100.0m, ExpectedEscrow = 85.0m },
            new { SessionPrice = 50.0m, ExpectedEscrow = 42.5m },
            new { SessionPrice = 200.0m, ExpectedEscrow = 170.0m },
            new { SessionPrice = 75.5m, ExpectedEscrow = 64.175m }
        };

        foreach (var testCase in testCases)
        {
            // Arrange
            var uniqueId = Guid.NewGuid().ToString("N")[..8];
            var (mentorArrange, mentorAct) = await CreateMentor($"mentor_escrow_{uniqueId}", testCase.SessionPrice, 15);
            var (menteeArrange, menteeAct) = await CreateMentee($"mentee_escrow_{uniqueId}");

            await SetupMentorAvailability(mentorArrange, DayOfWeek.Friday, "09:00", "17:00");

            var nextFriday = GetNextWeekday(DayOfWeek.Friday);
            var bookingRequest = new
            {
                MentorSlug = await GetMentorSlug(mentorArrange),
                Date = nextFriday.ToString("yyyy-MM-dd"),
                StartTime = "10:00",
                EndTime = "11:00",
                TimeZoneId = "Africa/Tunis"
            };

            var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
            var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
            var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);
            var sessionId = await GetLatestSessionId(menteeAct);

            // Act
            var webhookPayload = new
            {
                paymentRef = paymentRef,
                status = "completed",
                amount = (int)(testCase.SessionPrice * 100), // Convert to cents
                orderId = sessionId.ToString()
            };

            var client = Factory.CreateClient();
            await client.PostAsJsonAsync(MentorshipEndpoints.Payment.Webhook, webhookPayload);

            await Task.Delay(2000);

            // Assert
            var escrowAmount = await GetEscrowAmount(sessionId);
            Assert.Equal(testCase.ExpectedEscrow, escrowAmount);
        }
    }

    #region Helper Methods

    private string ExtractPaymentRefFromUrl(string payUrl)
    {
        var parts = payUrl.Split('/');
        return parts.LastOrDefault() ?? string.Empty;
    }

    private async Task<int> GetLatestSessionId(HttpClient client)
    {
        var response = await client.GetAsync(MentorshipEndpoints.Sessions.GetSessions);
        response.EnsureSuccessStatusCode();

        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        var sessionsArray = sessions.EnumerateArray().ToList();

        var latestSession = sessionsArray.Last();
        return latestSession.GetProperty("id").GetInt32();
    }

    private async Task VerifySessionStatus(int sessionId, SessionStatus expectedStatus)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var session = await dbContext.Sessions.FindAsync(sessionId);
        Assert.NotNull(session);
        Assert.Equal(expectedStatus, session.Status);
    }

    private async Task VerifySessionHasMeetingLink(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var session = await dbContext.Sessions.FindAsync(sessionId);
        Assert.NotNull(session);
        Assert.False(string.IsNullOrEmpty(session.MeetLink?.Value));
    }

    private async Task VerifyEscrowCreated(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.NotNull(escrow);
        Assert.True(escrow.Amount > 0);
    }

    private async Task VerifyNoEscrowCreated(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.Null(escrow);
    }

    private async Task<decimal> GetEscrowAmount(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        return escrow?.Amount ?? 0;
    }

    private async Task<int> GetEscrowCountForSession(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        return await dbContext.Escrows.CountAsync(e => e.SessionId == sessionId);
    }

    private async Task SetupMentorAvailability(HttpClient mentorClient, DayOfWeek dayOfWeek, string startTime, string endTime)
    {
        var availabilityRequest = new
        {
            DayOfWeek = (int)dayOfWeek,
            StartTime = startTime,
            EndTime = endTime
        };

        var response = await mentorClient.PostAsJsonAsync(MentorshipEndpoints.Schedule.SetSchedule, new
        {
            Availabilities = new[] { availabilityRequest }
        });

        response.EnsureSuccessStatusCode();
    }

    private async Task<string> GetMentorSlug(HttpClient mentorClient)
    {
        var response = await mentorClient.GetAsync(MentorshipEndpoints.Mentor.GetDetails);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("slug").GetString()!;
    }

    private static DateTime GetNextWeekday(DayOfWeek dayOfWeek)
    {
        var today = DateTime.Today;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7;
        return today.AddDays(daysUntilTarget);
    }

    #endregion
}*/

