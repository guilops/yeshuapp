using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Dtos;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Despesas
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public DespesasResponseDto? Despesas { get; set; }
        private readonly DespesasServices _despesasServices;

        private readonly JsonSerializerOptions options;

        public EditModel(DespesasServices despesasServices)
        {
            _despesasServices = despesasServices;
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _despesasServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            var response = await _despesasServices.GetDespesasByIdAsync(id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Despesas = JsonSerializer.Deserialize<DespesasResponseDto>(json, options);
                return Page();
            }

            return RedirectToPage("/Eventos/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _despesasServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            var response = await _despesasServices.UpdateDespesasAsync(Despesas);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Despesas/Index");

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao editar essa despesa.");
            return Page();
        }
    }
}