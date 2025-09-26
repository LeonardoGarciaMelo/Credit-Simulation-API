namespace Simulador_de_crédito.DTO
{
    public record SimulacaoDTO
    {
        public Guid Id { get; init; }
        public DateTimeOffset Data {  get; init; }
        public decimal ValorDesejado { get; init; }
        public short Prazo { get; init; }
        public decimal ValorTotalParcelas { get; init; }
    }
}
