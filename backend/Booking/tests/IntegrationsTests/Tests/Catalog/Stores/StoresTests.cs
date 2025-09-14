using System.Net;
using System.Net.Http.Json;
using System.Text;
using Booking.Modules.Catalog.Features;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;
using Microsoft.AspNetCore.Http;
using Snapshooter.Xunit;

namespace IntegrationsTests.Tests.Catalog.Stores;

public class StoresTests : CatalogTestBase
{
    public StoresTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Create Store Tests

    [Fact]
    public async Task CreateStore_ShouldSucceed_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_create_store");
        await CreateUserAndLogin(null, null, userArrange);

        var storeData = CreateValidStoreFormData("My Awesome Store", "my-awesome-store", "A great store description");

        // Act
        var response = await userAct.PostAsync(CatalogEndpoints.Stores.Create, storeData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var storeResponse = System.Text.Json.JsonSerializer.Deserialize<dynamic>(responseContent);

        Assert.NotNull(storeResponse);

    }

    [Fact]
    public async Task CreateStore_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();
        var storeData = CreateValidStoreFormData("Test Store", "test-store");

        // Act
        var response = await unauthClient.PostAsync("/api/catalog/stores", storeData);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateStore_ShouldFail_WhenDuplicateSlug()
    {
        // Arrange
        var (user1Arrange, user1Act) = GetClientsForUser("user1_duplicate_slug");
        var (user2Arrange, user2Act) = GetClientsForUser("user2_duplicate_slug");

        await CreateUserAndLogin(null, null, user1Arrange);
        await CreateUserAndLogin(null, null, user2Arrange);

        var slug = "duplicate-store-slug";
        var store1Data = CreateValidStoreFormData("Store 1", slug);
        var store2Data = CreateValidStoreFormData("Store 2", slug);

        // Act
        var response1 = await user1Act.PostAsync("/api/catalog/stores", store1Data);
        var response2 = await user2Act.PostAsync("/api/catalog/stores", store2Data);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
    }

    [Fact]
    public async Task CreateStore_ShouldFail_WhenUserAlreadyHasStore()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_already_has_store");
        await CreateUserAndLogin(null, null, userArrange);

        var store1Data = CreateValidStoreFormData("First Store", "first-store");
        var store2Data = CreateValidStoreFormData("Second Store", "second-store");

        // Act
        var response1 = await userAct.PostAsync("/api/catalog/stores", store1Data);
        var response2 = await userAct.PostAsync("/api/catalog/stores", store2Data);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
    }

    [Theory]
    [InlineData("", "valid-slug", "Title cannot be empty")]
    [InlineData("Valid Title", "", "Slug cannot be empty")]
    [InlineData(null, "valid-slug", "Title cannot be empty")]
    [InlineData("Valid Title", null, "Slug cannot be empty")]
    public async Task CreateStore_ShouldFail_WhenRequiredFieldsAreMissing(string title, string slug, string expectedError)
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser($"user_validation_{Guid.NewGuid():N}");
        await CreateUserAndLogin(null, null, userArrange);

        var storeData = CreateValidStoreFormData(title, slug);

        // Act
        var response = await userAct.PostAsync("/api/catalog/stores", storeData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateStore_ShouldSucceed_WithSocialLinks()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_with_social_links");
        await CreateUserAndLogin(null, null, userArrange);

        var storeData = CreateStoreFormDataWithSocialLinks("Store with Links", "store-with-links",
            new[] {
                ("twitter", "https://twitter.com/mystore"),
                ("instagram", "https://instagram.com/mystore")
            });

        // Act
        var response = await userAct.PostAsync("/api/catalog/stores", storeData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        ;
    }

    #endregion

    #region Get My Store Tests

    [Fact]
    public async Task GetMyStore_ShouldSucceed_WhenStoreExists()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_get_store");
        await CreateUserAndLogin(null, null, userArrange);

        // Create a store first
        var storeData = CreateValidStoreFormData("My Store", "my-store", "Store description");
        var createResponse = await userAct.PostAsync("/api/catalog/stores", storeData);
        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

        // Act
        var response = await userAct.GetAsync("/api/catalog/stores/my-store");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);


    }

    [Fact]
    public async Task GetMyStore_ShouldFail_WhenStoreDoesNotExist()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_no_store");
        await CreateUserAndLogin(null, null, userArrange);

        // Act (don't create a store)
        var response = await userAct.GetAsync("/api/catalog/stores/my-store");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMyStore_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();

        // Act
        var response = await unauthClient.GetAsync("/api/catalog/stores/my-store");

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
        await CreateUserAndLogin(null, null, userArrange);

        // Create a store first
        var storeData = CreateValidStoreFormData("Original Store", "original-store");
        var createResponse = await userAct.PostAsync("/api/catalog/stores", storeData);
        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

        var updateRequest = new
        {
            Title = "Updated Store Title",
            Description = "Updated store description"
        };

        // Act
        var response = await userAct.PutAsJsonAsync("/api/catalog/stores/1", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();

    }

    [Fact]
    public async Task UpdateStore_ShouldFail_WhenStoreNotFound()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("user_update_nonexistent");
        await CreateUserAndLogin(null, null, userArrange);

        var updateRequest = new
        {
            Title = "Updated Store Title",
            Description = "Updated store description"
        };

        // Act
        var response = await userAct.PutAsJsonAsync("/api/catalog/stores/999", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateStore_ShouldFail_WhenUnauthenticated()
    {
        // Arrange
        var unauthClient = Factory.CreateClient();
        var updateRequest = new
        {
            Title = "Updated Store Title",
            Description = "Updated store description"
        };

        // Act
        var response = await unauthClient.PutAsJsonAsync("/api/catalog/stores/1", updateRequest);

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
        await CreateUserAndLogin(null, null, userArrange);

        var availableSlug = $"available-slug-{Guid.NewGuid():N}";

        // Act
        var response = await userAct.GetAsync($"/api/catalog/stores/slug-availability/{availableSlug}");

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
        var (user2Arrange, user2Act) = GetClientsForUser("user2_check_unavailable_slug");

        await CreateUserAndLogin(null, null, user1Arrange);
        await CreateUserAndLogin(null, null, user2Arrange);

        var slug = "taken-slug";
        var storeData = CreateValidStoreFormData("Test Store", slug);

        // Create store with the slug
        var createResponse = await user1Act.PostAsync("/api/catalog/stores", storeData);
        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

        // Act
        var response = await user2Act.GetAsync($"/api/catalog/stores/slug-availability/{slug}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("false", content, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region Helper Methods

    private static MultipartFormDataContent CreateValidStoreFormData(string title, string slug, string description = "")
    {
        var formData = new MultipartFormDataContent();

        formData.Add(new StringContent(title), "Title");
        formData.Add(new StringContent(slug), "Slug");
        formData.Add(new StringContent(description), "Description");

        // Add a minimal valid JPEG image (1x1 pixel)
        var jpegBytes = Convert.FromBase64String("/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCAABAAEDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/8QAFQEBAQAAAAAAAAAAAAAAAAAAAAX/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwA/AB//2Q==");
        var imageContent = new ByteArrayContent(jpegBytes);
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        formData.Add(imageContent, "File", "test-image.jpg");

        return formData;
    }

    private static MultipartFormDataContent CreateStoreFormDataWithSocialLinks(string title, string slug, (string platform, string url)[] socialLinks, string description = "")
    {
        var formData = CreateValidStoreFormData(title, slug, description);

        for (int i = 0; i < socialLinks.Length; i++)
        {
            formData.Add(new StringContent(socialLinks[i].platform), $"SocialLinks[{i}].Platform");
            formData.Add(new StringContent(socialLinks[i].url), $"SocialLinks[{i}].Url");
        }

        return formData;
    }

    #endregion
}