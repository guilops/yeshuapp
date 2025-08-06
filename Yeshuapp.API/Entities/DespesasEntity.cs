using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yeshuapp.Entities
{
    public class DespesasEntity
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public string Anexo { get; set; }
        public decimal Valor { get; set; }
        public string Detalhes { get; set; }

    }
}
