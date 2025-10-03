using System.Text.Json.Serialization;

namespace Simulador_de_Credito.DTO
{
    /// <summary>
    /// Representa a resposta completa e detalhada de uma simulação de crédito bem-sucedida.
    /// </summary>
    /// <remarks>
    /// Este objeto é retornado no corpo (body) de uma resposta HTTP 200 OK do endpoint de simulação.
    /// </remarks>
    public record SimulacaoResponseDTO
    {
        /// <summary>
        /// O identificador único da simulação que foi gerado e salvo no banco de dados.
        /// </summary>
        [JsonPropertyName("IdSimulacao")]
        public Guid SimulacaoId { get; init; }

        /// <summary>
        /// O código de identificação do produto de crédito utilizado na simulação.
        /// </summary>
        [JsonPropertyName("CodigoProduto")]
        public int CodigoProduto { get; init; }

        /// <summary>
        /// O nome comercial do produto de crédito utilizado.
        /// </summary>
        [JsonPropertyName("DescricaoProduto")]
        public string CodigoDescricao { get; init; }

        /// <summary>
        /// A taxa de juros mensal, em formato decimal, que foi aplicada na simulação.
        /// </summary>
        [JsonPropertyName("TaxaJuros")]
        public decimal TaxaJuros { get; init; }

        /// <summary>
        /// Uma lista contendo os resultados detalhados dos cálculos para cada sistema de amortização (SAC e Price).
        /// </summary>
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
