using System.Text.Json;
using System.Text.RegularExpressions;
using Amazon.SimpleEmail.Model;
using Bogus;
using IntegrationsTests.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter;
using Snapshooter.Xunit;

namespace IntegrationsTests.Abstractions.Base;

[Collection(nameof(IntegrationTestsCollection))]
public abstract class BaseIntegrationTest : IDisposable, IAsyncLifetime
{
    protected readonly HttpClient _authenticatedVerifiedUserClient;
    protected readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;
    protected readonly IServiceScope _scope;
    protected readonly string _verifiedUserEmail = "verified.user@example.com";


    // authentications purpose
    // users claims wihtout really creating a user ( not persisted in the database )
    // for endpoints that require authentication and authorization
    protected readonly Guid _verifiedUserId = Guid.NewGuid();
    protected readonly TestHttpClientFactory ClientFactory;

    protected readonly CookieManager CookieManager;
    protected readonly IntegrationTestsWebAppFactory Factory;
    protected readonly Faker Fake = new();


    public BaseIntegrationTest(IntegrationTestsWebAppFactory factory)
    {
        Factory = factory;
        _scope = Factory.Services.CreateScope();
        _resetDatabase = factory.ResetDatabase;
        EmailCapturer?.Clear();

        _client = Factory.CreateClient();

        CookieManager = new CookieManager();
        ClientFactory = new TestHttpClientFactory(factory, CookieManager);
    }

    protected List<SendEmailRequest> EmailCapturer => Factory.CapturedEmails;

    // Default clients (backward compatibility)
    protected HttpClient ArrangeClient => ClientFactory.GetArrangeClient();
    protected HttpClient ActClient => ClientFactory.GetActClient();

    public async Task InitializeAsync()
    {
        await _resetDatabase();
        EmailCapturer?.Clear();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected bool IsSucceed(int statusCode) // Change parameter type to int
    {
        return statusCode == StatusCodes.Status200OK ||
               statusCode == StatusCodes.Status201Created ||
               statusCode == StatusCodes.Status204NoContent;
    }

    /*protected void CheckSuccess(HttpResponseMessage response) => Assert.True(IsSucceed((int)response.StatusCode),
        "The response status code does not indicate success.");*/

    protected void CheckSuccess(HttpResponseMessage response)
    {
        Assert.True(response.IsSuccessStatusCode, $"Expected success status code, but got {(int)response.StatusCode}");
    }

    protected (string? Token, string? Email) ExtractTokenAndEmailFromEmail(string userEmail)
    {
        var sentEmail = EmailCapturer.LastOrDefault(e => e.Destination.ToAddresses.Contains(userEmail));
        if (sentEmail is null) return (null, null);

        var match = Regex.Match(
            sentEmail.Message.Body.Html.Data,
            @"href=['""](?<url>https?://[^'""]+\?token=[^&]+&email=[^'""]+)['""]",
            RegexOptions.IgnoreCase);

        if (!match.Success) return (null, null);

        var url = match.Groups["url"].Value;
        var uri = new Uri(url);

        // Use QueryHelpers to parse and decode the query string
        var queryParams = QueryHelpers.ParseQuery(uri.Query);

        queryParams.TryGetValue("token", out var token);
        queryParams.TryGetValue("email", out var email);

        return (token.ToString(), email.ToString());
    }

    #region Snapshot Testing Helpers

    protected async Task<JsonDocument> ParseResponseContent(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);
        return jsonDocument;

        /*
         access an array

        var timeSlots = jsonDocument.RootElement.GetProperty("timeSlots"); // JsonElement (array)
        if (timeSlots.ValueKind == JsonValueKind.Array)
        {
            var timeSlotsLength = timeSlots.GetArrayLength(); // 0 for []
            var timeSlotsList = timeSlots.EnumerateArray().ToList();

         }

         access a boolean

         var isAvailable = jsonDocument.RootElement.GetProperty("isAvailable").GetBoolean();

         access int

         var year = jsonDocument.RootElement.GetProperty("year").GetInt32()

        access object

        var days = jsonDocument.RootElement.GetProperty("days"); // JsonElement (object)
           if (days.ValueKind == JsonValueKind.Object)
           {
               // Access specific properties
               bool monday = days.GetProperty("monday").GetBoolean(); // true
               bool tuesday = days.GetProperty("tuesday").GetBoolean(); // false

               // Iterate over all properties
               var daysProperties = days.EnumerateObject().ToDictionary(prop => prop.Name, prop => prop.Value.GetBoolean());
               // Result: { "monday": true, "tuesday": false, "wednesday": true }
           }
        */
    }

    /// <summary>
    ///     Captures and compares JSON response snapshot with optional field ignores.
    /// </summary>
    /// <param name="response">The HTTP response to snapshot.</param>
    /// <param name="snapshotName">Optional name for the snapshot file.</param>
    /// <param name="ignoreFields">Fields to ignore.</param>
    protected async Task MatchSnapshotAsync(HttpResponseMessage response, string snapshotName,
        Func<MatchOptions, MatchOptions> matchOptions = null)
    {
        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);
        var formattedJson = JsonSerializer.Serialize(jsonDocument, new JsonSerializerOptions { WriteIndented = true });
        Snapshot.Match(formattedJson, snapshotName, matchOptions);
    }

    #endregion

    #region User Management Methods

    /// <summary>
    ///     Creates multiple users for testing scenarios
    ///     Usage: var users = CreateUsers("mentor", "mentee", "admin");
    ///     Then: await users["mentor"].arrange.PostAsync(...);
    /// </summary>
    protected Dictionary<string, (HttpClient arrange, HttpClient act)> CreateUsers(params string[] userIds)
    {
        return ClientFactory.CreateUsers(userIds);
    }

    /// <summary>
    ///     Gets HTTP clients for a specific user
    /// </summary>
    protected (HttpClient arrange, HttpClient act) GetClientsForUser(string userId)
    {
        return ClientFactory.GetBothClients(userId);
    }

    /// <summary>
    ///     Clears cookies for a specific user or all users
    /// </summary>
    protected void ClearCookies(string? userId = null)
    {
        CookieManager.ClearCookies(userId);
    }

    /// <summary>
    ///     Gets all cookies for a user
    /// </summary>
    protected Dictionary<string, string> GetCookies(string userId = "default")
    {
        return CookieManager.GetAllCookies(userId);
    }

    /// <summary>
    ///     Checks if a user has authentication cookies
    /// </summary>
    protected bool IsUserAuthenticated(string userId = "default")
    {
        return CookieManager.HasCookies(userId);
    }

    #endregion


    #region Reset and Cleanup Methods

    /// <summary>
    ///     Resets state before each test (similar to resetBeforeEach in Node.js)
    /// </summary>
    protected virtual void ResetBeforeEach()
    {
        ClearCookies(); // Clear all cookies
        ClientFactory.ClearAllClients(); // Dispose and recreate clients
    }

    /// <summary>
    ///     Cleanup method called after each test
    /// </summary>
    public virtual void Dispose()
    {
        ClientFactory.ClearAllClients();
        CookieManager.ClearCookies();

        _scope.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}