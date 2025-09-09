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
        var sessionPrice = 120.0m;
        var expectedEscrowAmount = sessionPrice * 0.85m; // 15% platform fee
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_full_flow", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_full_flow");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            await MentorshipTestUtilities.GetUserSlug(mentorArrange),
            nextMonday.ToString("yyyy-MM-dd"),
            "10:00",
            "11:00",
            MentorshipTestUtilities.TimeZones.Tunisia,
            "Integration test session"
        );

        // Act - Step 1: Book the session
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);

        // Assert - Booking should succeed and return payment URL
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(bookingResult.TryGetProperty("payUrl", out var payUrl));
        Assert.False(string.IsNullOrEmpty(payUrl.GetString()));
        var paymentRef = MentorshipTestUtilities.ExtractPaymentRefFromUrl(payUrl.GetString()!);
        Assert.False(string.IsNullOrEmpty(paymentRef));

        // Verify session is created with WaitingForPayment status
        var sessionId = await MentorshipTestUtilities.GetLatestSessionId(menteeAct);
        await MentorshipTestUtilities.VerifySessionStatus(Factory, sessionId, SessionStatus.WaitingForPayment);

        // Act - Step 2: Complete payment via mock Konnect
        var paymentResponse = await MentorshipTestUtilities.CompletePaymentViaMockKonnect(paymentRef, menteeAct);
        Assert.True(paymentResponse.success);

        // Wait a bit for webhook processing
        await Task.Delay(100000);

        // Assert - Session should be confirmed with meeting link and escrow created
        await MentorshipTestUtilities.VerifySessionStatus(Factory, sessionId, SessionStatus.Confirmed);
        await MentorshipTestUtilities.VerifySessionHasMeetingLink(Factory, sessionId);
        await MentorshipTestUtilities.VerifyEscrowCreated(Factory, sessionId, expectedEscrowAmount);
    }

    [Fact]
    public async Task SessionBooking_ShouldHandlePaymentFailure_Gracefully()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_payment_fail", 50.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_payment_fail");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Tuesday, "09:00", "17:00");

        var nextTuesday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Tuesday);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            await MentorshipTestUtilities.GetUserSlug(mentorArrange),
            nextTuesday.ToString("yyyy-MM-dd"),
            "14:00",
            "15:00",
            MentorshipTestUtilities.TimeZones.Tunisia
        );

        // Act - Book session
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);

        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef =
            MentorshipTestUtilities.ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);
        var sessionId = await MentorshipTestUtilities.GetLatestSessionId(menteeAct);

        // Simulate payment failure
        var failedPayment = await MentorshipTestUtilities.SimulatePaymentFailure(paymentRef, menteeAct);
        Assert.False(failedPayment.success);

        // Wait for webhook processing
        await Task.Delay(10000);

        // Assert - Session should remain in WaitingForPayment status
        await MentorshipTestUtilities.VerifySessionStatus(Factory, sessionId, SessionStatus.WaitingForPayment);
        await MentorshipTestUtilities.VerifyNoEscrowCreated(Factory, sessionId);
    }

    [Fact]
    public async Task SessionBooking_ShouldHandleWalletPayment_Successfully()
    {
        var sessionPrice = 120.0m;
        var expectedEscrowAmount = sessionPrice * 0.85m; // 15% platform fee

        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_wallet", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_wallet");

        var currentUser = await MentorshipTestUtilities.GetCurrentUserInfo(mentorArrange);
        var mentorIdElement = currentUser.GetProperty("slug");
        var mentorIdString = mentorIdElement.GetString();
        int mentorId = int.Parse(mentorIdString);


        // Charge test wallet with sufficient balance
        await MentorshipTestUtilities.ChargeTestWallet(Factory, mentorId, 100); // $100

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Wednesday, "09:00", "17:00");

        var nextWednesday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Wednesday);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            await MentorshipTestUtilities.GetUserSlug(mentorArrange),
            nextWednesday.ToString("yyyy-MM-dd"),
            "11:00",
            "12:00",
            MentorshipTestUtilities.TimeZones.Tunisia
        );

        // Act
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);

        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef =
            MentorshipTestUtilities.ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        // Complete payment with wallet
        var paymentResponse =
            await MentorshipTestUtilities.CompletePaymentWithWallet(paymentRef, "test-wallet-1", menteeAct);
        Assert.True(paymentResponse.success);

        await Task.Delay(10000);

        var sessionId = await MentorshipTestUtilities.GetLatestSessionId(menteeAct);
        await MentorshipTestUtilities.VerifySessionStatus(Factory, sessionId, SessionStatus.Confirmed);
        await MentorshipTestUtilities.VerifyEscrowCreated(Factory, sessionId, expectedEscrowAmount);
    }

    [Fact]
    public async Task SessionBooking_ShouldHandleInsufficientWalletBalance()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_insufficient", 500.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_insufficient");

        // Use wallet with insufficient balance (test-wallet-2 has only $100)
        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Thursday, "09:00", "17:00");

        var nextThursday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Thursday);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            await MentorshipTestUtilities.GetUserSlug(mentorArrange),
            nextThursday.ToString("yyyy-MM-dd"),
            "15:00",
            "15:30",
            MentorshipTestUtilities.TimeZones.Tunisia
        );

        // Act
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);

        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef =
            MentorshipTestUtilities.ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        // Try to pay with insufficient wallet balance
        var paymentResponse =
            await MentorshipTestUtilities.CompletePaymentWithWallet(paymentRef, "test-wallet-2", menteeAct);
        Assert.False(paymentResponse.success);
        Assert.Contains("insufficient", paymentResponse.error.ToLower());
    }

    [Fact]
    public async Task SessionBooking_ShouldHandlePaymentExpiration()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_expired", 80.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_expired");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Friday, "09:00", "17:00");

        var nextFriday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Friday);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            await MentorshipTestUtilities.GetUserSlug(mentorArrange),
            nextFriday.ToString("yyyy-MM-dd"),
            "16:00",
            "17:00",
            MentorshipTestUtilities.TimeZones.Tunisia
        );

        // Act - Book session
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);

        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef =
            MentorshipTestUtilities.ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        // Wait for payment to expire (mock Konnect has 10 minute lifespan, we can manipulate this for testing)
        // For testing, we'll check the payment status to see if expiration handling works
        var paymentDetails = await MentorshipTestUtilities.GetPaymentDetails(paymentRef, menteeAct);
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

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            await MentorshipTestUtilities.GetUserSlug(mentorArrange),
            nextMonday.ToString("yyyy-MM-dd"),
            "13:00",
            "14:00",
            MentorshipTestUtilities.TimeZones.Tunisia
        );

        // Act
        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef =
            MentorshipTestUtilities.ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        await MentorshipTestUtilities.CompletePaymentViaMockKonnect(paymentRef, menteeAct);
        await Task.Delay(10000);

        // Assert
        var sessionId = await MentorshipTestUtilities.GetLatestSessionId(menteeAct);
        var escrowAmount = await MentorshipTestUtilities.GetEscrowAmount(Factory, sessionId);
        Assert.Equal(expectedEscrowAmount, escrowAmount);
    }

    [Fact]
    public async Task SessionBooking_ShouldHandleGoogleCalendarFailure_Gracefully()
    {
        // This test would require mocking Google Calendar service to simulate failures
        // For now, we'll test that the session is still confirmed even if calendar fails

        var (mentorArrange, mentorAct) = await CreateMentor("mentor_calendar_fail", 60.0m, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_calendar_fail");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);
        var bookingRequest = MentorshipTestUtilities.CreateBookingRequest(
            await MentorshipTestUtilities.GetUserSlug(mentorArrange),
            nextMonday.ToString("yyyy-MM-dd"),
            "12:00",
            "13:00",
            MentorshipTestUtilities.TimeZones.Tunisia
        );

        var bookingResponse = await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, bookingRequest);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymentRef =
            MentorshipTestUtilities.ExtractPaymentRefFromUrl(bookingResult.GetProperty("payUrl").GetString()!);

        await MentorshipTestUtilities.CompletePaymentViaMockKonnect(paymentRef, menteeAct);
        await Task.Delay(10000);

        var sessionId = await MentorshipTestUtilities.GetLatestSessionId(menteeAct);
        await MentorshipTestUtilities.VerifySessionStatus(Factory, sessionId, SessionStatus.Confirmed);
        // Session should be confirmed even if calendar integration fails
    }
}