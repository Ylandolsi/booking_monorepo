using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Booking.Modules.Users.Features;
using Booking.Modules.Users.Features.Authentication;
using Booking.Modules.Users.Features.Authentication.Me;
using Booking.Modules.Users.Features.Utils;
using Booking.Modules.Users.Presistence;
using IntegrationsTests.Abstractions.Base;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationsTests.Abstractions.Authentication;

public abstract class AuthenticationTestBase : BaseIntegrationTest
{
    protected AuthenticationTestBase(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    protected string DefaultPassword => "TestPassword123!";
    protected string DefaultEmail => "test@gmail.com";

    protected async Task<MeData> GetCurrenUserInfo(HttpClient? arrangeClient = null)
    {
        arrangeClient ??= ArrangeClient;
        var response = await arrangeClient.GetAsync(UsersEndpoints.GetCurrentUser);
        return await response.Content.ReadFromJsonAsync<MeData>() ??
               throw new InvalidOperationException("Failed to deserialize user info response");
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

    #region Authentication Helper Methods

    /// <summary>
    ///     Registers and verifies a user, then logs them in
    ///     Returns the login response and sets up cookies for the specified userId
    /// </summary>
    protected async Task<LoginResponse> CreateUserAndLogin(string? email = null, string? password = null,
        HttpClient? arrangeClient = null)
    {
        arrangeClient ??= ArrangeClient;

        email ??= Fake.Internet.Email();
        password ??= "SecurePassword123!";

        await RegisterAndVerifyUser(email, password, true, arrangeClient);
        var loginInfo = await LoginUser(email, password, arrangeClient);
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        var currentUser = await dbContext.Users.Where(u => u.Email == loginInfo.Email).FirstOrDefaultAsync();
        if (currentUser != null)
        {
            currentUser.IntegrateWithGoogle(loginInfo.Email);
            await dbContext.SaveChangesAsync();
        }

        return loginInfo;
    }

    /// <summary>
    ///     Registers and verifies a user using the specified client
    /// </summary>
    protected async Task RegisterAndVerifyUser(string email, string password, bool verify = true,
        HttpClient? arrangeClient = null)
    {
        arrangeClient ??= ArrangeClient;

        var registerPayload = new
        {
            Email = email,
            Password = password,
            FirstName = "Test",
            LastName = "User"
        };

        var registerResponse = await arrangeClient.PostAsJsonAsync(UsersEndpoints.Register, registerPayload);
        await Task.Delay(TimeSpan.FromSeconds(2));
        if (!verify) return;


        var (token, parsedEmail) = ExtractTokenAndEmailFromEmail(email);

        Assert.NotNull(token);
        Assert.NotNull(parsedEmail);

        var verificationPayload = new { Email = parsedEmail, Token = token };
        var verifyResponse = await arrangeClient.PostAsJsonAsync(UsersEndpoints.VerifyEmail, verificationPayload);
        verifyResponse.EnsureSuccessStatusCode();
    }

    /// <summary>
    ///     Logs in a user using the specified client
    /// </summary>
    protected async Task<LoginResponse> LoginUser(string email, string password, HttpClient? arrangeClient = null)
    {
        arrangeClient ??= ArrangeClient;

        var loginPayload = new
        {
            Email = email,
            Password = password
        };

        var loginResponse = await arrangeClient.PostAsJsonAsync(UsersEndpoints.Login, loginPayload);
        loginResponse.EnsureSuccessStatusCode();

        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        return loginData ?? throw new InvalidOperationException("Failed to deserialize login response");
    }

    #endregion
}