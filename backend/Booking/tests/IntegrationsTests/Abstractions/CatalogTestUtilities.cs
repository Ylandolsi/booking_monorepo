using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Booking.Modules.Catalog.Features;
using Booking.Modules.Catalog.Features.Products.Sessions;
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

            var jpegBytes = Convert.FromBase64String(
                "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCAABAAEDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/8QAFQEBAQAAAAAAAAAAAAAAAAAAAAX/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwA/AB//2Q==");
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
                request["SocialLinks"] =
                    socialLinks.Select(sl => new { Platform = sl.platform, Url = sl.url }).ToArray();
            }

            return request;
        }
    }

    public static class SessionProductTestData
    {
        // create 

        public static MultipartFormDataContent CreateValidSessionProductFormData(
            string title,
            decimal price,
            int bufferTimeMinutes = 15,
            string subtitle = "Test subtitle",
            string description = "Test description",
            string clickToPay = "Book now",
            string meetingInstructions = "Test instructions",
            string timeZoneId = "Africa/Tunis",
            ( DayOfWeek DayOfWeek, bool IsActive, List<AvailabilityRange> AvailabilityRanges )[]? dayAvailabilities =
                null,
            int durationMinutes = 60)
        {
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(title), "Title");
            formData.Add(new StringContent(subtitle), "Subtitle");

            formData.Add(new StringContent(price.ToString(CultureInfo.InvariantCulture)), "Price");
            formData.Add(new StringContent(description), "Description");
            formData.Add(new StringContent(clickToPay), "ClickToPay");
            formData.Add(new StringContent(bufferTimeMinutes.ToString()), "DurationMinutes");
            formData.Add(new StringContent(durationMinutes.ToString()), "BufferTimeMinutes");
            formData.Add(new StringContent(meetingInstructions), "MeetingInstructions");
            formData.Add(new StringContent(timeZoneId), "TimeZoneId");

            var previewBase64 =
                "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCAABAAEDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/8QAFQEBAQAAAAAAAAAAAAAAAAAAAAX/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwA/AB//2Q==";
            var previewBytes = Convert.FromBase64String(previewBase64);

            var thumbnailBase64 =
                "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCAABAAEDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/8QAFQEBAQAAAAAAAAAAAAAAAAAAAAX/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwA/AB//2Q==";
            var thumbnailBytes = Convert.FromBase64String(thumbnailBase64);


            var previewContent = new ByteArrayContent(previewBytes);
            previewContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            formData.Add(previewContent, "PreviewImage", "preview.jpg");

            var thumbnailContent = new ByteArrayContent(thumbnailBytes);
            thumbnailContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            formData.Add(thumbnailContent, "ThumbnailImage", "thumbnail.jpg");


            // Add dayAv if provided 
            if (dayAvailabilities != null)
            {
                for (int i = 0; i < dayAvailabilities.Length; i++)
                {
                    // DayOfWeek
                    formData.Add(
                        new StringContent(((int)dayAvailabilities[i].DayOfWeek).ToString()),
                        $"DayAvailabilities[{i}].DayOfWeek"
                    );

                    // IsActive
                    formData.Add(
                        new StringContent(dayAvailabilities[i].IsActive.ToString().ToLower()), // "true"/"false"
                        $"DayAvailabilities[{i}].IsActive"
                    );

                    // AvailabilityRanges
                    for (int j = 0; j < dayAvailabilities[i].AvailabilityRanges.Count; j++)
                    {
                        formData.Add(
                            new StringContent(dayAvailabilities[i].AvailabilityRanges[j].StartTime),
                            $"DayAvailabilities[{i}].AvailabilityRanges[{j}].StartTime"
                        );
                        formData.Add(
                            new StringContent(dayAvailabilities[i].AvailabilityRanges[j].EndTime),
                            $"DayAvailabilities[{i}].AvailabilityRanges[{j}].EndTime"
                        );
                    }
                }
            }

            return formData;
        }

        public static (DayOfWeek DayOfWeek, bool IsActive, List<AvailabilityRange> AvailabilityRanges)[]
            CreateDefaultDayAvailabilities()
        {
            return new[]
            {
                (
                    DayOfWeek: DayOfWeek.Monday,
                    IsActive: true,
                    AvailabilityRanges: new List<AvailabilityRange>
                    {
                        new AvailabilityRange { StartTime = "09:00", EndTime = "12:00" },
                        new AvailabilityRange { StartTime = "14:00", EndTime = "17:00" }
                    }
                ),
                (
                    DayOfWeek: DayOfWeek.Wednesday,
                    IsActive: true,
                    AvailabilityRanges: new List<AvailabilityRange>
                    {
                        new AvailabilityRange { StartTime = "10:00", EndTime = "16:00" }
                    }
                )
            };
        }


        public static (DayOfWeek DayOfWeek, bool IsActive, List<AvailabilityRange> AvailabilityRanges)[]?
            CreateCustomDayAvailabilities(
                params (DayOfWeek day, bool isActive, (string start, string end)[] ranges)[] dayConfigs)
        {
            return dayConfigs.Select(config => (
                DayOfWeek: config.day,
                IsActive: config.isActive,
                AvailabilityRanges: config.ranges
                    .Select(range => new AvailabilityRange
                    {
                        StartTime = range.start,
                        EndTime = range.end
                    })
                    .ToList()
            )).ToArray();
        }
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

    public static async Task<string> CreateStoreForUser(
        HttpClient client,
        string title,
        string slug,
        string description = "")
    {
        var response = await CreateStoreRequest(client, title, slug, description);

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
        ( DayOfWeek DayOfWeek, bool IsActive, List<AvailabilityRange> AvailabilityRanges )[]? dayAvailabilities = null,
        int durationMinutes = 60)
    {
        var response = await CreateSessionProductRequest(
            client,
            title,
            price,
            bufferTimeMinutes,
            subtitle,
            description,
            clickToPay,
            dayAvailabilities,
            durationMinutes);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException($"Failed to create session product. Status: {response.StatusCode}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonDoc = System.Text.Json.JsonDocument.Parse(responseContent);
        return jsonDoc.RootElement.GetProperty("slug").GetString()!;
    }


    public static async Task<HttpResponseMessage> CreateSessionProductRequest(
        HttpClient client,
        string title,
        decimal price,
        int bufferTimeMinutes = 15,
        string subtitle = "Test subtitle",
        string description = "Test description",
        string clickToPay = "Book now",
        ( DayOfWeek DayOfWeek, bool IsActive, List<AvailabilityRange> AvailabilityRanges )[]? dayAvailabilities = null,
        int durationMinutes = 60)
    {
        var sessionProductRequest = SessionProductTestData.CreateValidSessionProductFormData(
            title,
            price,
            bufferTimeMinutes,
            subtitle,
            description,
            clickToPay,
            "Test instructions",
            "Africa/Tunis",
            dayAvailabilities,
            durationMinutes);

        return await client.PostAsync(CatalogEndpoints.Products.Sessions.Create, sessionProductRequest);
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
        ( DayOfWeek DayOfWeek, bool IsActive, List<AvailabilityRange> AvailabilityRanges )[]? dayAvailabilities = null,
        string? clickToPay = "Book now",
        string? timeZoneId = "Africa/Tunis")
    {
        var updateRequest = SessionProductTestData.CreateValidSessionProductFormData(
            title,
            price,
            durationMinutes,
            subtitle,
            description,
            clickToPay,
            meetingInstructions,
            timeZoneId
            , dayAvailabilities,
            durationMinutes);

        return await client.PutAsync(
            CatalogEndpoints.Products.Sessions.Update.Replace("{productSlug}", productSlug),
            updateRequest);
    }


    public static async Task<HttpResponseMessage> GetSessionProductRequest(HttpClient client, string productSlug)
    {
        return await client.GetAsync(CatalogEndpoints.Products.Sessions.Get.Replace("{productSlug}", productSlug));
    }

    public static async Task<HttpResponseMessage> CheckSlugAvailabilityRequest(HttpClient client, string slug)
    {
        return await client.GetAsync(CatalogEndpoints.Stores.CheckSlugAvailability.Replace("{slug}", slug));
    }
}