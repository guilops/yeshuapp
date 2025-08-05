using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;
using Yeshuapp.Web.Dtos;
using Yeshuapp.Web.Enums;

namespace Yeshuapp.Web.Pages.Pedidos
{
    public class NotifyModel : PageModel
    {
        [BindProperty]
        public PedidoResponseDto Pedido { get; set; }
        private readonly PedidosServices _pedidosServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly JsonSerializerOptions options;

        public NotifyModel(PedidosServices pedidosServices, IHttpContextAccessor httpContextAccessor)
        {
            _pedidosServices = pedidosServices;
            _httpContextAccessor = httpContextAccessor;
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _pedidosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            var response = await _pedidosServices.GetPedidoByIdAsync(id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Pedido = System.Text.Json.JsonSerializer.Deserialize<PedidoResponseDto>(json, options);
                return Page();
            }

            return RedirectToPage("/Pedidos/Index");
        }

        public async Task<IActionResult> OnPostAsync(string metodoNotificacao)
        {
            _pedidosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            var eCanal = (ECanalNotificacao)Enum.Parse(typeof(ECanalNotificacao), metodoNotificacao, true);

            var response = await _pedidosServices.GetPedidoByIdAsync(Pedido.Id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Pedido = System.Text.Json.JsonSerializer.Deserialize<PedidoResponseDto>(json, options);

                var mensagemEnviar = MontarMensagemNotificacao(Pedido);

                if (eCanal == ECanalNotificacao.Whatsapp)
                    EnviarMensagemWhatsApp(Pedido.Cliente.TelefoneCelular, mensagemEnviar);
                else
                    EnviarMensagemEmail(Pedido.Cliente.Email, mensagemEnviar);
            }

            return RedirectToPage("/Pedidos/Index");
        }

        private void EnviarMensagemEmail(string emailDestinatario, string mensagemEnviar)
        {
            // Configura��es do servidor SMTP
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 465;
            string emailRemetente = "guilherme.glsantos@gmail.com";
            string senhaRemetente = "Saoirseronan13@@";

            try
            {
                // Configura a mensagem de e-mail
                MailMessage mensagem = new MailMessage();
                mensagem.From = new MailAddress(emailRemetente);
                mensagem.To.Add(emailDestinatario);
                mensagem.Subject = "Lembrete - Casa de Ora��o Yeshua";
                mensagem.Body = mensagemEnviar;
                mensagem.IsBodyHtml = true;

                // Configura o cliente SMTP
                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(emailRemetente, senhaRemetente),
                    EnableSsl = true
                };

                // Envia o e-mail
                smtpClient.Send(mensagem);
                Console.WriteLine("E-mail enviado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}");
            }
        }

        private string MontarMensagemNotificacao(PedidoResponseDto? pedido)
        {
            var mensagem = new StringBuilder();

            mensagem.AppendLine($"Ol� {pedido.Cliente.Nome}, tudo bem?");
            mensagem.AppendLine($"Apenas gostar�amos de te lembrar que existe um pedido em aberto na casa de Ora��o Yeshua.");
            mensagem.AppendLine($"Data do Pedido: {pedido.Data.ToShortDateString()}, Valor: {pedido.Valor}");
            mensagem.AppendLine($"Se j� tiver efetuado o pagamento, desconsidere essa mensagem");

            return mensagem.ToString();

        }

        public static void EnviarMensagemWhatsApp(string numero, string mensagem)
        {
            // Remove caracteres indesejados do n�mero
            numero = numero.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            if (!numero.StartsWith("11"))
                numero = $"11{numero}";

            if (!numero.StartsWith("55"))
                numero = $"55{numero}";

            // Codifica a mensagem para ser usada na URL
            string mensagemCodificada = HttpUtility.UrlEncode(mensagem);

            // Monta a URL para o wa.me
            string url = $"https://wa.me/{numero}?text={mensagemCodificada}";

            // Abre a URL no navegador padr�o
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}