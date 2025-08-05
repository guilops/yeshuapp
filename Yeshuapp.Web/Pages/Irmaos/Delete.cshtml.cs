using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Irmaos
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public ClienteResponseDto Irmao { get; set; }
        private readonly IrmaosServices _irmaosServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly JsonSerializerOptions options;

        public DeleteModel(IrmaosServices irmaosServices,
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
                Irmao = JsonSerializer.Deserialize<ClienteResponseDto>(json, options);
                return Page();
            }

            return RedirectToPage("/Irmaos/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _irmaosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            var response = await _irmaosServices.DeleteIrmaoAsync(Irmao.Id);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Irmaos/Index");

            return RedirectToPage("/Irmaos/Index");
        }
    }
}