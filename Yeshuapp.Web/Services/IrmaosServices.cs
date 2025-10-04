using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Yeshuapp.Web.Dtos;

public class IrmaosServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string urlBase = string.Empty;  

    public IrmaosServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        urlBase = _configuration["baseApiUrl"];
    }

    public async Task<HttpResponseMessage> GetIrmaosAsync()
    {
        return await _httpClient.GetAsync($"{urlBase}/irmaos");
    }

    public async Task<HttpResponseMessage> GetIrmaoByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"{urlBase}/irmaos/{id}");
    }

    public async Task<HttpResponseMessage> CreateIrmaoAsync(ClienteResponseDto irmao)
    {
        var json = JsonConvert.SerializeObject(irmao);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{urlBase}/irmaos", content);
        return response;
    }

    public async Task<HttpResponseMessage> PutIrmaoAsync(ClienteResponseDto irmao)
    {
        var json = JsonConvert.SerializeObject(irmao);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"{urlBase}/irmaos/{irmao.Id}", content);
    }

    public async Task<HttpResponseMessage> DeleteIrmaoAsync(int id)
    {
        return await _httpClient.DeleteAsync($"{urlBase}/irmaos/{id}");
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

}