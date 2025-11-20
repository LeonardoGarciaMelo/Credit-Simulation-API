using System.Text.Json.Serialization;

namespace Simulador_de_Credito.DTO
{
    public record SimulacaoPorDiaDTO
    {
        [JsonPropertyName("codigoProduto")]
        public int CodigoProduto { get; init; }

        [JsonPropertyName("descricaoProduto")]
        public string DescricaoProduto { get; init; }

        [JsonPropertyName("taxaMediaJuro")]
        public decimal TaxaMediaJuro { get; set; }

        [JsonPropertyName("valorMedioPrestacao")]
        public decimal ValorMedioPrestacao { get; init; }

        [JsonPropertyName("valorTotalDesejado")]
        public decimal ValorTotalDesejado { get; init; }

        [JsonPropertyName("valorTotalCredito")]
        public decimal ValorTotalCredito { get; init; }
    }
}
