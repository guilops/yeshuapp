namespace Yeshuapp.Dtos
{
    public class DespesasDto
    {
        public string? Descricao { get; set; }
        public string? Detalhes { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string? Anexo { get; set; }
    }

    public class DespesasResponseDto
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public string? Detalhes { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string? Anexo { get; set; }
    }
}
