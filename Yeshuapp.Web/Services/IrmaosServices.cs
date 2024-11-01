using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using Yeshuapp.Web.Dtos;

public class IrmaosServices
{
    private readonly HttpClient _httpClient;

    public IrmaosServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetIrmaosAsync()
    {
        return await _httpClient.GetAsync("https://localhost:44337/irmaos");
    }

    public async Task<HttpResponseMessage> GetIrmaoByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"https://localhost:44337/irmaos/{id}");
    }

    public async Task<HttpResponseMessage> CreateIrmaoAsync(ClienteResponseDto irmao)
    {
        var json = JsonConvert.SerializeObject(irmao);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://localhost:44337/irmaos", content);
        return response;
    }

    public async Task<HttpResponseMessage> PutIrmaoAsync(ClienteResponseDto irmao)
    {
        var json = JsonConvert.SerializeObject(irmao);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"https://localhost:44337/irmaos/{irmao.Id}", content);
    }

    public async Task<HttpResponseMessage> DeleteIrmaoAsync(int id)
    {
        return await _httpClient.DeleteAsync($"https://localhost:44337/irmaos/{id}");
    }
}