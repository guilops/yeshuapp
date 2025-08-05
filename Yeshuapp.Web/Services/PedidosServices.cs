using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Yeshuapp.Web.Dtos;
using Yeshuapp.Web.Enums;

public class PedidosServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string urlBase = string.Empty;

    public PedidosServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        urlBase = "https://yeshuapp.onrender.com";
    }

    public async Task<HttpResponseMessage> GetPedidosAsync()
    {
       return await _httpClient.GetAsync($"{urlBase}/pedidos");
    }

    public async Task<HttpResponseMessage> GetPedidoByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"{urlBase}/pedidos/{id}");
    }

    public async Task<HttpResponseMessage> CreatePedidoAsync(PedidoResponseDto pedido)
    {
        var json = JsonConvert.SerializeObject(pedido);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{urlBase}/pedidos", content);
    }

    public async Task<HttpResponseMessage> UpdatePedidoAsync(PedidoResponseDto pedido)
    {
        var json = JsonConvert.SerializeObject(pedido);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"{urlBase}/pedidos/{pedido.Id}", content);
    }

    public async Task<HttpResponseMessage> DeletePedidoAsync(int id)
    {
        return await _httpClient.DeleteAsync($"{urlBase}/pedidos/{id}");
    }

    public async Task<HttpResponseMessage> NotificarPedidoAsync(int id, ECanalNotificacao eCanal)
    {
        throw new NotImplementedException();
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}