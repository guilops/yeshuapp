using System.ComponentModel.DataAnnotations;
using Yeshuapp.API.Enums;

namespace Yeshuapp.Entities
{
    public class EventosEntity
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Data { get; set; }
        public string Horario { get; set; }
        public ETipoEvento TipoEvento { get; set; }
        public string Detalhes { get; set; }
    }
}
