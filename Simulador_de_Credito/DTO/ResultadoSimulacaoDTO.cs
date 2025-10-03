using System.Text.Json.Serialization;

namespace Simulador_de_Credito.DTO
{
    /// <summary>
    /// Agrupa o resultado completo de uma simulação para um sistema de amortização específico.
    /// </summary>
    /// <remarks>
    /// Por exemplo, um objeto deste tipo conteria o resultado para o sistema "SAC" e outro para o sistema "PRICE".
    /// </remarks>
    public record ResultadoSimulacaoDTO
    {
        /// <summary>
        /// O tipo do sistema de amortização calculado.
        /// </summary>
        /// <example>SAC</example>
        /// <example>PRICE</example>
        [JsonPropertyName("Tipo")]
        public string Tipo {  get; init; }

        /// <summary>
        /// Uma lista contendo os detalhes de cada parcela calculada para este sistema de amortização.
        /// </summary>
        [JsonPropertyName("Parcelas")]
        public List<ParcelaDTO> Parcelas { get; init; }

        /// <summary>
        /// Inicializa uma nova instância do DTO <see cref="ResultadoSimulacaoDTO"/>.
        /// </summary>
        /// <param name="tipo">O tipo do sistema de amortização (ex: "SAC").</param>
        /// <param name="parcelas">A lista de parcelas calculadas.</param>
        public ResultadoSimulacaoDTO(string tipo, List<ParcelaDTO> parcelas)
        {
            this.Tipo = tipo;
            this.Parcelas = parcelas;
        }
    }
}
