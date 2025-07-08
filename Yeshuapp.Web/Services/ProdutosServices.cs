using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Yeshuapp.Web.Dtos;

public class ProdutosServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string urlBase = string.Empty;

    public ProdutosServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        urlBase = _configuration["baseApiUrl"];
    }

    public async Task<HttpResponseMessage> GetProdutosAsync()
    {
       return await _httpClient.GetAsync($"{urlBase}/produtos");
    }

    public async Task<HttpResponseMessage> GetProdutoByIdAsync(int id)
    {
        return await _httpClient.GetAsync($"{urlBase}/produtos/{id}");
    }

    public async Task<HttpResponseMessage> CreateProdutoAsync(ProdutoDto produto)
    {
        var json = JsonConvert.SerializeObject(produto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{urlBase}/produtos", content);
    }

    public async Task<HttpResponseMessage> UpdateProdutoAsync(ProdutoDto produto)
    {
        var json = JsonConvert.SerializeObject(produto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync($"{urlBase}/produtos/{produto.Id}", content);
    }

    public async Task<HttpResponseMessage> DeleteProdutoAsync(int id)
    {
        return await _httpClient.DeleteAsync($"{urlBase}/produtos/{id}");
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}