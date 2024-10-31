using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Irmaos
{
    public class IndexModel : PageModel
    {
        private readonly IrmaosServices _apiService;

        public IndexModel(IrmaosServices apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public List<ClienteResponseDto> Clientes { get; set; } = new List<ClienteResponseDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            var result = await _apiService.GetIrmaosAsync();

            if (!result.IsSuccessStatusCode)
                return Page();

            var clientes =  await result.Content.ReadFromJsonAsync<List<ClienteResponseDto>>();
            Clientes = clientes;

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