using MultiTenants.Web.Helpers;
using MultiTenants.Web.Interfaces;
using MultiTenants.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// HttpContextAccessor for session
builder.Services.AddHttpContextAccessor();

// Access token store
builder.Services.AddScoped<IAccessTokenStore, AccessTokenStore>();

// Authenticated HTTP handler
builder.Services.AddScoped<AuthenticatedHttpClientHandler>();

// API HttpClient with handler
builder.Services.AddHttpClient<IApiClient, ApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7164/");
})
.AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

// Identity HttpClient for refresh token calls
builder.Services.AddHttpClient("identity", client =>
{
    client.BaseAddress = new Uri("https://localhost:7164/");
});

// Session for storing tokens
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // MUST be before Auth middleware if used

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
