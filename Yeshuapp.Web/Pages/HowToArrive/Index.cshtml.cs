using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Yeshuapp.Web.Pages.HowToArrive;

public class IndexModel : PageModel
{
    public string Address { get; set; } = "Rua Joao Isidori, 46 - Vila Itaberaba, São Paulo BR";
    public void OnGet() {}
}
