using MultiTenants.Web.Interfaces;

namespace MultiTenants.Web.Services
{
    public class AccessTokenStore : IAccessTokenStore
    {
        public string? AccessToken { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public bool IsExpired()
        {
            if (ExpiresAt == null) return true;
            return DateTime.UtcNow >= ExpiresAt.Value;
        }
    }
}
