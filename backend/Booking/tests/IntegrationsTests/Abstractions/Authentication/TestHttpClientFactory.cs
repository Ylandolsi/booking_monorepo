using System.Collections.Concurrent;
using Booking.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationsTests.Abstractions.Authentication;

/// <summary>
///     Factory for creating HTTP clients with cookie management for different users
///     Similar to buildHttpClient function in Node.js
/// </summary>
public class TestHttpClientFactory
{
    private readonly ConcurrentDictionary<string, HttpClient> _actClients = new();
    private readonly ConcurrentDictionary<string, HttpClient> _arrangeClients = new();
    private readonly CookieManager _cookieManager;
    private readonly WebApplicationFactory<Program> _factory;

    public TestHttpClientFactory(WebApplicationFactory<Program> factory, CookieManager cookieManager)
    {
        _factory = factory;
        _cookieManager = cookieManager;
    }

    /// <summary>
    ///     Creates an HTTP client for arrange operations (setup data, login, etc.)
    ///     Throws on error status codes to catch setup issues early
    /// </summary>
    public HttpClient GetArrangeClient(string userId = "default")
    {
        return _arrangeClients.GetOrAdd(userId, _ => CreateHttpClient(userId, true));
    }

    /// <summary>
    ///     Creates an HTTP client for act operations (actual test calls)
    ///     Does not throw on error status codes so tests can verify error responses
    /// </summary>
    public HttpClient GetActClient(string userId = "default")
    {
        return _actClients.GetOrAdd(userId, _ => CreateHttpClient(userId, false));
    }

    /// <summary>
    ///     Creates both arrange and act clients for a user
    /// </summary>
    public (HttpClient arrange, HttpClient act) GetBothClients(string userId = "default")
    {
        return (GetArrangeClient(userId), GetActClient(userId));
    }

    /// <summary>
    ///     Creates multiple users with both arrange and act clients
    ///     Usage: var users = factory.CreateUsers(["mentor", "mentee", "admin"]);
    ///     Then: users["mentor"].arrange.PostAsync(...) or users["mentee"].act.GetAsync(...)
    /// </summary>
    public Dictionary<string, (HttpClient arrange, HttpClient act)> CreateUsers(params string[] userIds)
    {
        var users = new Dictionary<string, (HttpClient arrange, HttpClient act)>();
        foreach (var userId in userIds) users[userId] = GetBothClients(userId);

        return users;
    }

    private HttpClient CreateHttpClient(string userId, bool throwOnError)
    {
        var baseHandler = _factory.Server.CreateHandler();
        var cookieHandler = new CookieHttpMessageHandler(_cookieManager, userId, baseHandler);

        DelegatingHandler handlerChain = cookieHandler;

        if (throwOnError) handlerChain = new ThrowOnErrorHandler { InnerHandler = cookieHandler };

        var client = new HttpClient(handlerChain)
        {
            BaseAddress = _factory.CreateClient().BaseAddress,
            Timeout = _factory.CreateClient().Timeout
        };

        // Copy headers from factory client
        var factoryClient = _factory.CreateClient();
        foreach (var header in factoryClient.DefaultRequestHeaders)
            client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

        factoryClient.Dispose();

        return client;
    }

    public void ClearAllClients()
    {
        foreach (var client in _arrangeClients.Values)
            client.Dispose();
        foreach (var client in _actClients.Values)
            client.Dispose();

        _arrangeClients.Clear();
        _actClients.Clear();
    }
}

/// <summary>
///     Custom HTTP message handler that automatically manages cookies per user
/// </summary>
public class CookieHttpMessageHandler : DelegatingHandler
{
    private readonly CookieManager _cookieManager;
    private readonly string _userId;

    public CookieHttpMessageHandler(CookieManager cookieManager, string userId, HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        _cookieManager = cookieManager;
        _userId = userId;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Add existing cookies to request
        if (request.RequestUri != null)
        {
            var cookieHeader = _cookieManager.GetCookieHeader(_userId, request.RequestUri);
            if (!string.IsNullOrEmpty(cookieHeader)) request.Headers.Add("Cookie", cookieHeader);
        }

        // Send request
        var response = await base.SendAsync(request, cancellationToken);

        // Store new cookies from response
        if (response.Headers.TryGetValues("Set-Cookie", out var setCookieHeaders) && request.RequestUri != null)
            _cookieManager.SetCookies(_userId, setCookieHeaders, request.RequestUri);

        return response;
    }
}

/// <summary>
///     DelegatingHandler that throws an exception if response status is not success
/// </summary>
public class ThrowOnErrorHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode}). Content: {content}");
        }

        return response;
    }
}