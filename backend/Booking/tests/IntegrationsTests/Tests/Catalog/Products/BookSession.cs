using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Booking.Modules.Catalog.Features;
using Booking.Modules.Catalog.Persistence;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationsTests.Tests.Catalog.Products;

public class BookSessionTests : CatalogTestBase
{
    public BookSessionTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CompleteSessionBookingFlow_ShouldSucceed_WithPaymentAndCalendarIntegrationCatalog()
    {
        var sessionPrice = 120.0m;
        var expectedEscrowAmount = sessionPrice * 0.85m; // 15% platform fee

        var (userArrange, userAct) = GetClientsForUser("user_create_session_product");
        var unauthClient = Factory.CreateClient();
        await CreateUserAndLogin("user_create_session_product@example.com", null, userArrange);

        await CatalogTestUtilities.CreateStoreForUser(userAct, "Test Store", "test-store");

        var dayAvailabilities = CatalogTestUtilities.SessionProductTestData.CreateCustomDayAvailabilities(
            (DayOfWeek.Monday, true, new[] { ("09:00", "12:00"), ("14:00", "17:00") }),
            (DayOfWeek.Wednesday, true, new[] { ("10:00", "16:00") })
        );

        var sessionProductSlug =
            await CatalogTestUtilities.CreateSessionProductForUser(userAct, "Test Session", 100.0m, 15,
                "Test subtitle",
                "Test description",
                "Book now",
                dayAvailabilities);


        var nextMonday = MentorshipTestUtilities.GetNextWeekday(DayOfWeek.Monday);


        var bookingRequest = new
        {
            Date = nextMonday.ToString("yyyy-MM-dd"),
            // YYYY-MM-DD, // TODO : maybe pass a Date type instead of ? 
            StartTime = "10:00", // TIMEONLY  
            EndTime = "11:00",
            Title = "Title tst",
            Email = "yesslandolsi@gmail.com",
            Name = "yassine",
            Phone = "25202909",
            TimeZoneId = "Africa/Tunis",
            Note = ""
        };

        // Act - Step 1: Book the session
        var bookingResponse =
            await unauthClient.PostAsJsonAsync(CatalogEndpoints.Sessions.Book + $"?productSlug={sessionProductSlug}",
                bookingRequest);

        // Assert - Booking should succeed and return payment URL
        Assert.Equal(HttpStatusCode.OK, bookingResponse.StatusCode);
        var bookingResult = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(bookingResult.TryGetProperty("payUrl", out var payUrl));
        Assert.False(string.IsNullOrEmpty(payUrl.GetString()));
        var paymentRef = MentorshipTestUtilities.ExtractPaymentRefFromUrl(payUrl.GetString()!);
        Assert.False(string.IsNullOrEmpty(paymentRef));

        // Verify session is created with WaitingForPayment status
        var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        
        var latestSession = await dbContext.BookedSessions.FirstOrDefaultAsync(s => s.ProductSlug ==  sessionProductSlug);
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.ProductId == latestSession.ProductId);
        Assert.Equal(SessionStatus.WaitingForPayment ,latestSession.Status);

        // Act - Step 2: Complete payment via mock Konnect
        var paymentResponse = await CompletePaymentViaMockKonnect(paymentRef, userAct);
        Assert.True(paymentResponse.success);

        // Wait a bit for webhook processing
        await Task.Delay(100000);
        scope = Factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        latestSession = await dbContext.BookedSessions.FirstOrDefaultAsync(s => s.ProductSlug ==  sessionProductSlug);
        order = await dbContext.Orders.FirstOrDefaultAsync(o => o.ProductId == latestSession.ProductId);
        // Assert - Session should be confirmed with meeting link and escrow created
        Assert.Equal(SessionStatus.Confirmed,latestSession.Status);
        
        /// TOOD :
        //var escrow = //
        //await MentorshipTestUtilities.VerifyEscrowCreated(Factory, sessionId, expectedEscrowAmount);
    }

    public static async Task<dynamic> CompletePaymentViaMockKonnect(string paymentRef, HttpClient client)
    {
        if (paymentRef == "paid")
            return new
            {
                success = true,
                error = (string?)null
            };

        var paymentRequest = new { paymentMethod = "card" };

        var response = await client.PostAsJsonAsync($"v2/process-payment/{paymentRef}", paymentRequest);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();

        return new
        {
            success = result.TryGetProperty("success", out var success) && success.GetBoolean(),
            error = result.TryGetProperty("error", out var error) ? error.GetString() : null
        };
    }
}