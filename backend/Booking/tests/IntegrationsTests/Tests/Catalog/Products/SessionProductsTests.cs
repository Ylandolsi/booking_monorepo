using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Catalog.Features;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;
using Snapshooter.Xunit;
using Xunit.Abstractions;

namespace IntegrationsTests.Tests.Catalog.Products;

public class SessionProductsTests : CatalogTestBase
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SessionProductsTests(IntegrationTestsWebAppFactory factory, ITestOutputHelper testOutputHelper) :
        base(factory)
    {
        _testOutputHelper = testOutputHelper;
    }

    #region Create Session Product Tests

    [Fact]
    public async Task CreateSessionProduct_ShouldSucceed_WhenValidDataProvided()
    {
        // Arrange
        //var (userArrange, userAct) = GetClientsForUser("CreateSessionProduct_ShouldSucceed_WhenValidDataProvided");
        var httpclient = Factory.CreateClient();
        await CreateUserAndLogin(null, null, httpclient);
        await CatalogTestUtilities.CreateStoreForUser(httpclient, "Test Store", "test-store");

        var dayAvailabilities = CatalogTestUtilities.SessionProductTestData.CreateCustomDayAvailabilities(
            (DayOfWeek.Monday, true, new[] { ("09:00", "12:00"), ("14:00", "17:00") }),
            (DayOfWeek.Wednesday, true, new[] { ("10:00", "16:00") })
        );

        // Act
        var response = await CatalogTestUtilities.CreateSessionProductRequest(
            httpclient, "1-on-1 Coaching Session", 100.0m, 15, "Personalized coaching",
            "A detailed coaching session tailored to your needs", "clicktoPay", dayAvailabilities);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateSessionProduct_ShouldFail_WhenUserHasNoStore()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_no_store_session_product");
        await CreateUserAndLogin("user_no_store_session_product@example.com", null, userArrange);

        // Act
        var response = await CatalogTestUtilities.CreateSessionProductRequest(userAct, "Test Session", 100.0m);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateSessionProduct_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();

        // Act
        var response = await CatalogTestUtilities.CreateSessionProductRequest(unauthClient, "Test Session", 50.0m);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData("", 50.0, 15, "Title cannot be empty")]
    [InlineData("Valid Title", -10.0, 15, "Price cannot be negative")]
    [InlineData("Valid Title", 50.0, -5, "Buffer time must be between 0 and 240 minutes")]
    [InlineData("Valid Title", 50.0, 300, "Buffer time must be between 0 and 240 minutes")]
    public async Task CreateSessionProduct_ShouldFail_WhenInvalidDataProvided(string title, decimal price,
        int bufferTime, string expectedErrorType)
    {
        // Arrange
        var userKey = $"user_validation_{Guid.NewGuid():N}";
        var (userArrange, userAct) = GetClientsForUser(userKey);
        await CreateUserAndLogin($"{userKey}@example.com", null, userArrange);
        await CatalogTestUtilities.CreateStoreForUser(userAct, "Test Store", "test-store");

        // Act
        var response = await CatalogTestUtilities.CreateSessionProductRequest(
            userAct, title, price, bufferTime, "Test subtitle", "Test description",
            dayAvailabilities: CatalogTestUtilities.SessionProductTestData.CreateDefaultDayAvailabilities());

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Get Session Product Tests

    [Fact]
    public async Task GetSessionProduct_ShouldSucceed_WhenProductExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_get_session_product");
        await CreateUserAndLogin("user_get_session_product@example.com", null, userArrange);
        await CatalogTestUtilities.CreateStoreForUser(userAct, "Test Store", "test-store");
        var sessionProductSlug =
            await CatalogTestUtilities.CreateSessionProductForUser(userAct, "Test Session", 100.0m);

        // Act
        var response = await CatalogTestUtilities.GetSessionProductRequest(userAct, sessionProductSlug);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
    }

    [Fact]
    public async Task GetSessionProduct_ShouldFail_WhenProductNotFound()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_get_nonexistent_product");
        await CreateUserAndLogin("user_get_nonexistent_product@example.com", null, userArrange);

        // Act
        var response = await CatalogTestUtilities.GetSessionProductRequest(userAct, "no-found");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetSessionProduct_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();

        // Act
        var response = await CatalogTestUtilities.GetSessionProductRequest(unauthClient, "slug-slug");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion

    #region Update Session Product Tests

    [Fact]
    public async Task UpdateSessionProduct_ShouldSucceed_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_update_session_product");
        await CreateUserAndLogin("user_update_session_product@example.com", null, userArrange);
        await CatalogTestUtilities.CreateStoreForUser(userAct, "Test Store", "test-store");
        var sessionProductSlug =
            await CatalogTestUtilities.CreateSessionProductForUser(userAct, "Original Session", 100.0m);

        var dayAvailabilities = CatalogTestUtilities.SessionProductTestData.CreateCustomDayAvailabilities(
            (DayOfWeek.Monday, true, new[] { ("09:00", "12:00"), ("14:00", "17:00") }),
            (DayOfWeek.Wednesday, true, new[] { ("10:00", "16:00") })
        );

        // Act
        var response = await CatalogTestUtilities.UpdateSessionProductRequest(
            userAct, sessionProductSlug, "1-on-1 Coaching Session", 100.0m, 30, 15,
            "Personalized coaching", "A detailed coaching session tailored to your needs",
            "Join the Zoom meeting 5 minutes early", dayAvailabilities);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
    }

    [Fact]
    public async Task UpdateSessionProduct_ShouldFail_WhenProductNotFound()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_update_nonexistent_product");
        await CreateUserAndLogin("user_update_nonexistent_product@example.com", null, userArrange);

        var dayAvailabilities = CatalogTestUtilities.SessionProductTestData.CreateCustomDayAvailabilities(
            (DayOfWeek.Monday, true, new[] { ("09:00", "12:00"), ("14:00", "17:00") }),
            (DayOfWeek.Wednesday, true, new[] { ("10:00", "16:00") })
        );

        // Act
        var response = await CatalogTestUtilities.UpdateSessionProductRequest(
            userAct, "not-found-slug", "1-on-1 Coaching Session", 100.0m, 30, 15,
            "Personalized coaching", "A detailed coaching session", "Join the Zoom meeting 5 minutes early",
            dayAvailabilities);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateSessionProduct_ShouldFail_WhenUserDoesNotOwnProduct()
    {
        // Arrange
        var (user1Arrange, user1Act) = GetClientsForUser("user1_owns_product");
        await CreateUserAndLogin("user1_owns_product@example.com", null, user1Arrange);

        var (user2Arrange, user2Act) = GetClientsForUser("user2_tries_update");
        await CreateUserAndLogin("user2_tries_update@example.com", null, user2Arrange);

        await CatalogTestUtilities.CreateStoreForUser(user1Act, "User1 Store", "user1-store");
        var sessionProductSlug =
            await CatalogTestUtilities.CreateSessionProductForUser(user1Act, "User1 Session", 100.0m);

        var dayAvailabilities = CatalogTestUtilities.SessionProductTestData.CreateCustomDayAvailabilities(
            (DayOfWeek.Monday, true, new[] { ("09:00", "12:00"), ("14:00", "17:00") }),
            (DayOfWeek.Wednesday, true, new[] { ("10:00", "16:00") })
        );

        // Act - try to update with user2
        var response = await CatalogTestUtilities.UpdateSessionProductRequest(
            user2Act, sessionProductSlug, "1-on-1 Coaching Session", 100.0m, 30, 15,
            "Personalized coaching", "A detailed coaching session", "Join the Zoom meeting 5 minutes early",
            dayAvailabilities);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion
}