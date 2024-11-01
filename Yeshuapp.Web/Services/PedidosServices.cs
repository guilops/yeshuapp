using Newtonsoft.Json;
using System.Text;
using Yeshuapp.Web.Dtos;
using Yeshuapp.Web.Enums;

public class PedidosServices
{
    private readonly HttpClient _httpClient;

    public PedidosServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetPedidosAsync()
    {
       return await _httpClient.GetAsync("https://localhost:44337/pedidos");
    }

    public async Task<HttpResponseMessage> GetPedidoByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"https://localhost:44337/pedidos/{id}");
    }

    public async Task<HttpResponseMessage> CreatePedidoAsync(PedidoResponseDto pedido)
    {
        var json = JsonConvert.SerializeObject(pedido);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync("https://localhost:44337/pedidos", content);
    }

    public async Task<HttpResponseMessage> UpdatePedidoAsync(PedidoResponseDto pedido)
    {
        var json = JsonConvert.SerializeObject(pedido);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"https://localhost:44337/pedidos/{pedido.Id}", content);
    }

    public async Task<HttpResponseMessage> DeletePedidoAsync(int id)
    {
        return await _httpClient.DeleteAsync($"https://localhost:44337/pedidos/{id}");
    }

    public async Task<HttpResponseMessage> NotificarPedidoAsync(int id, ECanalNotificacao eCanal)
    {
        throw new NotImplementedException();
    }
}