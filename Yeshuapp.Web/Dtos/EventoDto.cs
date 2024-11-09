using Yeshuapp.Web.Enums;

namespace Yeshuapp.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Data { get; set; }
        public string Horario { get; set; }
        public ETipoEvento TipoEvento { get; set; }
        public string Detalhes { get; set; }
    }
}
