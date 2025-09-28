using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Catalog.Features;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;

namespace IntegrationsTests.Tests.Catalog.Stores;

public class PrivateStore : CatalogTestBase
{
    public PrivateStore(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }


    #region Create Store Tests

    [Fact]
    public async Task CreateStore_ShouldSucceed_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_create_store");
        await CreateUserAndLogin("user_create_store@example.com", null, userArrange);

        // Act
        var response = await CatalogTestUtilities.CreateStoreRequest(userAct, "My Awesome Store", "my-awesome-store",
            "A great store description");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateStore_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();

        // Act
        var response = await CatalogTestUtilities.CreateStoreRequest(unauthClient, "Test Store", "test-store");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateStore_ShouldFail_WhenDuplicateSlug()
    {
        // Arrange
        var (user1Arrange, user1Act) = GetClientsForUser("user1_duplicate_slug");
        await CreateUserAndLogin("user1_duplicate_slug@example.com", null, user1Arrange);

        var (user2Arrange, user2Act) = GetClientsForUser("user2_duplicate_slug");
        await CreateUserAndLogin("user2_duplicate_slug@example.com", null, user2Arrange);

        var slug = "duplicate-store-slug";

        // Act
        var response1 = await CatalogTestUtilities.CreateStoreRequest(user1Act, "Store 1", slug);
        var response2 = await CatalogTestUtilities.CreateStoreRequest(user2Act, "Store 2", slug);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
    }

    [Fact]
    public async Task CreateStore_ShouldFail_WhenUserAlreadyHasStore()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_already_has_store");
        await CreateUserAndLogin("user_already_has_store@example.com", null, userArrange);

        // Act
        var response1 = await CatalogTestUtilities.CreateStoreRequest(userAct, "First Store", "first-store");
        var response2 = await CatalogTestUtilities.CreateStoreRequest(userAct, "Second Store", "second-store");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
    }


    [Fact]
    public async Task CreateStore_ShouldSucceed_WithSocialLinks()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_with_social_links");
        await CreateUserAndLogin("user_with_social_links@example.com", null, userArrange);

        // Act
        var response = await CatalogTestUtilities.CreateStoreRequest(
            userAct, "Store with Links", "store-with-links", "",
            new[] { ("twitter", "https://twitter.com/mystore"), ("instagram", "https://instagram.com/mystore") });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Get My Store Tests

    [Fact]
    public async Task GetMyStore_ShouldSucceed_WhenStoreExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_get_store");
        await CreateUserAndLogin("user_get_store@example.com", null, userArrange);
        await CatalogTestUtilities.CreateStoreForUser(userAct, "My Store", "my-store", "Store description");

        // Act
        var response = await userAct.GetAsync(CatalogEndpoints.Stores.GetMy);


        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMyStore_ShouldFail_WhenStoreDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_no_store");
        await CreateUserAndLogin("user_no_store@example.com", null, userArrange);

        // Act (don't create a store)
        var response = await userAct.GetAsync(CatalogEndpoints.Stores.GetMy);


        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMyStore_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();

        // Act
        var response = await unauthClient.GetAsync(CatalogEndpoints.Stores.GetMy);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion

    #region Update Store Tests

    [Fact]
    public async Task UpdateStore_ShouldSucceed_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_update_store");
        await CreateUserAndLogin("user_update_store@example.com", null, userArrange);
        await CatalogTestUtilities.CreateStoreForUser(userAct, "Original Store", "original-store");

        // Act
        var updateRequest =
            CatalogTestUtilities.StoreTestData.CreateStoreUpdateRequest("Updated Store Title",
                "Updated store description");
        var response = await userAct.PutAsJsonAsync(CatalogEndpoints.Stores.Update, updateRequest);
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateStore_ShouldFail_WhenStoreNotFound()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_update_nonexistent");
        await CreateUserAndLogin("user_update_nonexistent@example.com", null, userArrange);

        // Act
        var updateRequest =
            CatalogTestUtilities.StoreTestData.CreateStoreUpdateRequest("Updated Store Title",
                "Updated store description");
        var response = await userAct.PutAsJsonAsync(CatalogEndpoints.Stores.Update, updateRequest);
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateStore_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();

        // Act
        var updateRequest =
            CatalogTestUtilities.StoreTestData.CreateStoreUpdateRequest("Updated Store Title",
                "Updated store description");
        var response = await unauthClient.PutAsJsonAsync(CatalogEndpoints.Stores.Update, updateRequest);
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion

    #region Check Slug Availability Tests

    [Fact]
    public async Task CheckSlugAvailability_ShouldReturnTrue_WhenSlugIsAvailable()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_check_available_slug");
        await CreateUserAndLogin("user_check_available_slug@example.com", null, userArrange);
        var availableSlug = $"available-slug-{Guid.NewGuid():N}";

        // Act
        var response = await CatalogTestUtilities.CheckSlugAvailabilityRequest(userAct, availableSlug);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("true", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CheckSlugAvailability_ShouldReturnFalse_WhenSlugIsNotAvailable()
    {
        // Arrange
        var (user1Arrange, user1Act) = GetClientsForUser("user1_check_unavailable_slug");
        await CreateUserAndLogin("user1_check_unavailable_slug@example.com", null, user1Arrange);

        var (user2Arrange, user2Act) = GetClientsForUser("user2_check_unavailable_slug");
        await CreateUserAndLogin("user2_check_unavailable_slug@example.com", null, user2Arrange);

        var slug = "taken-slug";
        await CatalogTestUtilities.CreateStoreForUser(user1Act, "Test Store", slug);

        // Act
        var response = await CatalogTestUtilities.CheckSlugAvailabilityRequest(user2Act, slug);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("taken", content, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}