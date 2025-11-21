using System.Text.Json.Serialization;

namespace Simulador_de_Credito.DTO
{
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
