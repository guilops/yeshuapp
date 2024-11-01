using System.ComponentModel.DataAnnotations;
using Yeshuapp.API.Enums;

namespace Yeshuapp.Entities
{
    public class PedidosEntity
    {
        [Key]
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public ClientesEntity Cliente { get; set; }
        public List<PedidoProdutoEntity> PedidoProdutos { get; set; } = new();
        public DateTime Data { get; set; }
        public EStatusPedido StatusPedido { get; set; }
    }
}
