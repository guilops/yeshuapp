using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Yeshuapp.Web.Pages.FluxoCaixa
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public FluxoCaixaResponseDto Relatorio { get; set; }
        public DateTime Inicio { get; set; } = DateTime.Now.AddDays(-7); // �ltimos 7 dias
        public DateTime Fim { get; set; } = DateTime.Now;

        private readonly FluxoCaixaServices _fluxoCaixaServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(FluxoCaixaServices fluxoCaixaServices,
                        IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _fluxoCaixaServices = fluxoCaixaServices;
        }
        public async Task<IActionResult> OnGetAsync(DateTime? inicio, DateTime? fim)
        {
            Inicio = inicio ?? Inicio;
            Fim = fim ?? Fim;

            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Login");

            _fluxoCaixaServices.SetAuthorizationHeader(token);
            var result = await _fluxoCaixaServices.GetFluxoCaixaAsync($"dataInicio={Inicio:yyyy-MM-dd}&dataFim={Fim:yyyy-MM-dd}");

            if (!result.IsSuccessStatusCode)
                return Page();

            var fluxoCaixa = await result.Content.ReadFromJsonAsync<FluxoCaixaResponseDto>();
            Relatorio = fluxoCaixa;

            return Page();
        }
    }
}