using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Data { get; set; }
        public EStatusPedido StatusPedido { get; set; }
    }
}
