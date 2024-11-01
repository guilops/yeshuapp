namespace Yeshuapp.Dtos
{
    public class ProdutoPedidoDto
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public ProdutoDto Produto { get; set; }
    }

    public class ProdutoDto
    {
        public string Imagem { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
    }
}
