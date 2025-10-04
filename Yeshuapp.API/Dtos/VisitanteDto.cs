using Yeshuapp.API.Enums;

namespace Yeshuapp.Dtos
{
    public class VisitanteDto
    {
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Notes { get; set; }
        public bool WantsContact { get; set; } = false;
    }

}
