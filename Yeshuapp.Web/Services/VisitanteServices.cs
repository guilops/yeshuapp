using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Yeshuapp.Dtos;

public class VisitanteServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string urlBase = string.Empty;

    public VisitanteServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        urlBase = _configuration["baseApiUrl"];
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<HttpResponseMessage> GetVisitantesAsync()
    {
        return await _httpClient.GetAsync($"{urlBase}/visitantes");
    }

    public async Task<HttpResponseMessage> SalvarVisitantesAsync(VisitanteDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{urlBase}/visitantes", content);
    }

}