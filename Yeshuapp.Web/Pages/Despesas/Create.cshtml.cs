using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Despesas
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public DespesasDto Despesas { get; set; } = new DespesasDto();
        private readonly DespesasServices _despesasServices;

        public CreateModel(DespesasServices despesasServices)
        {
            _despesasServices = despesasServices;
        }

        public void OnGetAsync() {}

        public async Task<IActionResult> OnPostAsync(IFormFile imagemFile)
        {
            _despesasServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);

            if (imagemFile != null && imagemFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagemFile.CopyToAsync(memoryStream);
                    var imageBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    Despesas.Anexo = imageBase64; 
                }
            }

            var response = await _despesasServices.CreateDespesasAsync(Despesas);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Despesas/Index");

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar a despesa.");
            return Page();
        }
    }
}
