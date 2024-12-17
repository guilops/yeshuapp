using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Yeshuapp.Web.Dtos;

public class FluxoCaixaServices
{
    private readonly HttpClient _httpClient;

    public FluxoCaixaServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetFluxoCaixaAsync(string query)
    {
        return await _httpClient.GetAsync($"https://localhost:44337/fluxocaixa/relatorio?{query}");
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}