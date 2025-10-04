using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Dtos;

namespace Yeshuapp.Web.Pages.Visitantes;

public class CreateModel : PageModel
{
    [BindProperty]
    public VisitanteDto VisitanteDto { get; set; } = new VisitanteDto();
    private readonly VisitanteServices _visitanteServices;
    public string ErrorMessage;

    public CreateModel(VisitanteServices visitanteServices)
    {
        _visitanteServices = visitanteServices;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        _visitanteServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);

        if (!ModelState.IsValid) return Page();
        var ok = await _visitanteServices.SalvarVisitantesAsync(VisitanteDto);
        if (ok != null && ok.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Ação realizada com sucesso";
            return Redirect("/Login");
        }

        ErrorMessage = "Ocorreu um erro ao salvar o visitante.";
        return Page();
    }
}
