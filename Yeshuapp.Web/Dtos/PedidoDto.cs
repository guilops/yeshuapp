using Yeshuapp.Web.Enums;

namespace Yeshuapp.Web.Dtos
{
    public class PedidoDto
    {
        public int CodigoCliente { get; set; }
        public List<ProdutoPedidoDto> Produtos { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public bool StatusPedido { get; set; }
    }

    public class PedidoResponseDto
    {
        public int Id { get; set; }
        public int CodigoCliente { get; set; }
        public ClienteRequestDto Cliente { get; set; }
        public List<ProdutoPedidoDto> Produtos { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public EStatusPedido StatusPedido { get; set; }
    }
}
