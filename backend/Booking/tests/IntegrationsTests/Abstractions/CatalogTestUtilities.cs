using System.Net;
using System.Net.Http.Json;
using System.Text;

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

            // Add a dummy image file
            var imageContent = new ByteArrayContent(Encoding.UTF8.GetBytes("dummy image content"));
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            formData.Add(imageContent, "file", "test-image.jpg");

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
            object[]? dayAvailabilities = null)
        {
            return new
            {
                Title = title,
                Subtitle = subtitle,
                Description = description,
                ClickToPay = clickToPay,
                Price = price,
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
            string? meetingInstructions = null,
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
            if (meetingInstructions != null) request["MeetingInstructions"] = meetingInstructions;
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

    public static class ApiEndpoints
    {
        public const string CreateStore = "/api/catalog/stores";
        public const string GetMyStore = "/api/catalog/stores/my-store";
        public const string UpdateStore = "/api/catalog/stores/{0}";
        public const string CheckSlugAvailability = "/api/catalog/stores/slug-availability/{0}";

        public const string CreateSessionProduct = "/api/catalog/products/sessions";
        public const string GetSessionProduct = "/api/catalog/products/sessions/{0}";
        public const string UpdateSessionProduct = "/api/catalog/products/sessions/{0}";
    }

    public static async Task<int> CreateStoreAndGetId(HttpClient client, string storeName, string storeSlug, string description = "")
    {
        var storeData = StoreTestData.CreateValidStoreFormData(storeName, storeSlug, description);
        var response = await client.PostAsync(ApiEndpoints.CreateStore, storeData);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException($"Failed to create store. Status: {response.StatusCode}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonDoc = System.Text.Json.JsonDocument.Parse(responseContent);

        // Assuming the response contains an ID field
        return jsonDoc.RootElement.TryGetProperty("id", out var idProperty)
            ? idProperty.GetInt32()
            : 1; // Fallback ID
    }

    public static async Task<int> CreateSessionProductAndGetId(
        HttpClient client,
        string title,
        decimal price,
        int bufferTimeMinutes = 15)
    {
        var sessionProductRequest = SessionProductTestData.CreateValidSessionProductRequest(title, price, bufferTimeMinutes);
        var response = await client.PostAsJsonAsync(ApiEndpoints.CreateSessionProduct, sessionProductRequest);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException($"Failed to create session product. Status: {response.StatusCode}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonDoc = System.Text.Json.JsonDocument.Parse(responseContent);
        return jsonDoc.RootElement.GetProperty("id").GetInt32();
    }

    public static async Task VerifyStoreResponse(HttpResponseMessage response, string? expectedTitle = null)
    {
        var content = await response.Content.ReadAsStringAsync();
        var jsonDoc = System.Text.Json.JsonDocument.Parse(content);
        var root = jsonDoc.RootElement;

        Assert.True(root.TryGetProperty("title", out _), "Response should contain title");
        Assert.True(root.TryGetProperty("slug", out _), "Response should contain slug");
        Assert.True(root.TryGetProperty("isPublished", out _), "Response should contain isPublished");
        Assert.True(root.TryGetProperty("createdAt", out _), "Response should contain createdAt");

        if (expectedTitle != null)
        {
            var actualTitle = root.GetProperty("title").GetString();
            Assert.Equal(expectedTitle, actualTitle);
        }
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
