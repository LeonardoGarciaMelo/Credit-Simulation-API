using System.Text.Json.Serialization;

namespace Simulador_de_Credito.DTO
{
    public record ParcelaDTO
    {
        [JsonPropertyName("numero")]
        public int Numero { get; init; }

        [JsonPropertyName("valorAmortizacao")]
        public decimal ValorAmortizacao { get; init; }

        [JsonPropertyName("valorJuros")]
        public decimal ValorJuros { get; init; }

        [JsonPropertyName("valorPrestacao")]
        public decimal ValorPrestacao { get; init; }

    }
}
