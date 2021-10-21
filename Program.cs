var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

if (String.IsNullOrEmpty(config["openid-secret"]))
{
    throw new Exception("`openid-secret` configuration missing. Please add one to your appsettings!");
}
if (String.IsNullOrEmpty(config["dips-subscription-key"]))
{
    throw new Exception("`dips-subscription-key` configuration missing. Please add one to your appsettings!");
}

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie(options =>
{
    options.ForwardChallenge = "oidc";
})
.AddOpenIdConnect("oidc", options =>
{

    options.Authority = "https://api.dips.no/dips.oauth";
    options.ClientId = "uit";
    options.ClientSecret = config["openid-secret"];
    options.ResponseType = "code";

    options.SaveTokens = true;
    options.CallbackPath = new PathString("/callback");

    options.Scope.Clear();
    options.Scope.Add("openid");



    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
    };

});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();

