namespace IdentityServer.Models;

public class RefreshToken
{
    public string Id { get; init; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? ReplacedByToken { get; private set; }

    public string UserId { get; private set; }
    public IdentityUser? User { get; private set; }

    public bool IsValid => !RevokedAt.HasValue && ExpiresAt > DateTime.UtcNow;


    private RefreshToken() { }
    public RefreshToken(string token, DateTime expiresAt, string userId)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be empty.", nameof(token));
        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be in the future.", nameof(expiresAt));
        Id = Guid.NewGuid().ToString();
        Token = token;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
    }

    public void Revoke(string? replacedByToken = null)
    {
        if (RevokedAt.HasValue)
            throw new InvalidOperationException("Token has already been revoked.");

        RevokedAt = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
    }
}
