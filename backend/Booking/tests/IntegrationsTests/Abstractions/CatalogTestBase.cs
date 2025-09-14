using IntegrationsTests.Abstractions.Authentication;
using IntegrationsTests.Abstractions.Base;

namespace IntegrationsTests.Abstractions;

public class CatalogTestBase : AuthenticationTestBase
{
    public CatalogTestBase(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }
}