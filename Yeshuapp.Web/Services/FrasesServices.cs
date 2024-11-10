using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Yeshuapp.Dtos;


public class FrasesServices
{
    private readonly HttpClient _httpClient;

    public FrasesServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<HttpResponseMessage> GetFrasesAsync()
    {
        return await _httpClient.GetAsync("https://localhost:44337/frases");
    }

    public async Task<HttpResponseMessage> GetFraseByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"https://localhost:44337/frases/{id}");
    }

    public async Task<HttpResponseMessage> CreateFrasesAsync(FraseDto frase)
    {
        var json = JsonConvert.SerializeObject(frase);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync("https://localhost:44337/frases", content);
    }

    public async Task<HttpResponseMessage> UpdateFrasesAsync(FraseDto frase)
    {
        var json = JsonConvert.SerializeObject(frase);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"https://localhost:44337/frases/{frase.Id}", content);
    }

    public async Task<HttpResponseMessage> DeleteFrasesAsync(int id)
    {
        return await _httpClient.DeleteAsync($"https://localhost:44337/frases/{id}");
    }
}