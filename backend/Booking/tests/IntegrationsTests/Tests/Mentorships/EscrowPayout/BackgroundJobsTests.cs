/*using Booking.Modules.Mentorships.BackgroundJobs.Escrow;
using Booking.Modules.Mentorships.BackgroundJobs.Payout;
using Booking.Modules.Mentorships.Persistence;
using Booking.Modules.Mentorships.Domain.Entities;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationsTests.Tests.Mentorships.EscrowPayout;

public class BackgroundJobsTests : MentorshipTestBase
{
    public BackgroundJobsTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task EscrowJob_ShouldProcessCompletedSessions()
    {
        // Arrange - Create escrows for completed sessions
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        var escrowJob = scope.ServiceProvider.GetRequiredService<EscrowJob>();

        // Create test data directly in database
        var escrow1 = new Escrow(85.0m, 1, 1);
        var escrow2 = new Escrow(127.5m, 2, 2);
        
        await dbContext.Escrows.AddRangeAsync(escrow1, escrow2);
        await dbContext.SaveChangesAsync();

        // Act
        await escrowJob.ExecuteAsync(null);

        // Assert - Check that escrows are processed based on your business logic
        await dbContext.Entry(escrow1).ReloadAsync();
        await dbContext.Entry(escrow2).ReloadAsync();
        
        // Verify escrow processing based on your implementation
    }

    [Fact]
    public async Task PayoutJob_ShouldProcessApprovedPayouts()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        var payoutJob = scope.ServiceProvider.GetRequiredService<PayoutJob>();

        // Create approved payouts
        var payout1 = new Payout(1, "wallet1", 1, 85.0m);
        payout1.Approve("PAY_123");
        
        var payout2 = new Payout(2, "wallet2", 2, 127.5m);
        payout2.Approve("PAY_456");

        await dbContext.Payouts.AddRangeAsync(payout1, payout2);
        await dbContext.SaveChangesAsync();

        // Act
        await payoutJob.ExecuteAsync(null);

        // Assert - Verify payouts are processed
        await dbContext.Entry(payout1).ReloadAsync();
        await dbContext.Entry(payout2).ReloadAsync();
        
        // Check that Konnect payment was initiated (payment refs should be set)
        Assert.False(string.IsNullOrEmpty(payout1.PaymentRef));
        Assert.False(string.IsNullOrEmpty(payout2.PaymentRef));
    }

    [Fact]
    public async Task ManualJobExecution_ShouldWork_InIsolation()
    {
        // This test verifies that jobs can be triggered manually for testing
        using var scope = Factory.Services.CreateScope();
        
        var escrowJob = scope.ServiceProvider.GetRequiredService<EscrowJob>();
        var payoutJob = scope.ServiceProvider.GetRequiredService<PayoutJob>();

        // Act & Assert - Should not throw exceptions
        await escrowJob.ExecuteAsync(null);
        await payoutJob.ExecuteAsync(null);
        
        // Jobs should complete without errors even with no data to process
        Assert.True(true); // If we reach here, jobs executed successfully
    }
}*/