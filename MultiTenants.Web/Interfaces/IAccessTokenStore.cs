namespace MultiTenants.Web.Interfaces
{
    public interface IAccessTokenStore
    {
        string? AccessToken { get; set; }

        DateTime? ExpiresAt { get; set; }

        bool IsExpired();
    }
}
