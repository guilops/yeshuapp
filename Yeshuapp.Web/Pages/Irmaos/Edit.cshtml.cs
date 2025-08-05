using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Irmaos
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public ClienteResponseDto? Cliente { get; set; }
        private readonly IrmaosServices _irmaosServices;

        private readonly JsonSerializerOptions options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditModel(IrmaosServices irmaosServices,
                         IHttpContextAccessor httpContextAccessor)
        {
            _irmaosServices = irmaosServices;
            _httpContextAccessor = httpContextAccessor;

            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _irmaosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            var response = await _irmaosServices.GetIrmaoByIdAsync(id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Cliente = JsonSerializer.Deserialize<ClienteResponseDto>(json, options);
                Cliente.TelefoneCelular = FormatPhoneNumber(Cliente.TelefoneCelular);
                Cliente.TelefoneFixo = FormatPhoneNumber(Cliente.TelefoneFixo);
                return Page();
            }

            return RedirectToPage("/Irmaos/Index");
        }

        public async Task<IActionResult> OnPostAsync(IFormFile imagemFile)
        {
            _irmaosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            if (imagemFile != null && imagemFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagemFile.CopyToAsync(memoryStream);
                    Cliente.Imagem = Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            var response = await _irmaosServices.PutIrmaoAsync(Cliente);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Irmaos/Index");

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao editar o cliente.");
            return Page();
        }

        private string FormatPhoneNumber(string number)
        {
            var cleaned = new string(number.Where(char.IsDigit).ToArray());

            if (cleaned.Length == 11) 
            {
                return $"({cleaned.Substring(0, 2)}) {cleaned.Substring(2, 5)}-{cleaned.Substring(7)}";
            }
            else if (cleaned.Length == 10)
            {
                return $"({cleaned.Substring(0, 2)}) {cleaned.Substring(2, 4)}-{cleaned.Substring(6)}";
            }

            return number;
        }
    }
}