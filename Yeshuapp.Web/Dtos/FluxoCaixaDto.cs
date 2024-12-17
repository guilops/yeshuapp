public class FluxoCaixaResponseDto
{
    public decimal TotalEntrada { get; set; }
    public decimal TotalSaida { get; set; }
    public decimal Saldo { get; set; }
    public List<FluxoCaixaDto> Itens { get; set; }
}

public class FluxoCaixaDto
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public string Tipo { get; set; } 
    public decimal Valor { get; set; }
    public string Origem { get; set; }
    public string Descricao { get; set; }
}
