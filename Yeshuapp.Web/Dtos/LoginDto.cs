using System.ComponentModel.DataAnnotations;

namespace Yeshuapp.Web.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }
    }
}
