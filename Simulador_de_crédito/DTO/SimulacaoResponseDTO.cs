using System.Text.Json.Serialization;

namespace Simulador_de_crédito.DTO
{
    public record SimulacaoResponseDTO
    {
        [JsonPropertyName("IdSimulacao")]
        public Guid SimulacaoId { get; init; }
        [JsonPropertyName("CodigoProduto")]
        public int CodigoProduto { get; init; }
        [JsonPropertyName("DescricaoProduto")]
        public string CodigoDescricao { get; init; }
        [JsonPropertyName("TaxaJuros")]
        public decimal TaxaJuros { get; init; }
        [JsonPropertyName("Resultados")]
        public List<ResultadoSimulacaoDTO> ResultadoSimulacao { get; init; }
    }
}
