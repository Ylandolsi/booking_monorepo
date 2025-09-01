using System.Collections.Concurrent;
using System.Net;

namespace IntegrationsTests.Abstractions.Authentication;

/// <summary>
/// Manages cookies for different user contexts in integration tests
/// Similar to Node.js CookieJar functionality
/// </summary>
public class CookieManager
{
    private readonly ConcurrentDictionary<string, CookieContainer> _cookieContainers = new();
    private readonly ConcurrentDictionary<string, Dictionary<string, string>> _cookieStore = new();

    public CookieContainer GetCookieContainer(string userId = "default")
    {
        return _cookieContainers.GetOrAdd(userId, _ => new CookieContainer());
    }

    public void SetCookies(string userId, IEnumerable<string> setCookieHeaders, Uri baseUri)
    {
        var container = GetCookieContainer(userId);
        var cookieDict = _cookieStore.GetOrAdd(userId, _ => new Dictionary<string, string>());

        foreach (var setCookieHeader in setCookieHeaders)
        {
            try
            {
                container.SetCookies(baseUri, setCookieHeader);
                
                // Also store in our dictionary for easy access
                var parts = setCookieHeader.Split(';')[0].Split('=', 2);
                if (parts.Length == 2)
                {
                    cookieDict[parts[0].Trim()] = parts[1].Trim();
                }
            }
            catch (Exception ex)
            {
                // Log but don't fail - some cookies might have formatting issues
                Console.WriteLine($"Failed to set cookie for user {userId}: {ex.Message}");
            }
        }
    }

    public string GetCookieHeader(string userId = "default", Uri? uri = null)
    {
        var container = GetCookieContainer(userId);
        if (uri == null) return string.Empty;
        
        return container.GetCookieHeader(uri);
    }

    public Dictionary<string, string> GetAllCookies(string userId = "default")
    {
        return _cookieStore.GetOrAdd(userId, _ => new Dictionary<string, string>());
    }

    public void ClearCookies(string? userId = null)
    {
        if (userId != null)
        {
            _cookieContainers.TryRemove(userId, out _);
            _cookieStore.TryRemove(userId, out _);
        }
        else
        {
            _cookieContainers.Clear();
            _cookieStore.Clear();
        }
    }

    public bool HasCookies(string userId = "default")
    {
        return _cookieStore.ContainsKey(userId) && _cookieStore[userId].Any();
    }
}