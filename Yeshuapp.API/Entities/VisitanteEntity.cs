using System.ComponentModel.DataAnnotations;

namespace Yeshuapp.Entities
{
    public class VisitanteEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime VisitDate { get; set; }
        public string? Notes { get; set; }
        public bool WantsContact { get; set; } = false;
    }
}
