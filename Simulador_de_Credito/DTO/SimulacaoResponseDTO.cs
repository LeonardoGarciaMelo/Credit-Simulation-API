using System.Text.Json.Serialization;

namespace Simulador_de_Credito.DTO
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

        public SimulacaoResponseDTO(Guid simulacaoId, int codigoProduto, string codigoDescricao, decimal taxaJuros, List<ResultadoSimulacaoDTO> resultadoSimulacao)
        {
            SimulacaoId = simulacaoId;
            CodigoProduto = codigoProduto;
            CodigoDescricao = codigoDescricao;
            TaxaJuros = taxaJuros;
            ResultadoSimulacao = resultadoSimulacao;
        }
    }
}
