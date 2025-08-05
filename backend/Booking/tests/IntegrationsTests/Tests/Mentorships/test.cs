using System.Net.Http.Json;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships;

public class test : AuthenticationTestBase
{
    public test(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ff()
    {
        var (userArrange, userAct) = GetClientsForUser("test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);
        var res = await userAct.GetAsync(UsersEndpoints.GetCurrentUser);
        var data = await res.Content.ReadFromJsonAsync<Object>();

        var e = 10; 
    }
}

