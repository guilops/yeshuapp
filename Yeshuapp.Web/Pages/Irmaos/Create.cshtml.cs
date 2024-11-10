using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Irmaos
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ClienteResponseDto Irmao { get; set; } = new ClienteResponseDto();
        private readonly IrmaosServices _irmaosServices;

        public CreateModel(IrmaosServices irmaosServices)
        {
            _irmaosServices = irmaosServices;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(IFormFile imagemFile)
        {
            _irmaosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            // Verifique se a imagem foi enviada
            if (imagemFile != null && imagemFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagemFile.CopyToAsync(memoryStream);
                    var imageBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    Irmao.Imagem = imageBase64; // Salve a imagem como Base64
                }
            }

            var response = await _irmaosServices.CreateIrmaoAsync(Irmao);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Irmaos/Index");
            }

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar o cliente.");
            return Page();
        }
    }
}
