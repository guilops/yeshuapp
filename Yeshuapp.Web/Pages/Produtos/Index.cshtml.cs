using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Produtos
{
    public class IndexModel : PageModel
    {
        private readonly ProdutosServices _produtosServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(ProdutosServices produtosServices, IHttpContextAccessor httpContextAccessor)
        {
            _produtosServices = produtosServices;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public List<ProdutoDto>? Produtos { get; set; } = new List<ProdutoDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Login");

            _produtosServices.SetAuthorizationHeader(token);

            var result = await _produtosServices.GetProdutosAsync();

            if (!result.IsSuccessStatusCode)
                return Page();

            var produtos =  await result.Content.ReadFromJsonAsync<List<ProdutoDto>>();
            Produtos = produtos;

            return Page();
        }

        public string FormatarTelefone(string telefone)
        {
            if (string.IsNullOrEmpty(telefone))
                return string.Empty;

            // Remover caracteres n�o num�ricos
            telefone = telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace(".", "");

            // Verifica se o telefone possui o formato correto para a m�scara
            if (telefone.Length == 11) // Para celular
            {
                return $"({telefone.Substring(0, 2)}) {telefone.Substring(2, 5)}-{telefone.Substring(7)}";
            }
            else if (telefone.Length == 10) // Para telefone fixo
            {
                return $"({telefone.Substring(0, 2)}) {telefone.Substring(2, 4)}-{telefone.Substring(6)}";
            }

            return telefone; // Retorna sem formata��o se n�o atender aos crit�rios
        }
    }
}