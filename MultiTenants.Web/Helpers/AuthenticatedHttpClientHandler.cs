using MultiTenants.Web.Interfaces;
using MultiTenants.Web.Models;
using System.Net;
using System.Net.Http.Headers;

namespace MultiTenants.Web.Helpers
{
    public class AuthenticatedHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IAccessTokenStore _tokenStore;
        private readonly IHttpClientFactory _clientFactory;

        public AuthenticatedHttpClientHandler(
            IHttpContextAccessor accessor,
            IAccessTokenStore tokenStore,
            IHttpClientFactory clientFactory)
        {
            _accessor = accessor;
            _tokenStore = tokenStore;
            _clientFactory = clientFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 1. Attach token from session
            var token = _accessor.HttpContext?.Session.GetString("access_token");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _tokenStore.AccessToken = token;
            }

            var response = await base.SendAsync(request, cancellationToken);

            // 2. If 401, attempt refresh
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshed = await RefreshTokenAsync();
                if (refreshed)
                {
                    // Retry original request with new token
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenStore.AccessToken!);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            return response;
        }

        private async Task<bool> RefreshTokenAsync()
        {
            // Read refresh token from HttpOnly cookie
            var refreshToken = _accessor.HttpContext?.Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return false;

            var client = _clientFactory.CreateClient("identity");

            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        });

            var response = await client.PostAsync("account/refreshtoken", content);
            if (!response.IsSuccessStatusCode)
                return false;

            var token = await response.Content.ReadFromJsonAsync<TokenResponseModel>();
            if (token == null)
                return false;

            // Store new tokens
            _tokenStore.AccessToken = token.AccessToken;
            _tokenStore.ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);

            // Update session + cookie
            _accessor.HttpContext?.Session.SetString("access_token", token.AccessToken);
            _accessor.HttpContext?.Response.Cookies.Append("refresh_token", token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return true;
        }
    }

}
