using Newtonsoft.Json;
using System.Text;
using Yeshuapp.Dtos;

public class EventosServices
{
    private readonly HttpClient _httpClient;

    public EventosServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetEventosAsync()
    {
       return await _httpClient.GetAsync("https://localhost:44337/eventos");
    }

    public async Task<HttpResponseMessage> GetEventoByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"https://localhost:44337/eventos/{id}");
    }

    public async Task<HttpResponseMessage> CreateEventoAsync(EventoDto evento)
    {
        var json = JsonConvert.SerializeObject(evento);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync("https://localhost:44337/eventos", content);
    }

    public async Task<HttpResponseMessage> UpdateEventoAsync(EventoDto evento)
    {
        var json = JsonConvert.SerializeObject(evento);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"https://localhost:44337/eventos/{evento.Id}", content);
    }

    public async Task<HttpResponseMessage> DeleteEventoAsync(int id)
    {
        return await _httpClient.DeleteAsync($"https://localhost:44337/eventos/{id}");
    }

    internal void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}