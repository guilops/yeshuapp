using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yeshuapp.Entities
{
    public class PedidoProdutoEntity
    {
        [Key, Column(Order = 0)]
        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public PedidosEntity Pedido { get; set; }

        [Key, Column(Order = 1)]
        public int ProdutoId { get; set; }
        [ForeignKey("ProdutoId")]
        public ProdutosEntity Produto { get; set; }

        public int Quantidade { get; set; }
    }
}
