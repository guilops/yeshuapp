using Newtonsoft.Json;
using System.Text;
using Yeshuapp.Dtos;
using Yeshuapp.Web.Dtos;

public class AutenticacaoServices
{
    private readonly HttpClient _httpClient;

    public AutenticacaoServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

   
    public async Task<HttpResponseMessage> LoginAsync(LoginDto login)
    {
        var json = JsonConvert.SerializeObject(login);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync("https://localhost:44337/auth/login", content);
    }

    public async Task<HttpResponseMessage> CreateAsync(LoginDto login)
    {
        var json = JsonConvert.SerializeObject(login);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync("https://localhost:44337/auth/create", content);
    }

}