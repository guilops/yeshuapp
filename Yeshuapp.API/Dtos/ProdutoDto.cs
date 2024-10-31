namespace Yeshuapp.Dtos
{
    public class ProdutoPedidoDto
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
    }

    public class ProdutoDto
    {
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
    }
}
