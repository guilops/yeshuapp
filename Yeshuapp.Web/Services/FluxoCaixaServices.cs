using System.Net.Http.Headers;

public class FluxoCaixaServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string urlBase = string.Empty;

    public FluxoCaixaServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        urlBase = _configuration["baseApiUrl"];
    }

    public async Task<HttpResponseMessage> GetFluxoCaixaAsync(string query)
    {
        return await _httpClient.GetAsync($"{urlBase}/fluxocaixa/relatorio?{query}");
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}