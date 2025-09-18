using System.Net;
using System.Net.Http.Json;
using System.Text;
using Booking.Modules.Catalog.Features;
using IntegrationsTests.Abstractions.Authentication;
using IntegrationsTests.Abstractions.Base;

namespace IntegrationsTests.Abstractions;

public static class CatalogTestUtilities
{
    public static class StoreTestData
    {
        public static MultipartFormDataContent CreateValidStoreFormData(
            string title,
            string slug,
            string description = "",
            (string platform, string url)[]? socialLinks = null)
        {
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(title), "Title");
            formData.Add(new StringContent(slug), "Slug");
            formData.Add(new StringContent(description), "Description");

            // Add social links if provided
            if (socialLinks != null)
            {
                for (int i = 0; i < socialLinks.Length; i++)
                {
                    formData.Add(new StringContent(socialLinks[i].platform), $"SocialLinks[{i}].Platform");
                    formData.Add(new StringContent(socialLinks[i].url), $"SocialLinks[{i}].Url");
                }
            }

            var jpegBytes = Convert.FromBase64String("/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCAABAAEDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/8QAFQEBAQAAAAAAAAAAAAAAAAAAAAX/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwA/AB//2Q==");
            var imageContent = new ByteArrayContent(jpegBytes);
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            formData.Add(imageContent, "File", "test-image.jpg");

            return formData;
        }

        public static object CreateStoreUpdateRequest(
            string title,
            string? description = null,
            (string platform, string url)[]? socialLinks = null)
        {
            var request = new Dictionary<string, object>
            {
                ["Title"] = title
            };

            if (description != null)
                request["Description"] = description;

            if (socialLinks != null)
            {
                request["SocialLinks"] = socialLinks.Select(sl => new { Platform = sl.platform, Url = sl.url }).ToArray();
            }

            return request;
        }
    }

    public static class SessionProductTestData
    {
        public static object CreateValidSessionProductRequest(
            string title,
            decimal price,
            int bufferTimeMinutes = 15,
            string subtitle = "Test subtitle",
            string description = "Test description",
            string clickToPay = "Book now",
            string meetingInstructions = "Test instructions",
            string timeZoneId = "Africa/Tunis",
            object[]? dayAvailabilities = null,
            int durationMinutes = 60)
        {
            return new
            {
                Title = title,
                Subtitle = subtitle,
                Description = description,
                ClickToPay = clickToPay,
                Price = price,
                DurationMinutes = durationMinutes,
                BufferTimeMinutes = bufferTimeMinutes,
                DayAvailabilities = dayAvailabilities ?? CreateDefaultDayAvailabilities(),
                MeetingInstructions = meetingInstructions,
                TimeZoneId = timeZoneId
            };
        }

        public static object CreateSessionProductUpdateRequest(
            string title,
            decimal price,
            int durationMinutes = 60,
            int bufferTimeMinutes = 15,
            string? subtitle = null,
            string? description = null,
            string? clickToPay = null,
            string? meetingInstructions = null,
            string? timeZoneId = null,
            object[]? dayAvailabilities = null)
        {
            var request = new Dictionary<string, object>
            {
                ["Title"] = title,
                ["Price"] = price,
                ["DurationMinutes"] = durationMinutes,
                ["BufferTimeMinutes"] = bufferTimeMinutes
            };

            if (subtitle != null) request["Subtitle"] = subtitle;
            if (description != null) request["Description"] = description;
            if (clickToPay != null) request["ClickToPay"] = clickToPay;
            if (meetingInstructions != null) request["MeetingInstructions"] = meetingInstructions;
            if (timeZoneId != null) request["TimeZoneId"] = timeZoneId;
            if (dayAvailabilities != null) request["DayAvailabilities"] = dayAvailabilities;

            return request;
        }

        public static object[] CreateDefaultDayAvailabilities()
        {
            return new[]
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
        }

        public static object[] CreateCustomDayAvailabilities(params (DayOfWeek day, bool isActive, (string start, string end)[] ranges)[] dayConfigs)
        {
            return dayConfigs.Select(config => new
            {
                DayOfWeek = config.day,
                IsActive = config.isActive,
                AvailabilityRanges = config.ranges.Select(range => new
                {
                    StartTime = range.start,
                    EndTime = range.end
                }).ToArray()
            }).ToArray();
        }
    }

    /// <summary>
    /// Verifies a user account for testing purposes
    /// </summary>
    /// <param name="client">The HTTP client to use for verification</param>
    public static async Task VerifyUser(HttpClient client)
    {
        var response = await client.GetAsync("/api/authentication/verify?token=valid_token&email=test@example.com");
        // Verification is handled by the test infrastructure
    }

    public static async Task<string> CreateStoreForUser(
        HttpClient client,
        string title,
        string slug,
        string description = "")
    {
        var storeData = StoreTestData.CreateValidStoreFormData(title, slug, description);
        var response = await client.PostAsync(CatalogEndpoints.Stores.Create, storeData);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException($"Failed to create store. Status: {response.StatusCode}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonDoc = System.Text.Json.JsonDocument.Parse(responseContent);
        return jsonDoc.RootElement.GetProperty("slug").GetString()!;
    }

    public static async Task<string> CreateSessionProductForUser(
        HttpClient client,
        string title,
        decimal price,
        int bufferTimeMinutes = 15,
        string subtitle = "Test subtitle",
        string description = "Test description",
        string clickToPay = "Book now",
        object[]? dayAvailabilities = null,
        int durationMinutes = 60)
    {
        var sessionProductRequest = SessionProductTestData.CreateValidSessionProductRequest(
            title, price, bufferTimeMinutes, subtitle, description, clickToPay,
            "Test instructions", "Africa/Tunis", dayAvailabilities, durationMinutes);

        var response = await client.PostAsJsonAsync(CatalogEndpoints.Products.Sessions.Create, sessionProductRequest);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException($"Failed to create session product. Status: {response.StatusCode}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonDoc = System.Text.Json.JsonDocument.Parse(responseContent);
        return jsonDoc.RootElement.GetProperty("slug").GetString()!;
    }

    public static async Task<HttpResponseMessage> CreateStoreRequest(
        HttpClient client,
        string title,
        string slug,
        string description = "",
        (string platform, string url)[]? socialLinks = null)
    {
        var storeData = StoreTestData.CreateValidStoreFormData(title, slug, description, socialLinks);
        return await client.PostAsync(CatalogEndpoints.Stores.Create, storeData);
    }

    public static async Task<HttpResponseMessage> CreateSessionProductRequest(
        HttpClient client,
        string title,
        decimal price,
        int bufferTimeMinutes = 15,
        string subtitle = "Test subtitle",
        string description = "Test description",
        object[]? dayAvailabilities = null,
        int durationMinutes = 60)
    {
        var sessionProductRequest = SessionProductTestData.CreateValidSessionProductRequest(
            title, price, bufferTimeMinutes, subtitle, description, "Book now",
            "Test instructions", "Africa/Tunis", dayAvailabilities, durationMinutes);

        return await client.PostAsJsonAsync(CatalogEndpoints.Products.Sessions.Create, sessionProductRequest);
    }

    public static async Task<HttpResponseMessage> UpdateStoreRequest(
        HttpClient client,
        string title,
        string? description = null,
        (string platform, string url)[]? socialLinks = null)
    {
        var updateRequest = StoreTestData.CreateStoreUpdateRequest(title, description, socialLinks);
        return await client.PutAsJsonAsync(CatalogEndpoints.Stores.Update, updateRequest);
    }

    public static async Task<HttpResponseMessage> UpdateSessionProductRequest(
        HttpClient client,
        string productSlug,
        string title,
        decimal price,
        int durationMinutes = 60,
        int bufferTimeMinutes = 15,
        string? subtitle = null,
        string? description = null,
        string? meetingInstructions = null,
        object[]? dayAvailabilities = null,
        string? clickToPay = "Book now",
        string? timeZoneId = "Africa/Tunis")
    {
        var updateRequest = SessionProductTestData.CreateSessionProductUpdateRequest(
            title, price, durationMinutes, bufferTimeMinutes, subtitle, description,
            clickToPay, meetingInstructions, timeZoneId, dayAvailabilities);

        return await client.PutAsJsonAsync(
            CatalogEndpoints.Products.Sessions.Update.Replace("{productSlug}", productSlug),
            updateRequest);
    }

    public static async Task<HttpResponseMessage> GetStoreRequest(HttpClient client)
    {
        return await client.GetAsync(CatalogEndpoints.Stores.GetMy);
    }

    public static async Task<HttpResponseMessage> GetSessionProductRequest(HttpClient client, string productSlug)
    {
        return await client.GetAsync(CatalogEndpoints.Products.Sessions.Get.Replace("{productSlug}", productSlug));
    }

    public static async Task<HttpResponseMessage> CheckSlugAvailabilityRequest(HttpClient client, string slug)
    {
        return await client.GetAsync(CatalogEndpoints.Stores.CheckSlugAvailability.Replace("{slug}", slug));
    }

    public static async Task VerifySessionProductResponse(HttpResponseMessage response, string? expectedTitle = null, decimal? expectedPrice = null)
    {
        var content = await response.Content.ReadAsStringAsync();
        var jsonDoc = System.Text.Json.JsonDocument.Parse(content);
        var root = jsonDoc.RootElement;

        Assert.True(root.TryGetProperty("title", out _), "Response should contain title");
        Assert.True(root.TryGetProperty("price", out _), "Response should contain price");
        Assert.True(root.TryGetProperty("durationMinutes", out _), "Response should contain durationMinutes");
        Assert.True(root.TryGetProperty("bufferTimeMinutes", out _), "Response should contain bufferTimeMinutes");

        if (expectedTitle != null)
        {
            var actualTitle = root.GetProperty("title").GetString();
            Assert.Equal(expectedTitle, actualTitle);
        }

        if (expectedPrice != null)
        {
            var actualPrice = root.GetProperty("price").GetDecimal();
            Assert.Equal(expectedPrice.Value, actualPrice);
        }
    }

    public static async Task VerifyErrorResponse(HttpResponseMessage response, string? expectedErrorCode = null)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (!string.IsNullOrEmpty(expectedErrorCode))
        {
            Assert.Contains(expectedErrorCode, content, StringComparison.OrdinalIgnoreCase);
        }
    }
}
