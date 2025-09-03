 /*using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Mentorships.Domain.Enums;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Persistence;
using Booking.Modules.Mentorships.BackgroundJobs.Escrow;
using Booking.Modules.Mentorships.BackgroundJobs.Payout;
using IntegrationsTests.Abstractions.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Hangfire;

namespace IntegrationsTests.Tests.Mentorships.EscrowPayout;

public class EscrowPayoutFlowTests : MentorshipTestBase
{
    public EscrowPayoutFlowTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CompleteEscrowFlow_ShouldCreatePayoutRequest_WhenSessionCompleted()
    {
        // Arrange
        var sessionPrice = 120.0m;
        var expectedEscrowAmount = sessionPrice * 0.85m; // 15% platform fee
        
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_escrow_flow", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_escrow_flow");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        var sessionId = await BookAndCompleteSession(mentorAct, menteeAct, nextMonday);

        // Verify escrow is created with correct amount
        await VerifyEscrowCreated(sessionId, expectedEscrowAmount);

        // Act - Simulate session completion (mentor and mentee mark as completed)
        await MarkSessionAsCompleted(sessionId);

        // Manually trigger escrow job to process completed sessions
        await TriggerEscrowJob();

        // Assert - Should create payout request and release escrow
        await VerifyEscrowReleased(sessionId);
        await VerifyPayoutRequestCreated(await GetMentorId(mentorArrange), expectedEscrowAmount);
    }

    [Fact]
    public async Task AdminPayoutFlow_ShouldProcessPayoutCorrectly()
    {
        // Arrange
        var sessionPrice = 100.0m;
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_admin_payout", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_admin_payout");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "09:00", "17:00");

        var nextTuesday = GetNextWeekday(DayOfWeek.Tuesday);
        var sessionId = await BookAndCompleteSession(mentorAct, menteeAct, nextTuesday);

        await MarkSessionAsCompleted(sessionId);
        await TriggerEscrowJob();

        var payoutId = await GetLatestPayoutId();
        var adminClient = await CreateAdminClient();

        // Act - Admin approves payout
        var approveResponse = await adminClient.PostAsJsonAsync(
            MentorshipEndpoints.Payment.Admin.ApprovePayout, 
            new { payoutId = payoutId });

        // Assert
        Assert.Equal(HttpStatusCode.OK, approveResponse.StatusCode);
        await VerifyPayoutStatus(payoutId, PayoutStatus.Approved);

        // Manually trigger payout job to process approved payouts
        await TriggerPayoutJob();

        // Should have payment reference from Konnect (or mock)
        await VerifyPayoutHasPaymentReference(payoutId);
    }

    [Fact]
    public async Task AdminRejectPayout_ShouldRefundEscrow()
    {
        // Arrange
        var sessionPrice = 80.0m;
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_reject_payout", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_reject_payout");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Wednesday, "09:00", "17:00");

        var nextWednesday = GetNextWeekday(DayOfWeek.Wednesday);
        var sessionId = await BookAndCompleteSession(mentorAct, menteeAct, nextWednesday);

        await MarkSessionAsCompleted(sessionId);
        await TriggerEscrowJob();

        var payoutId = await GetLatestPayoutId();
        var adminClient = await CreateAdminClient();

        // Act - Admin rejects payout
        var rejectResponse = await adminClient.PostAsJsonAsync(
            MentorshipEndpoints.Payment.Admin.RejectPayout, 
            new { payoutId = payoutId });

        // Assert
        Assert.Equal(HttpStatusCode.OK, rejectResponse.StatusCode);
        await VerifyPayoutStatus(payoutId, PayoutStatus.Rejected);
        await VerifyEscrowRefunded(sessionId);
    }

    [Fact]
    public async Task GetAllPayouts_ShouldReturnCorrectData_ForAdmin()
    {
        // Arrange - Create multiple sessions and payouts
        var testCases = new[]
        {
            new { Price = 100.0m, MentorName = "mentor_payout_1" },
            new { Price = 150.0m, MentorName = "mentor_payout_2" },
            new { Price = 75.0m, MentorName = "mentor_payout_3" }
        };

        var sessionIds = new List<int>();

        foreach (var testCase in testCases)
        {
            var (mentorArrange, mentorAct) = await CreateMentor(testCase.MentorName, testCase.Price, 15);
            var (menteeArrange, menteeAct) = await CreateMentee($"mentee_{testCase.MentorName}");

            await SetupMentorAvailability(mentorArrange, DayOfWeek.Thursday, "09:00", "17:00");

            var nextThursday = GetNextWeekday(DayOfWeek.Thursday);
            var sessionId = await BookAndCompleteSession(mentorAct, menteeAct, nextThursday);
            sessionIds.Add(sessionId);

            await MarkSessionAsCompleted(sessionId);
        }

        await TriggerEscrowJob();

        // Act - Get all payouts as admin
        var adminClient = await CreateAdminClient();
        var response = await adminClient.GetAsync(MentorshipEndpoints.Payment.Admin.GetAllPayouts);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var payouts = await response.Content.ReadFromJsonAsync<JsonElement>();
        var payoutsArray = payouts.EnumerateArray().ToList();
        
        Assert.True(payoutsArray.Count >= testCases.Length);

        // Verify each payout has correct structure and status
        foreach (var payout in payoutsArray.Take(testCases.Length))
        {
            Assert.True(payout.TryGetProperty("id", out _));
            Assert.True(payout.TryGetProperty("amount", out _));
            Assert.True(payout.TryGetProperty("status", out var status));
            Assert.Equal("Pending", status.GetString());
        }
    }

    [Fact]
    public async Task PayoutWebhook_ShouldCompletePayoutFlow()
    {
        // Arrange
        var sessionPrice = 90.0m;
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_webhook_payout", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_webhook_payout");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Friday, "09:00", "17:00");

        var nextFriday = GetNextWeekday(DayOfWeek.Friday);
        var sessionId = await BookAndCompleteSession(mentorAct, menteeAct, nextFriday);

        await MarkSessionAsCompleted(sessionId);
        await TriggerEscrowJob();

        var payoutId = await GetLatestPayoutId();
        var adminClient = await CreateAdminClient();

        // Approve payout
        await adminClient.PostAsJsonAsync(
            MentorshipEndpoints.Payment.Admin.ApprovePayout, 
            new { payoutId = payoutId });

        await TriggerPayoutJob();

        var payout = await GetPayoutById(payoutId);

        // Act - Simulate Konnect payout webhook
        var webhookPayload = new
        {
            paymentRef = payout.PaymentRef,
            status = "completed",
            amount = (int)(payout.Amount * 100), // Convert to cents
            orderId = payoutId.ToString()
        };

        var client = Factory.CreateClient();
        var webhookResponse = await client.PostAsJsonAsync(
            MentorshipEndpoints.Payment.Admin.WebhookPayout, 
            webhookPayload);

        // Assert
        Assert.Equal(HttpStatusCode.OK, webhookResponse.StatusCode);
        await VerifyPayoutStatus(payoutId, PayoutStatus.Completed);
    }

    [Fact]
    public async Task EscrowFlow_ShouldHandleMultipleSessions_Correctly()
    {
        // Arrange - Create multiple sessions for the same mentor
        var sessionPrice = 110.0m;
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_multiple_sessions", sessionPrice, 15);
        
        var sessionIds = new List<int>();
        
        for (int i = 0; i < 3; i++)
        {
            var (menteeArrange, menteeAct) = await CreateMentee($"mentee_multi_{i}");
            await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

            var nextMonday = GetNextWeekday(DayOfWeek.Monday).AddDays(i * 7); // Different weeks
            var sessionId = await BookAndCompleteSession(mentorAct, menteeAct, nextMonday);
            sessionIds.Add(sessionId);

            await MarkSessionAsCompleted(sessionId);
        }

        // Act - Trigger escrow job
        await TriggerEscrowJob();

        // Assert - Should create separate escrows and payouts for each session
        foreach (var sessionId in sessionIds)
        {
            await VerifyEscrowReleased(sessionId);
        }

        var mentorId = await GetMentorId(mentorArrange);
        var mentorPayouts = await GetPayoutsForUser(mentorId);
        
        Assert.Equal(3, mentorPayouts.Count);
        
        var expectedAmount = sessionPrice * 0.85m;
        Assert.All(mentorPayouts, payout => Assert.Equal(expectedAmount, payout.Amount));
    }

    [Fact]
    public async Task EscrowRefund_ShouldHappenWhen_SessionNotCompleted()
    {
        // Arrange - Session that will be refunded due to no completion
        var sessionPrice = 95.0m;
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_refund", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_refund");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Saturday, "09:00", "17:00");

        var nextSaturday = GetNextWeekday(DayOfWeek.Saturday);
        var sessionId = await BookAndCompleteSession(mentorAct, menteeAct, nextSaturday);

        // Don't mark session as completed - simulate expired session
        await SimulateSessionExpiration(sessionId);

        // Act - Trigger escrow job (should refund expired sessions)
        await TriggerEscrowJob();

        // Assert - Escrow should be refunded
        await VerifyEscrowRefunded(sessionId);
        
        // Should not create payout request
        var mentorId = await GetMentorId(mentorArrange);
        var mentorPayouts = await GetPayoutsForUser(mentorId);
        Assert.Empty(mentorPayouts);
    }

    [Fact]
    public async Task WalletIntegration_ShouldWork_WithPayoutFlow()
    {
        // Arrange - Test wallet integration with payout
        var sessionPrice = 85.0m;
        var testWalletId = "test-mentor-wallet";
        
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_wallet_payout", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_wallet_payout");

        // Set up mentor's Konnect wallet
        await SetupMentorWallet(mentorArrange, testWalletId);

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Sunday, "09:00", "17:00");

        var nextSunday = GetNextWeekday(DayOfWeek.Sunday);
        var sessionId = await BookAndCompleteSession(mentorAct, menteeAct, nextSunday);

        await MarkSessionAsCompleted(sessionId);
        await TriggerEscrowJob();

        var payoutId = await GetLatestPayoutId();
        var adminClient = await CreateAdminClient();

        // Act - Admin approves and processes payout
        await adminClient.PostAsJsonAsync(
            MentorshipEndpoints.Payment.Admin.ApprovePayout, 
            new { payoutId = payoutId });

        await TriggerPayoutJob();

        // Assert - Payout should use the mentor's wallet
        var payout = await GetPayoutById(payoutId);
        Assert.Equal(testWalletId, payout.KonnectWalletId);
        Assert.False(string.IsNullOrEmpty(payout.PaymentRef));
    }

    #region Helper Methods

    private async Task<int> BookAndCompleteSession(HttpClient mentorAct, HttpClient menteeAct, DateTime sessionDate)
    {
        var mentorSlug = await GetMentorSlug(mentorAct);
        
        var bookingRequest = new
        {
            MentorSlug = mentorSlug,
            Date = sessionDate.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis"
        };

        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        // Complete payment
        await CompletePaymentViaMockKonnect(paymentRef);
        await Task.Delay(3000); // Wait for webhook processing

        return await GetLatestSessionId(menteeAct);
    }

    private async Task<dynamic> CompletePaymentViaMockKonnect(string paymentRef)
    {
        var client = Factory.CreateClient();
        var paymentRequest = new { paymentMethod = "card" };
        
        var response = await client.PostAsJsonAsync($"/api/mock-konnect/process-payment/{paymentRef}", paymentRequest);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        return new
        {
            success = result.TryGetProperty("success", out var success) && success.GetBoolean()
        };
    }

    private async Task TriggerEscrowJob()
    {
        using var scope = Factory.Services.CreateScope();
        var escrowJob = scope.ServiceProvider.GetRequiredService<EscrowJob>();
        
        await escrowJob.ExecuteAsync(null);
    }

    private async Task TriggerPayoutJob()
    {
        using var scope = Factory.Services.CreateScope();
        var payoutJob = scope.ServiceProvider.GetRequiredService<PayoutJob>();
        
        await payoutJob.ExecuteAsync(null);
    }

    private async Task MarkSessionAsCompleted(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var session = await dbContext.Sessions.FindAsync(sessionId);
        if (session != null)
        {
            // Simulate session completion - you might need to add a method to Session entity
            session.MarkAsCompleted(); // You'll need to implement this method
            await dbContext.SaveChangesAsync();
        }
    }

    private async Task SimulateSessionExpiration(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var session = await dbContext.Sessions.FindAsync(sessionId);
        if (session != null)
        {
            // Set session to past date to simulate expiration
            // You might need to adjust this based on your session completion logic
            await dbContext.SaveChangesAsync();
        }
    }

    private async Task SetupMentorWallet(HttpClient mentorClient, string walletId)
    {
        // This would typically involve setting the mentor's Konnect wallet ID
        // You might need to add an endpoint for this or simulate it in the database
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        // You might need to add wallet setup logic here based on your implementation
    }

    private async Task VerifyEscrowCreated(int sessionId, decimal expectedAmount)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.NotNull(escrow);
        Assert.Equal(expectedAmount, escrow.Price);
        Assert.Equal(EscrowState.Held, escrow.State);
    }

    private async Task VerifyEscrowReleased(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.NotNull(escrow);
        Assert.Equal(EscrowState.Released, escrow.State);
    }

    private async Task VerifyEscrowRefunded(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.NotNull(escrow);
        Assert.Equal(EscrowState.Refunded, escrow.State);
    }

    private async Task VerifyPayoutRequestCreated(int mentorId, decimal expectedAmount)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var payout = await dbContext.Payouts
            .Where(p => p.UserId == mentorId)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync();
            
        Assert.NotNull(payout);
        Assert.Equal(expectedAmount, payout.Amount);
        Assert.Equal(PayoutStatus.Pending, payout.Status);
    }

    private async Task VerifyPayoutStatus(int payoutId, PayoutStatus expectedStatus)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var payout = await dbContext.Payouts.FindAsync(payoutId);
        Assert.NotNull(payout);
        Assert.Equal(expectedStatus, payout.Status);
    }

    private async Task VerifyPayoutHasPaymentReference(int payoutId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var payout = await dbContext.Payouts.FindAsync(payoutId);
        Assert.NotNull(payout);
        Assert.False(string.IsNullOrEmpty(payout.PaymentRef));
    }

    private async Task<int> GetLatestPayoutId()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var payout = await dbContext.Payouts
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync();
            
        return payout?.Id ?? throw new InvalidOperationException("No payouts found");
    }

    private async Task<Payout> GetPayoutById(int payoutId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        return await dbContext.Payouts.FindAsync(payoutId) 
               ?? throw new InvalidOperationException($"Payout {payoutId} not found");
    }

    private async Task<List<Payout>> GetPayoutsForUser(int userId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        return await dbContext.Payouts
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    private async Task<HttpClient> CreateAdminClient()
    {
        // You'll need to implement admin authentication
        // This might involve creating an admin user and getting their JWT token
        var client = Factory.CreateClient();
        
        // Add admin authentication headers
        // You might need to adjust this based on your auth implementation
        var adminToken = await GetAdminAuthToken();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
            
        return client;
    }

    private async Task<string> GetAdminAuthToken()
    {
        // Implement admin token generation
        // This should create or authenticate an admin user and return JWT
        var client = Factory.CreateClient();
        
        var adminCredentials = new
        {
            Email = "admin@test.com",
            Password = "AdminPassword123!"
        };

        // You might need to create admin user first if not exists
        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", adminCredentials);
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        
        return loginResult.GetProperty("token").GetString()!;
    }

    // Reuse helper methods from previous tests
    private string ExtractPaymentRefFromUrl(string payUrl) => payUrl.Split('/').LastOrDefault() ?? string.Empty;

    private async Task<int> GetLatestSessionId(HttpClient client)
    {
        var response = await client.GetAsync(MentorshipEndpoints.Sessions.GetSessions);
        response.EnsureSuccessStatusCode();
        
        var sessions = await response.Content.ReadFromJsonAsync<JsonElement>();
        var sessionsArray = sessions.EnumerateArray().ToList();
        
        var latestSession = sessionsArray.Last();
        return latestSession.GetProperty("id").GetInt32();
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

    private async Task<int> GetMentorId(HttpClient mentorClient)
    {
        var response = await mentorClient.GetAsync(MentorshipEndpoints.Mentor.GetDetails);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("id").GetInt32();
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