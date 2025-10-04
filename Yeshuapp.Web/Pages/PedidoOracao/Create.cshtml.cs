using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Dtos;

namespace Yeshuapp.Web.Pages.PedidoOracao;

public class CreateModel : PageModel
{
    [BindProperty]
    public PedidoOracaoDto PedidoOracaoDto { get; set; } = new PedidoOracaoDto();
    private readonly PedidoOracaoServices _pedidoOracaoServices;
    public string ErrorMessage;

    public CreateModel(PedidoOracaoServices pedidoOracaoServices)
    {
        _pedidoOracaoServices = pedidoOracaoServices;
    }

    public void OnGet() {}

    public async Task<IActionResult> OnPostAsync()
    {
        _pedidoOracaoServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);

        if (!ModelState.IsValid) return Page();
        var ok = await _pedidoOracaoServices.CreatePedidoOracaoAsync(PedidoOracaoDto);
        if (ok != null && ok.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Ação realizada com sucesso";
            return Redirect("/Login");
        }

        ErrorMessage = "Ocorreu um erro ao enviar o pedido de oração.";
        return Page();
    }
}
