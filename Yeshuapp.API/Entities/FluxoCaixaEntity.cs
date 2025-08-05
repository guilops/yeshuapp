using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Yeshuapp.API.Enums;

namespace Yeshuapp.Entities
{
    public class FluxoCaixaEntity
    {
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Data { get; set; }
        public string Tipo { get; set; } 
        public decimal Valor { get; set; }
        public string Origem { get; set; }
        public string Descricao { get; set; }
    }
}
