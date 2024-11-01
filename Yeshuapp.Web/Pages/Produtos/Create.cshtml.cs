using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Produtos
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ProdutoDto Produto { get; set; } = new ProdutoDto();
        private readonly ProdutosServices _produtosServices;

        public CreateModel(ProdutosServices produtosServices)
        {
            _produtosServices = produtosServices;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(IFormFile imagemFile)
        {
            // Verifique se a imagem foi enviada
            if (imagemFile != null && imagemFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagemFile.CopyToAsync(memoryStream);
                    var imageBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    Produto.Imagem = imageBase64; // Salve a imagem como Base64
                }
            }

            var response = await _produtosServices.CreateProdutoAsync(Produto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Produtos/Index");
            }

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar o cliente.");
            return Page();
        }
    }
}
