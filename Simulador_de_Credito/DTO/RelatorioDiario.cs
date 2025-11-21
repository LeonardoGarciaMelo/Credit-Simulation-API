using System.Text.Json.Serialization;

namespace Simulador_de_Credito.DTO
{
    /// <summary>
    /// Objeto envelope que representa a resposta completa do endpoint de relatório diário.
    /// </summary>
    public record RelatorioDiario
    {
        [JsonPropertyName("dataReferencia")]
        public string DataReferencia { get; init; }

        [JsonPropertyName("simulacoes")]
        public List<SimulacaoPorDiaDTO> Simulacoes { get; init; }

        public RelatorioDiario(string dataReferencia, List<SimulacaoPorDiaDTO> simulacoes)
        {
            this.DataReferencia = dataReferencia;
            this.Simulacoes = simulacoes;
        }
    }
}
