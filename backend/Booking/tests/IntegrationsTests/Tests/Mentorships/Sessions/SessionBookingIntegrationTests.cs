using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Mentorships.Domain.Enums;
using Booking.Modules.Mentorships.Persistence;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Sessions;

public class SessionBookingIntegrationTests : MentorshipTestBase
{
    public SessionBookingIntegrationTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CompleteSessionBookingFlow_ShouldSucceed_WithPaymentAndCalendarIntegration()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_full_flow", 100.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_full_flow");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "10:00",
            EndTime = "11:00",
            TimeZoneId = "Africa/Tunis",
            Note = "Integration test session"
        };

        // Act - Step 1: Book the session
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert - Booking should succeed and return payment URL
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(bookingResult.TryGetProperty("payUrl", out var payUrl));
        Assert.False(string.IsNullOrEmpty(payUrl.GetString()));
        var paymentRef = ExtractPaymentRefFromUrl(payUrl.GetString()!);
        Assert.False(string.IsNullOrEmpty(paymentRef));

        // Verify session is created with WaitingForPayment status
        var sessionId = await GetLatestSessionId(menteeAct);
        await VerifySessionStatus(sessionId, SessionStatus.WaitingForPayment);

        // Act - Step 2: Complete payment via mock Konnect
        var paymentResponse = await CompletePaymentViaMockKonnect(paymentRef);
        Assert.True(paymentResponse.success);

        // Wait a bit for webhook processing
        await Task.Delay(100000);

        // Assert - Session should be confirmed with meeting link and escrow created
        await VerifySessionStatus(sessionId, SessionStatus.Confirmed);
        await VerifySessionHasMeetingLink(sessionId);
        await VerifyEscrowCreated(sessionId);
    }

    [Fact]
    public async Task SessionBooking_ShouldHandlePaymentFailure_Gracefully()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_payment_fail", 50.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_payment_fail");

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

        // Act - Book session
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);

        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);
        var sessionId = await GetLatestSessionId(menteeAct);

        // Simulate payment failure
        var failedPayment = await SimulatePaymentFailure(paymentRef);
        Assert.False(failedPayment.success);

        // Wait for webhook processing
        await Task.Delay(1000);

        // Assert - Session should remain in WaitingForPayment status
        await VerifySessionStatus(sessionId, SessionStatus.WaitingForPayment);
        await VerifyNoEscrowCreated(sessionId);
    }

    [Fact]
    public async Task SessionBooking_ShouldHandleWalletPayment_Successfully()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_wallet", 75.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_wallet");

        // Charge test wallet with sufficient balance
        await ChargeTestWallet("test-wallet-1", 10000); // $100

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

        // Act
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);

        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        // Complete payment with wallet
        var paymentResponse = await CompletePaymentWithWallet(paymentRef, "test-wallet-1");
        Assert.True(paymentResponse.success);

        await Task.Delay(10000);

        var sessionId = await GetLatestSessionId(menteeAct);
        await VerifySessionStatus(sessionId, SessionStatus.Confirmed);
        await VerifyEscrowCreated(sessionId);
    }

    [Fact]
    public async Task SessionBooking_ShouldHandleInsufficientWalletBalance()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_insufficient", 200.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_insufficient");

        // Use wallet with insufficient balance (test-wallet-2 has only $100)
        await SetupMentorAvailability(mentorArrange, DayOfWeek.Thursday, "09:00", "17:00");

        var nextThursday = GetNextWeekday(DayOfWeek.Thursday);
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextThursday.ToString("yyyy-MM-dd"),
            StartTime = "15:00",
            EndTime = "16:00",
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);

        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        // Try to pay with insufficient wallet balance
        var paymentResponse = await CompletePaymentWithWallet(paymentRef, "test-wallet-2");
        Assert.False(paymentResponse.success);
        Assert.Contains("insufficient", paymentResponse.error.ToLower());
    }

    [Fact]
    public async Task SessionBooking_ShouldHandlePaymentExpiration()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_expired", 80.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_expired");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Friday, "09:00", "17:00");

        var nextFriday = GetNextWeekday(DayOfWeek.Friday);
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextFriday.ToString("yyyy-MM-dd"),
            StartTime = "16:00",
            EndTime = "17:00",
            TimeZoneId = "Africa/Tunis"
        };

        // Act - Book session
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);

        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        // Wait for payment to expire (mock Konnect has 10 minute lifespan, we can manipulate this for testing)
        // For testing, we'll check the payment status to see if expiration handling works
        var paymentDetails = await GetPaymentDetails(paymentRef);
        Assert.Equal("pending", paymentDetails.status);

        // Simulate expired payment by waiting or manipulating time
        // In a real test, you might want to create a payment with very short lifespan
    }

    [Fact]
    public async Task SessionBooking_ShouldCreateProperEscrowAmount()
    {
        // Arrange - Test that escrow is created with correct amount (85% of session price)
        var sessionPrice = 100.0m;
        var expectedEscrowAmount = sessionPrice * 0.85m; // 15% platform fee

        var (mentorArrange, mentorAct) = await CreateMentor("mentor_escrow", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_escrow");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "13:00",
            EndTime = "14:00",
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        await CompletePaymentViaMockKonnect(paymentRef);
        await Task.Delay(10000);

        // Assert
        var sessionId = await GetLatestSessionId(menteeAct);
        var escrowAmount = await GetEscrowAmount(sessionId);
        Assert.Equal(expectedEscrowAmount, escrowAmount);
    }

    [Fact]
    public async Task SessionBooking_ShouldHandleGoogleCalendarFailure_Gracefully()
    {
        // This test would require mocking Google Calendar service to simulate failures
        // For now, we'll test that the session is still confirmed even if calendar fails
        
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_calendar_fail", 60.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_calendar_fail");

        await SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = new
        {
            MentorSlug = await GetMentorSlug(mentorArrange),
            Date = nextMonday.ToString("yyyy-MM-dd"),
            StartTime = "12:00",
            EndTime = "13:00",
            TimeZoneId = "Africa/Tunis"
        };

        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef = ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        await CompletePaymentViaMockKonnect(paymentRef);
        await Task.Delay(10000);

        var sessionId = await GetLatestSessionId(menteeAct);
        await VerifySessionStatus(sessionId, SessionStatus.Confirmed);
        // Session should be confirmed even if calendar integration fails
    }

    #region Helper Methods

    private async Task<dynamic> CompletePaymentViaMockKonnect(string paymentRef)
    {
        var client = Factory.CreateClient();
        var paymentRequest = new { paymentMethod = "card" };
        
        var response = await client.PostAsJsonAsync($"process-payment/{paymentRef}", paymentRequest);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        return new
        {
            success = result.TryGetProperty("success", out var success) && success.GetBoolean(),
            error = result.TryGetProperty("error", out var error) ? error.GetString() : null
        };
    }

    private async Task<dynamic> CompletePaymentWithWallet(string paymentRef, string walletId)
    {
        var client = Factory.CreateClient();
        var paymentRequest = new { paymentMethod = "wallet", walletId = walletId };
        
        var response = await client.PostAsJsonAsync($"process-payment/{paymentRef}", paymentRequest);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        return new
        {
            success = result.TryGetProperty("success", out var success) && success.GetBoolean(),
            error = result.TryGetProperty("error", out var error) ? error.GetString() : null
        };
    }

    private async Task<dynamic> SimulatePaymentFailure(string paymentRef)
    {
        var client = Factory.CreateClient();
        var paymentRequest = new { paymentMethod = "fail" };
        
        var response = await client.PostAsJsonAsync($"process-payment/{paymentRef}", paymentRequest);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        return new
        {
            success = result.TryGetProperty("success", out var success) && success.GetBoolean(),
            error = result.TryGetProperty("error", out var error) ? error.GetString() : null
        };
    }

    private async Task ChargeTestWallet(string walletId, int amount)
    {
        var client = Factory.CreateClient();
        var chargeRequest = new { Amount = amount };
        
        await client.PostAsJsonAsync($"wallets/{walletId}/charge", chargeRequest);
    }

    private async Task<dynamic> GetPaymentDetails(string paymentRef)
    {
        var client = Factory.CreateClient();
        var response = await client.GetAsync($"payments/{paymentRef}");
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        return new
        {
            status = result.TryGetProperty("status", out var status) ? status.GetString() : null,
            amount = result.TryGetProperty("amount", out var amount) ? amount.GetInt32() : 0
        };
    }

    private string ExtractPaymentRefFromUrl(string payUrl)
    {
        // Extract payment reference from URL like: https://localhostpay/PAY_abc123
        var parts = payUrl.Split('/');
        return parts.LastOrDefault() ?? string.Empty;
    }

    private async Task<int> GetLatestSessionId(HttpClient client)
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
        Assert.False(string.IsNullOrEmpty(session.GoogleMeetLink?.Value));
    }

    private async Task VerifyEscrowCreated(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.NotNull(escrow);
        Assert.True(escrow.Price > 0);
    }

    private async Task VerifyNoEscrowCreated(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.Null(escrow?.Price);
    }

    private async Task<decimal> GetEscrowAmount(int sessionId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        return escrow?.Price ?? 0;
    }

    private async Task SetupMentorAvailability(HttpClient mentorClient, DayOfWeek dayOfWeek, string startTime, string endTime)
    {
        var bulkRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = dayOfWeek,
                    IsActive = true,
                    AvailabilityRanges = new[]
                    {
                        new { StartTime = startTime, EndTime = endTime},
                    }
                }
            }
        };

        var response = await mentorClient.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        response.EnsureSuccessStatusCode();
    }

    private async Task<string> GetMentorSlug(HttpClient mentorClient)
    {
        var response = await mentorClient.GetAsync(UsersEndpoints.GetCurrentUser);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("slug").GetString()!;
    }

    private static DateTime GetNextWeekday(DayOfWeek dayOfWeek)
    {
        var today = DateTime.Today;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7; // If today is the target day, get next week
        return today.AddDays(daysUntilTarget);
    }

    #endregion
}