using Newtonsoft.Json;
using System.Text;
using Yeshuapp.Web.Dtos;

public class ProdutosServices
{
    private readonly HttpClient _httpClient;

    public ProdutosServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetProdutosAsync()
    {
       return await _httpClient.GetAsync("https://localhost:44337/produtos");
    }

    public async Task<HttpResponseMessage> GetProdutoByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"https://localhost:44337/produtos/{id}");
    }

    public async Task<HttpResponseMessage> CreateProdutoAsync(ProdutoDto produto)
    {
        var json = JsonConvert.SerializeObject(produto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync("https://localhost:44337/produtos", content);
    }

    public async Task<HttpResponseMessage> UpdateProdutoAsync(ProdutoDto produto)
    {
        var json = JsonConvert.SerializeObject(produto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"https://localhost:44337/produtos/{produto.Id}", content);
    }

    public async Task<HttpResponseMessage> DeleteProdutoAsync(int id)
    {
        return await _httpClient.DeleteAsync($"https://localhost:44337/produtos/{id}");
    }
}