using System.Net;
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
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.EscrowPayout;

public class EscrowPayoutFlowTests : MentorshipTestBase
{
    public EscrowPayoutFlowTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Escrow Flow Tests

    [Fact]
    public async Task CompleteEscrowFlow_ShouldCreateEscrow_AfterSessionCompleted()
    {
        // Arrange
        var sessionPrice = 120.0m;
        var expectedEscrowAmount = sessionPrice * 0.85m; // 15% platform fee
        
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_escrow_flow", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_escrow_flow");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextMonday();
        var sessionId = await MentorshipTestUtilities.BookAndCompleteSession(mentorArrange, menteeAct, nextMonday);

        await MentorshipTestUtilities.VerifyEscrowCreated(Factory, sessionId, expectedEscrowAmount);
    }

    [Fact]
    public async Task EscrowShouldNotBeReleased_AfterLessThan24HoursOfSessionCompletion()
    {
        // Arrange
        var sessionPrice = 100.0m;
        var expectedEscrowAmount = sessionPrice * 0.85m;
        
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_escrow_early", sessionPrice, 15);
        var (menteeArrange, menteeAct) = await CreateMentee("mentee_escrow_early");

        await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

        var nextMonday = MentorshipTestUtilities.GetNextMonday();
        var sessionId = await MentorshipTestUtilities.BookAndCompleteSession(mentorArrange, menteeAct, nextMonday);

        // Act - Mark session as completed but don't wait 24 hours
        await MentorshipTestUtilities.MarkSessionAsCompleted(sessionId, mentorArrange, menteeAct);
        
        // Trigger escrow job immediately (before 24 hours)
        await MentorshipTestUtilities.TriggerEscrowJob(Factory);

        // Escrow should NOT be released yet
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var escrow = await dbContext.Escrows.FirstOrDefaultAsync(e => e.SessionId == sessionId);
        Assert.NotNull(escrow);
        Assert.Equal(EscrowState.Held, escrow.State);
    }

    [Fact]
    public async Task EscrowAmount_ShouldBe85Percent_OfSessionPrice()
    {
        // Arrange - Test different session prices
        var testCases = new decimal[]
        {
            50.0m,
            100.0m,
            150.0m,
            200.0m,
        };

        foreach (var testCase in testCases)
        {
            var (mentorArrange, mentorAct) = await CreateMentor($"mentor_price_{testCase}", testCase, 15);
            var (menteeArrange, menteeAct) = await CreateMentee($"mentee_price_{testCase}");

            await MentorshipTestUtilities.SetupMentorAvailability(mentorArrange, DayOfWeek.Monday, "09:00", "17:00");

            var nextMonday = MentorshipTestUtilities.GetNextMonday();
            var sessionId = await MentorshipTestUtilities.BookAndCompleteSession(mentorArrange, menteeAct, nextMonday);

            // Assert - Verify correct escrow amount (85% of session price)
            await MentorshipTestUtilities.VerifyEscrowCreated(Factory, sessionId, testCase * 0.85m);
        }
    }

    #endregion

    #region Payout Request Tests

    [Fact]
    public async Task PayoutRequest_ShouldFail_WhenKonnectWalletNotLinked()
    {
        // Arrange
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_no_wallet", 100.0m, 15);
        
        // Try to request payout without linking Konnect wallet
        var response = await MentorshipTestUtilities.RequestPayout(mentorAct, 50.0m);
        
        // Assert - Should fail with appropriate error
        // Note: Exact status code may vary based on implementation
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PayoutRequest_ShouldFail_WhenAmountBelowMinimum()
    {
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_min_amount", 100.0m, 15);
        
        var walletId = "test-wallet-123";
        await MentorshipTestUtilities.LinkKonnectWallet(mentorArrange, walletId);
        
        // minimum is $20
        var response = await MentorshipTestUtilities.RequestPayout(mentorAct, 10.0m );
        
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.UnprocessableEntity);
    }

    // Note: The following tests are placeholders for when admin payout management and user balance tracking are fully implemented
    // They demonstrate the intended test structure but may need to be updated based on actual implementation
    
    /*
    [Fact]
    public async Task AdminAcceptPayout_ShouldReduceBalanceAndUpdateHistory()
    {
        // This test will be enabled when admin endpoints and balance tracking are implemented
    }

    [Fact] 
    public async Task UserCanSeePayoutHistory()
    {
        // This test will be enabled when payout history endpoints are implemented
    }
    */

    #endregion

    #region Helper Methods

    private async Task AddBalanceToMentor(string mentorId, decimal amount)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        
        var mentor = await dbContext.Mentors.FirstOrDefaultAsync(m => m.Id.ToString() == mentorId);
        if (mentor != null)
        {
            // Note: Balance property doesn't exist yet in Mentor entity
            // This is a placeholder for when balance tracking is implemented
            // mentor.Balance += amount;
            await dbContext.SaveChangesAsync();
        }
    }

    #endregion
}