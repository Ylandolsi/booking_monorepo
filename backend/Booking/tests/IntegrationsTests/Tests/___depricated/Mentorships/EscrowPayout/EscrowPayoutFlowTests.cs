/*using System.Net.Http.Json;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationsTests.Tests.___depricated.Mentorships.EscrowPayout;

public class EscrowPayoutFlowTests : MentorshipTestBase
{
    public EscrowPayoutFlowTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Helper Methods

    private async Task AddBalanceToUser(string userId, decimal amount)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();

        var wallet = await dbContext.Wallets.FirstOrDefaultAsync(m => m.UserId.ToString() == userId);
        if (wallet != null)
        {
            wallet.UpdateBalance(amount);
            await dbContext.SaveChangesAsync();
        }
    }

    #endregion

    /*
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
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task PayoutRequest_ShouldFail_WhenAmountBelowMinimum()
    {
        var (mentorArrange, mentorAct) = await CreateMentor("mentor_min_amount", 100.0m, 15);

        var walletId = "test-wallet-123";
        await MentorshipTestUtilities.LinkKonnectWallet(mentorArrange, walletId);

        // minimum is $20
        var response = await MentorshipTestUtilities.RequestPayout(mentorAct, 10.0m);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task PayoutRequest_ShouldSucceed_WhenAmountBelowMaximumAndAboveMinimum()
    {
        var (mentorArrange, mentorAct) = await CreateMentor("mentor", 100.0m, 15);

        var walletId = "test-wallet-123";
        await MentorshipTestUtilities.LinkKonnectWallet(mentorArrange, walletId);

        string? mentorId = await MentorshipTestUtilities.GetMentorId(mentorArrange);
        await AddBalanceToUser(mentorId, 20000m);

        var response = await MentorshipTestUtilities.RequestPayout(mentorAct, 200);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task PayoutRequest_ShouldFail_WhenAmountAboveMaximum()
    {
        var (mentorArrange, mentorAct) = await CreateMentor("mentor-above-max", 100.0m, 15);

        var walletId = "test-wallet-123";
        await MentorshipTestUtilities.LinkKonnectWallet(mentorArrange, walletId);

        string? mentorId = await MentorshipTestUtilities.GetMentorId(mentorArrange);
        await AddBalanceToUser(mentorId, 20000m);

        var response = await MentorshipTestUtilities.RequestPayout(mentorAct, 1001);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task UserCanSeePayoutHistory()
    {
        // This test will be enabled when payout history endpoints are implemented
    }



    #endregion
    #1#

    #region AdminPayout

    [Fact]
    public async Task AdminAcceptPayout_ShouldReduceBalanceAndUpdateHistory()
    {
        var (adminArrange, adminAct) = await CreateAdmin("admin");
        var (mentorArrange, mentorAct) = await CreateMentor("mentor", 100m);

        var mentorSlug = await MentorshipTestUtilities.GetUserSlug(mentorArrange);
        var mentorFullInfo = await GetFullUserInfoBySlug(mentorSlug);

        await AddBalanceToUser(mentorFullInfo.Id.ToString(), 200m);

        var walletBeforePayout = await MentorshipTestUtilities.GetUserWallet(Factory, mentorFullInfo.Id.ToString());

        await MentorshipTestUtilities.RequestPayout(mentorArrange, 100m);

        var allPendingPayouts = await adminArrange.GetAsync(MentorshipEndpoints.Payouts.Admin.GetAllPayouts);
        var walletDuringPayout =
            await MentorshipTestUtilities.GetUserWallet(Factory, mentorFullInfo.Id.ToString());

        Assert.Equal(100, walletDuringPayout.PendingBalance);

        var content = await allPendingPayouts.Content.ReadFromJsonAsync<List<PayoutResponse>>();
        Assert.NotNull(content);
        Assert.True(content.Count != 0);

        var approveRespone = await adminAct.PostAsJsonAsync(MentorshipEndpoints.Payouts.Admin.ApprovePayout,
            new
            {
                PayoutId = content[0].Id
            });

        var payUrl = await approveRespone.Content.ReadFromJsonAsync<ApprovePayoutAdminResponse>();
        Assert.NotNull(payUrl);
        var paymentRef = MentorshipTestUtilities.ExtractPaymentRefFromUrl(payUrl.PayUrl);
        await MentorshipTestUtilities.CompletePaymentViaMockKonnect(paymentRef, mentorArrange);
        await Task.Delay(5000);

        var walletAfterPayout =
            await MentorshipTestUtilities.GetUserWallet(Factory, mentorFullInfo.Id.ToString());

        Assert.Equal(walletBeforePayout.Balance - 100, walletAfterPayout.Balance);
        Assert.Equal(0, walletAfterPayout.PendingBalance);
    }

    [Fact]
    public async Task WhenAdminRejectsUserPayout_Balance_ShouldNotBeReduced_FromUser()
    {
        var (adminArrange, adminAct) = await CreateAdmin("admin");
        var (mentorArrange, mentorAct) = await CreateMentor("mentor", 100m);

        var mentorSlug = await MentorshipTestUtilities.GetUserSlug(mentorArrange);
        var mentorFullInfo = await GetFullUserInfoBySlug(mentorSlug);

        await AddBalanceToUser(mentorFullInfo.Id.ToString(), 200m);
        var walletBeforePayout = await MentorshipTestUtilities.GetUserWallet(Factory, mentorFullInfo.Id.ToString());


        await MentorshipTestUtilities.RequestPayout(mentorArrange, 100m);

        var walletDuringPayout =
            await MentorshipTestUtilities.GetUserWallet(Factory, mentorFullInfo.Id.ToString());

        Assert.Equal(100, walletDuringPayout.PendingBalance);

        var allPendingPayouts = await adminArrange.GetAsync(MentorshipEndpoints.Payouts.Admin.GetAllPayouts);

        var content = await allPendingPayouts.Content.ReadFromJsonAsync<List<PayoutResponse>>();
        Assert.NotNull(content);
        Assert.True(content.Count != 0);

        var approveRespone = await adminAct.PostAsJsonAsync(MentorshipEndpoints.Payouts.Admin.RejectPayout,
            new
            {
                PayoutId = content[0].Id
            });

        var walletAfterPayout =
            await MentorshipTestUtilities.GetUserWallet(Factory, mentorFullInfo.Id.ToString());

        Assert.Equal(walletBeforePayout.Balance, walletAfterPayout.Balance);
        Assert.Equal(0, walletAfterPayout.PendingBalance);
    }

    #endregion
}*/