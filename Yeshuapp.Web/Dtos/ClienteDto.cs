namespace Yeshuapp.Web.Dtos
{
    public class ClienteRequestDto
    {
        public string? Imagem { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneFixo { get; set; }
        public string Email { get; set; }
        public string DataNascimento { get; set; }
        public string Sexo { get; set; }
    }

    public class ClienteResponseDto
    {
        public int Id { get; set; }
        public string? Imagem { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneFixo { get; set; }
        public string Email { get; set; }
        public string DataNascimento { get; set; }
        public string Sexo { get; set; }
    }
}
