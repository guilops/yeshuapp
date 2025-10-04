using Newtonsoft.Json;
using System.Text;
using Yeshuapp.Dtos;

public class EventosServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string urlBase = string.Empty;

    public EventosServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        urlBase = _configuration["baseApiUrl"];
    }

    public async Task<HttpResponseMessage> GetEventosAsync(bool filtrarAbertos = false)
    {
        var url = $"{urlBase}/eventos";
        if (filtrarAbertos)
            url += "/abertos";
        return await _httpClient.GetAsync(url);
    }

    public async Task<HttpResponseMessage> GetEventoByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"{urlBase}/eventos/{id}");
    }

    public async Task<HttpResponseMessage> CreateEventoAsync(EventoDto evento)
    {
        var json = JsonConvert.SerializeObject(evento);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{urlBase}/eventos", content);
    }

    public async Task<HttpResponseMessage> UpdateEventoAsync(EventoDto evento)
    {
        var json = JsonConvert.SerializeObject(evento);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"{urlBase}/eventos/{evento.Id}", content);
    }

    public async Task<HttpResponseMessage> DeleteEventoAsync(int id)
    {
        return await _httpClient.DeleteAsync($"{urlBase}/eventos/{id}");
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}