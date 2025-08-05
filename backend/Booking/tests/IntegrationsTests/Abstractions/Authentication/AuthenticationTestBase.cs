using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Booking.Modules.Users.BackgroundJobs;
using Booking.Modules.Users.Features;
using Booking.Modules.Users.Features.Utils;

namespace IntegrationsTests.Abstractions;

public abstract class AuthenticationTestBase : BaseIntegrationTest
{
    protected string DefaultPassword => "TestPassword123!";
    protected string DefaultEmail => "test@gmail.com";
    protected AuthenticationTestBase(IntegrationTestsWebAppFactory factory) : base(factory)
    {

    }


    protected async Task TriggerOutboxProcess()
    {
        var outboxProcessor = _scope.ServiceProvider.GetRequiredService<ProcessOutboxMessagesJob>();
        await outboxProcessor.ExecuteAsync(null);
    }
    /*
    protected async Task<LoginResponse> LoginUser(string email, string password)
    {
        var loginPayload = new
        {
            Email = email,
            Password = password
        };

        var response = await _client.PostAsJsonAsync(UsersEndpoints.Login, loginPayload);
        response.EnsureSuccessStatusCode();

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginResponse);
        return loginResponse;
    }

    protected async Task RegisterAndVerifyUser(string email, string password, bool verify = true)
    {
        var registrationPayload = new
        {
            FirstName = "Test",
            LastName = "User",
            Email = email,
            Password = password,
            ProfilePictureSource = ""
        };

        var registerResponse = await _client.PostAsJsonAsync(UsersEndpoints.Register, registrationPayload);
        registerResponse.EnsureSuccessStatusCode();


        await TriggerOutboxProcess();

        await Task.Delay(TimeSpan.FromSeconds(2));
        if (!verify) return;


        var (token, parsedEmail) = ExtractTokenAndEmailFromEmail(email);

        Assert.NotNull(token);
        Assert.NotNull(parsedEmail);

        var verificationPayload = new { Email = parsedEmail, Token = token };
        var verifyResponse = await _client.PostAsJsonAsync(UsersEndpoints.VerifyEmail, verificationPayload);

        //verifyResponse.EnsureSuccessStatusCode();
        CheckSuccess(verifyResponse);
    }
    protected async Task<LoginResponse> CreateUserAndLogin()
    {
        string? email = Fake.Internet.Email();
        await RegisterAndVerifyUser(email, DefaultPassword, true);
        return await LoginUser(email, DefaultPassword);

    }

    */
    
     #region Authentication Helper Methods

    /// <summary>
    /// Registers and verifies a user, then logs them in
    /// Returns the login response and sets up cookies for the specified userId
    /// </summary>
    protected async Task<LoginResponse> CreateUserAndLogin(string? email = null, string? password = null ,  HttpClient? arrangeClient = null)
    {
        arrangeClient ??= ArrangeClient; 
        
        email ??= Fake.Internet.Email();
        password ??= "SecurePassword123!";

        await RegisterAndVerifyUser(email, password, true , arrangeClient);
        return await LoginUser(email, password, arrangeClient);
    }

    /// <summary>
    /// Registers and verifies a user using the specified client
    /// </summary>
    protected async Task RegisterAndVerifyUser(string email, string password, bool verify = true ,  HttpClient? arrangeClient = null)
    {
        arrangeClient ??= ArrangeClient;

        var registerPayload = new
        {
            Email = email,
            Password = password,
            FirstName = "Test",
            LastName = "User"
        };

        var registerResponse = await arrangeClient.PostAsJsonAsync("/users/register", registerPayload);
        if (!verify) return; 
        await TriggerOutboxProcess();

        await Task.Delay(TimeSpan.FromSeconds(2));


        var (token, parsedEmail) = ExtractTokenAndEmailFromEmail(email);

        Assert.NotNull(token);
        Assert.NotNull(parsedEmail);

        var verificationPayload = new { Email = parsedEmail, Token = token };
        var verifyResponse = await arrangeClient.PostAsJsonAsync(UsersEndpoints.VerifyEmail, verificationPayload);
        verifyResponse.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Logs in a user using the specified client
    /// </summary>
    protected async Task<LoginResponse> LoginUser(string email, string password, HttpClient? arrangeClient = null)
    {
        arrangeClient ??= ArrangeClient;

        var loginPayload = new
        {
            Email = email,
            Password = password
        };

        var loginResponse = await arrangeClient.PostAsJsonAsync("/users/login", loginPayload);
        loginResponse.EnsureSuccessStatusCode();

        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        return loginData ?? throw new InvalidOperationException("Failed to deserialize login response");
    }

    #endregion

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

}