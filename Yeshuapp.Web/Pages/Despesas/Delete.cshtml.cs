using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Dtos;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Despesas
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public DespesasResponseDto Despesas { get; set; }
        private readonly DespesasServices _despesasServices;

        private readonly JsonSerializerOptions options;

        public DeleteModel(DespesasServices despesasServices)
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

            return RedirectToPage("/Despesas/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _despesasServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            var response = await _despesasServices.DeleteDespesasAsync(Despesas.Id);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Despesas/Index");

            return RedirectToPage("/Despesas/Index");
        }
    }
}