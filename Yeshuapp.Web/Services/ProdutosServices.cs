using Yeshuapp.Web.Dtos;

public class ProdutosServices
{
    private readonly HttpClient _httpClient;

    public ProdutosServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProdutoDto>> GetClientesAsync()
    {
        var response = await _httpClient.GetAsync("https://localhost:44337/produtos");

        if (!response.IsSuccessStatusCode)
            return new List<ProdutoDto>();

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ProdutoDto>>();
    }

    public async Task<ProdutoDto> GetClienteByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:44337/produtos/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProdutoDto>();
    }

    public async Task CreateClienteAsync(ProdutoDto cliente)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:44337/produtos", cliente);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateClienteAsync(ProdutoDto cliente)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:44337/produtos", cliente);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteClienteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:44337/produtos/{id}");
        response.EnsureSuccessStatusCode();
    }
}