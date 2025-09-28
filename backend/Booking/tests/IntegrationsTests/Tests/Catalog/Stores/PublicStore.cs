using System.Net;
using System.Text.Json;
using Booking.Modules.Catalog.Features;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;

namespace IntegrationsTests.Tests.Catalog.Stores;

public class PublicStore : CatalogTestBase
{
    public PublicStore(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetPublicStore_ShouldReturnStore_WithProductDetails()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_create_session_product");
        await CreateUserAndLogin("user_create_session_product@example.com", null, userArrange);
        await CatalogTestUtilities.CreateStoreForUser(userArrange, "Test Store", "test-store");

        var dayAvailabilities = CatalogTestUtilities.SessionProductTestData.CreateCustomDayAvailabilities(
            (DayOfWeek.Monday, true, new[] { ("09:00", "12:00"), ("14:00", "17:00") }),
            (DayOfWeek.Wednesday, true, new[] { ("10:00", "16:00") })
        );

        await CatalogTestUtilities.CreateSessionProductRequest(
            userAct, "1-on-1 Coaching Session", 100.0m, 15, "Personalized coaching",
            "A detailed coaching session tailored to your needs", "clicktoPay", dayAvailabilities);

        var responsePublicStore =
            await userAct.GetAsync(CatalogEndpoints.Stores.GetPublic.Replace("{slug}", "test-store"));


        Assert.Equal(HttpStatusCode.OK, responsePublicStore.StatusCode);
        var content = await responsePublicStore.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(content);
        var root = jsonDoc.RootElement;

        Assert.True(root.TryGetProperty("title", out _), "Response should contain title");
        Assert.True(root.TryGetProperty("slug", out _), "Response should contain slug");
        Assert.True(root.TryGetProperty("products", out _), "Response should contain isPublished");
        Assert.True(root.TryGetProperty("socialLinks", out _), "Response should contain socialLinks");
        Assert.True(root.TryGetProperty("picture", out _), "Response should contain picture");
        Assert.True(root.TryGetProperty("description", out _), "Response should contain description");
    }
}