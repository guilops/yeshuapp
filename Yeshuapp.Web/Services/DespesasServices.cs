using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Yeshuapp.Dtos;
using Yeshuapp.Web.Dtos;


public class DespesasServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string urlBase = string.Empty;

    public DespesasServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        urlBase = "https://yeshuapp.onrender.com";
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<HttpResponseMessage> GetDespesasAsync()
    {
        return await _httpClient.GetAsync($"{urlBase}/despesas");
    }

    public async Task<HttpResponseMessage> GetDespesasByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"{urlBase}/despesas/{id}");
    }

    public async Task<HttpResponseMessage> CreateDespesasAsync(DespesasDto despesasDto)
    {
        var json = JsonConvert.SerializeObject(despesasDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{urlBase}/despesas", content);
    }

    public async Task<HttpResponseMessage> UpdateDespesasAsync(DespesasResponseDto despesasDto)
    {
        var json = JsonConvert.SerializeObject(despesasDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"{urlBase}/despesas/{despesasDto.Id}", content);
    }

    public async Task<HttpResponseMessage> DeleteDespesasAsync(int id)
    {
        return await _httpClient.DeleteAsync($"{urlBase}/despesas/{id}");
    }
}