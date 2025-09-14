using System.Net;
using System.Net.Http.Json;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;
using Snapshooter.Xunit;

namespace IntegrationsTests.Tests.Catalog.Products;

public class SessionProductsTests : CatalogTestBase
{
    public SessionProductsTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Create Session Product Tests

    [Fact]
    public async Task CreateSessionProduct_ShouldSucceed_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_create_session_product");
        await CreateUserAndLogin(null, null, userArrange);

        // Create a store first
        await CreateStoreForUser(userAct, "Test Store", "test-store");

        var dayAvailabilities = new[]
        {
            new
            {
                DayOfWeek = DayOfWeek.Monday,
                IsActive = true,
                AvailabilityRanges = new[]
                {
                    new { StartTime = "09:00", EndTime = "12:00" },
                    new { StartTime = "14:00", EndTime = "17:00" }
                }
            },
            new
            {
                DayOfWeek = DayOfWeek.Wednesday,
                IsActive = true,
                AvailabilityRanges = new[]
                {
                    new { StartTime = "10:00", EndTime = "16:00" }
                }
            }
        };

        var sessionProductRequest = new
        {
            Title = "1-on-1 Coaching Session",
            Subtitle = "Personalized coaching",
            Description = "A detailed coaching session tailored to your needs",
            ClickToPay = "Book session now",
            Price = 100.0m,
            BufferTimeMinutes = 15,
            DayAvailabilities = dayAvailabilities,
            MeetingInstructions = "Join the Zoom meeting 5 minutes early",
            TimeZoneId = "Africa/Tunis"
        };

        // Act
        var response = await userAct.PostAsJsonAsync("/api/catalog/products/sessions", sessionProductRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.MatchSnapshot(matchOptions => matchOptions
            .IgnoreField("id")
            .IgnoreField("productSlug")
            .IgnoreField("createdAt"));
    }

    [Fact]
    public async Task CreateSessionProduct_ShouldFail_WhenUserHasNoStore()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_no_store_session_product");
        await CreateUserAndLogin(null, null, userArrange);

        var sessionProductRequest = new
        {
            Title = "1-on-1 Coaching Session",
            Subtitle = "Personalized coaching",
            Description = "A coaching session",
            ClickToPay = "Book now",
            Price = 100.0m,
            BufferTimeMinutes = 15,
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Monday,
                    IsActive = true,
                    AvailabilityRanges = new[]
                    {
                        new { StartTime = "09:00", EndTime = "12:00" }
                    }
                }
            }
        };

        // Act
        var response = await userAct.PostAsJsonAsync("/api/catalog/products/sessions", sessionProductRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateSessionProduct_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();
        var sessionProductRequest = new
        {
            Title = "Test Session",
            Price = 50.0m,
            BufferTimeMinutes = 15,
            DayAvailabilities = new object[] { }
        };

        // Act
        var response = await unauthClient.PostAsJsonAsync("/api/catalog/products/sessions", sessionProductRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData("", 50.0, 15, "Title cannot be empty")]
    [InlineData("Valid Title", -10.0, 15, "Price cannot be negative")]
    [InlineData("Valid Title", 50.0, -5, "Buffer time must be between 0 and 240 minutes")]
    [InlineData("Valid Title", 50.0, 300, "Buffer time must be between 0 and 240 minutes")]
    public async Task CreateSessionProduct_ShouldFail_WhenInvalidDataProvided(string title, decimal price, int bufferTime, string expectedErrorType)
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser($"user_validation_{Guid.NewGuid():N}");
        await CreateUserAndLogin(null, null, userArrange);
        await CreateStoreForUser(userAct, "Test Store", "test-store");

        var sessionProductRequest = new
        {
            Title = title,
            Subtitle = "Test subtitle",
            Description = "Test description",
            ClickToPay = "Book now",
            Price = price,
            BufferTimeMinutes = bufferTime,
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Monday,
                    IsActive = true,
                    AvailabilityRanges = new[]
                    {
                        new { StartTime = "09:00", EndTime = "12:00" }
                    }
                }
            }
        };

        // Act
        var response = await userAct.PostAsJsonAsync("/api/catalog/products/sessions", sessionProductRequest);

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
        await CreateUserAndLogin(null, null, userArrange);
        await CreateStoreForUser(userAct, "Test Store", "test-store");

        // Create a session product first
        var sessionProductId = await CreateSessionProductForUser(userAct, "Test Session", 100.0m);

        // Act
        var response = await userAct.GetAsync($"/api/catalog/products/sessions/{sessionProductId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.MatchSnapshot(matchOptions => matchOptions
            .IgnoreField("id")
            .IgnoreField("storeId")
            .IgnoreField("createdAt")
            .IgnoreField("updatedAt")
            .IgnoreField("availabilitySlots[*].id"));
    }

    [Fact]
    public async Task GetSessionProduct_ShouldFail_WhenProductNotFound()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_get_nonexistent_product");
        await CreateUserAndLogin(null, null, userArrange);

        // Act
        var response = await userAct.GetAsync("/api/catalog/products/sessions/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetSessionProduct_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();

        // Act
        var response = await unauthClient.GetAsync("/api/catalog/products/sessions/1");

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
        await CreateUserAndLogin(null, null, userArrange);
        await CreateStoreForUser(userAct, "Test Store", "test-store");

        // Create a session product first
        var sessionProductId = await CreateSessionProductForUser(userAct, "Original Session", 100.0m);

        var updateRequest = new
        {
            Title = "Updated Session Title",
            Price = 150.0m,
            DurationMinutes = 90,
            BufferTimeMinutes = 20,
            Subtitle = "Updated subtitle",
            Description = "Updated description",
            MeetingInstructions = "Updated instructions"
        };

        // Act
        var response = await userAct.PutAsJsonAsync($"/api/catalog/products/sessions/{sessionProductId}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.MatchSnapshot(matchOptions => matchOptions
            .IgnoreField("id")
            .IgnoreField("storeId")
            .IgnoreField("updatedAt"));
    }

    [Fact]
    public async Task UpdateSessionProduct_ShouldFail_WhenProductNotFound()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_update_nonexistent_product");
        await CreateUserAndLogin(null, null, userArrange);

        var updateRequest = new
        {
            Title = "Updated Title",
            Price = 150.0m,
            DurationMinutes = 90,
            BufferTimeMinutes = 20
        };

        // Act
        var response = await userAct.PutAsJsonAsync("/api/catalog/products/sessions/999", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateSessionProduct_ShouldFail_WhenUserDoesNotOwnProduct()
    {
        // Arrange
        var (user1Arrange, user1Act) = GetClientsForUser("user1_owns_product");
        var (user2Arrange, user2Act) = GetClientsForUser("user2_tries_update");

        await CreateUserAndLogin(null, null, user1Arrange);
        await CreateUserAndLogin(null, null, user2Arrange);

        await CreateStoreForUser(user1Act, "User1 Store", "user1-store");

        // Create product with user1
        var sessionProductId = await CreateSessionProductForUser(user1Act, "User1 Session", 100.0m);

        var updateRequest = new
        {
            Title = "Hacked Title",
            Price = 1.0m,
            DurationMinutes = 30,
            BufferTimeMinutes = 5
        };

        // Act - try to update with user2
        var response = await user2Act.PutAsJsonAsync($"/api/catalog/products/sessions/{sessionProductId}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion

    #region Helper Methods

    private async Task CreateStoreForUser(HttpClient userClient, string storeName, string storeSlug)
    {
        var storeData = new MultipartFormDataContent();
        storeData.Add(new StringContent(storeName), "FullName");
        storeData.Add(new StringContent(storeSlug), "Slug");
        storeData.Add(new StringContent("Test store description"), "Description");

        // Add dummy image
        var imageContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("dummy image"));
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        storeData.Add(imageContent, "file", "test.jpg");

        var response = await userClient.PostAsync("/api/catalog/stores", storeData);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private async Task<int> CreateSessionProductForUser(HttpClient userClient, string title, decimal price)
    {
        var dayAvailabilities = new[]
        {
            new
            {
                DayOfWeek = DayOfWeek.Monday,
                IsActive = true,
                AvailabilityRanges = new[]
                {
                    new { StartTime = "09:00", EndTime = "12:00" }
                }
            }
        };

        var sessionProductRequest = new
        {
            Title = title,
            Subtitle = "Test subtitle",
            Description = "Test description",
            ClickToPay = "Book now",
            Price = price,
            BufferTimeMinutes = 15,
            DayAvailabilities = dayAvailabilities,
            MeetingInstructions = "Test instructions",
            TimeZoneId = "Africa/Tunis"
        };

        var response = await userClient.PostAsJsonAsync("/api/catalog/products/sessions", sessionProductRequest);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonDoc = System.Text.Json.JsonDocument.Parse(responseContent);
        return jsonDoc.RootElement.GetProperty("id").GetInt32();
    }

    #endregion
}
