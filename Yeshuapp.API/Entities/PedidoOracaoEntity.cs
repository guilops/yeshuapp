using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yeshuapp.Entities
{
    public class PedidoOracaoEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
