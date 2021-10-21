using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

public class IndexModel : PageModel
{

    public string? FirstName = "";
    public string? LastName = "";

    public string FhirJson = "";

    private IConfiguration _configuration;

    public IndexModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task OnGetAsync()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        if (accessToken == null)
        {
            await HttpContext.ChallengeAsync("oidc", new AuthenticationProperties()
            {
                RedirectUri = "/"
            });
        }

        var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        FirstName = token.Payload["dips-firstname"].ToString();
        LastName = token.Payload["dips-lastname"].ToString();

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        httpClient.DefaultRequestHeaders.Add("dips-subscription-key", _configuration["dips-subscription-key"]);

        var response = await httpClient.GetAsync("https://api.dips.no/fhir/patient/?family=Danser");
        FhirJson = await response.Content.ReadAsStringAsync();
    }

}
