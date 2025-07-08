using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Yeshuapp.Dtos;


public class FrasesServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string urlBase = string.Empty;

    public FrasesServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        urlBase = _configuration["baseApiUrl"];
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<HttpResponseMessage> GetFrasesAsync()
    {
        return await _httpClient.GetAsync($"{urlBase}/frases");
    }

    public async Task<HttpResponseMessage> GetFraseByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"{urlBase}/frases/{id}");
    }

    public async Task<HttpResponseMessage> CreateFrasesAsync(FraseDto frase)
    {
        var json = JsonConvert.SerializeObject(frase);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{urlBase}/frases", content);
    }

    public async Task<HttpResponseMessage> UpdateFrasesAsync(FraseDto frase)
    {
        var json = JsonConvert.SerializeObject(frase);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"{urlBase}/frases/{frase.Id}", content);
    }

    public async Task<HttpResponseMessage> DeleteFrasesAsync(int id)
    {
        return await _httpClient.DeleteAsync($"{urlBase}/frases/{id}");
    }
}