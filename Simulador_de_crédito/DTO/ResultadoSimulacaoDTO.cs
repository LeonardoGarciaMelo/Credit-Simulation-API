using System.Text.Json.Serialization;

namespace Simulador_de_crédito.DTO
{
    public record ResultadoSimulacaoDTO
    {
        [JsonPropertyName("Tipo")]
        public string Tipo {  get; init; }

        [JsonPropertyName("Parcelas")]
        public List<ParcelaDTO> Parcelas { get; init; }

        public ResultadoSimulacaoDTO(string tipo, List<ParcelaDTO> parcelas)
        {
            this.Tipo = tipo;
            this.Parcelas = parcelas;
        }
    }
}
