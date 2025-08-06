using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Despesas
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<DespesasResponseDto>? Despesas { get; set; }

        private readonly DespesasServices _despesasServices;

        public IndexModel(DespesasServices despesasServices)
        {
            _despesasServices = despesasServices;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Login");

            _despesasServices.SetAuthorizationHeader(token);
            var result = await _despesasServices.GetDespesasAsync();

            if (!result.IsSuccessStatusCode)
                return Page();

            var despesas = await result.Content.ReadFromJsonAsync<List<DespesasResponseDto>>();
            Despesas = despesas;

            return Page();
        }
    }
}