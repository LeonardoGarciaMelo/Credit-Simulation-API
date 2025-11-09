using System.Text.Json.Serialization;

namespace Simulador_de_Credito.DTO
    /// <summary>
    /// Representa o registro de uma simulação 
    /// </summary>
    /// <remarks>
    /// Este objeto acessa o banco de dados simulacoes e pega os dados necessários, para retornar no endpoint de listar todas as simulacoes
    /// </remarks>
{
    public record RegistrosDTO
    {
        [JsonPropertyName("idSimulacao")]
        public Guid Id { get; init; }

        [JsonPropertyName("valorDesejado")]
        public decimal ValorDesejado { get; init; }

        [JsonPropertyName("prazo")]
        public short Prazo { get; init; }

        [JsonPropertyName("valorTotalParcelas")]
        public decimal ValorTotalParcelas { get; init; }
    }
}
