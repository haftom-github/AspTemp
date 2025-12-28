using System.Collections.Concurrent;

namespace AspTemp.Features.Auth.Users.Services;

public static class RefreshTokenStore
{
    private static readonly ConcurrentDictionary<string, RefreshTokenInfo> Tokens = [];
    private record RefreshTokenInfo(Guid UserId, DateTime ExpiresAt);

    public static void Add(string token, Guid userId, TimeSpan lifetime)
    {
        var info = new RefreshTokenInfo(userId, DateTime.UtcNow.Add(lifetime));
        Tokens[token] = info;
    }

    public static bool TryConsume(string token, out Guid? userId)
    {
        userId = null;
        if (!Tokens.TryRemove(token, out var info))
            return false;
        
        if (info.ExpiresAt < DateTime.UtcNow)
            return false;
        
        userId = info.UserId;
        return true;
    }

    public static void Cleanup()
    {
        var now = DateTime.UtcNow;
        foreach (var kv in Tokens.Where(kv => kv.Value.ExpiresAt < now))
            Tokens.TryRemove(kv.Key, out _);
    }
}