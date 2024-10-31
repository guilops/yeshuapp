using System.ComponentModel.DataAnnotations;

namespace Yeshuapp.Entities
{
    public class ClientesEntity
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneFixo { get; set; }
        public string Email { get; set; }
        public string Sexo { get; set; }
        public string DataNascimento { get; set; }
        public string Imagem { get; set; }
    }
}
