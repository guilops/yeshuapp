using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
using System.Drawing;

public class PaymentModel : PageModel
{
    public string QrCodeImage { get; set; }

    public void OnGet(int id)
    {
        string paymentInfo = "Pagamento de R$100,00 para o evento 'Culto de Oração' na data 01/12/2024 às 18:00. Código do evento: 12345";

        // Uso
        string chavePix = "(11)95368-7653";
        string nomeRecebedor = "Guilherme Lopes dos Santos";
        string cidade = "SAO PAULO";
        decimal valor = 5.00M;

        var qrCode = GerarPayloadPix(chavePix, nomeRecebedor, cidade, valor);
        QrCodeImage = GenerateQrCode(qrCode);

    }

    public static string GerarPayloadPix(string chavePix, string nomeRecebedor, string cidade, decimal valor)
    {
        string payload = $"000201" + // Payload Format Indicator
                         $"010211" + // Point of Initiation Method
                         $"26330014BR.GOV.BCB.PIX01{chavePix.Length:D2}{chavePix}" + // Chave PIX
                         $"52040000" + // Merchant Category Code (sem categoria)
                         $"5303986" +  // Transaction Currency (986 = BRL)
                         $"54{valor:F2}".Replace(".", "").PadLeft(6, '0') + // Transaction Amount
                         $"5802BR" +   // Country Code
                         $"59{nomeRecebedor.Length:D2}{nomeRecebedor}" + // Nome do recebedor
                         $"60{cidade.Length:D2}{cidade}" + // Cidade do recebedor
                         "62070503***" + // Additional Data Field Template
                         "6304"; // CRC placeholder

        // Calcular CRC16 e inserir no payload
        string crc16 = CalcularCRC16(payload);
        return payload + crc16;
    }

    public string GenerateQrCode(string paymentInfo)
    {
        // Inicializa o gerador de QR Code
        QRCodeGenerator qrGenerator = new QRCodeGenerator();

        // Gera os dados do QR Code
        var qrCodeData = qrGenerator.CreateQrCode(paymentInfo, QRCodeGenerator.ECCLevel.Q);

        // Gera o QR Code como array de bytes de imagem com menor densidade
        BitmapByteQRCode bitmapByteQRCode = new BitmapByteQRCode(qrCodeData);
        byte[] qrCodeImage = bitmapByteQRCode.GetGraphic(3); // Reduza de 20 para 10 para diminuir o tamanho

        // Converte o array de bytes para uma string Base64
        return Convert.ToBase64String(qrCodeImage);
    }

    private static string CalcularCRC16(string payload)
    {
        ushort polynomial = 0x1021;
        ushort crc = 0xFFFF;

        foreach (char c in payload)
        {
            crc ^= (ushort)(c << 8);
            for (byte j = 0; j < 8; j++)
            {
                if ((crc & 0x8000) != 0)
                    crc = (ushort)((crc << 1) ^ polynomial);
                else
                    crc <<= 1;
            }
        }
        return crc.ToString("X4");
    }
}
