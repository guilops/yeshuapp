namespace Yeshuapp.Dtos
{
    public class FraseDto
    {
        public int Id { get; set; }
        public string Passagem { get; set; }
        public string Livro { get; set; }
        public string Capitulo { get; set; }
        public string Versiculo { get; set; }
        public bool Ativa { get; set; }
    }

    public class PedidoOracaoDto
    {
        public string Mensagem { get; set; }
    }

    public class VisitanteDto
    {
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Notes { get; set; }
        public bool WantsContact { get; set; } = false;
    }
}
