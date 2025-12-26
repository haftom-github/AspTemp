using System.Collections.Concurrent;

namespace AspTemp.Features.Auth.Services;

public static class RefreshTokenStore
{
    private static readonly ConcurrentDictionary<string, RefreshTokenInfo> Tokens = [];
    private record RefreshTokenInfo(string Username, DateTime ExpiresAt);

    public static void Add(string token, string username, TimeSpan lifetime)
    {
        var info = new RefreshTokenInfo(username, DateTime.UtcNow.Add(lifetime));
        Tokens[token] = info;
    }

    public static bool TryConsume(string token, out string? username)
    {
        username = null;
        if (!Tokens.TryRemove(token, out var info))
            return false;
        
        if (info.ExpiresAt < DateTime.UtcNow)
            return false;
        
        username = info.Username;
        return true;
    }

    public static void Cleanup()
    {
        var now = DateTime.UtcNow;
        foreach (var kv in Tokens.Where(kv => kv.Value.ExpiresAt < now))
            Tokens.TryRemove(kv.Key, out _);
    }
}