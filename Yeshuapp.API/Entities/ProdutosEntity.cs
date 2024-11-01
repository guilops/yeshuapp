using System.ComponentModel.DataAnnotations;

namespace Yeshuapp.Entities
{
    public class ProdutosEntity
    {
        [Key]
        public int Id { get; set; }
        public string Imagem { get; set; }
        public decimal Valor { get; set; }
        public string? Nome { get; set; }
        public int Quantidade { get; set; }
    }
}
