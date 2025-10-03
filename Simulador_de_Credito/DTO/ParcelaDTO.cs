using System.Text.Json.Serialization;

namespace Simulador_de_Credito.DTO
{
    /// <summary>
    /// Representa os dados detalhados de uma única parcela de uma simulação de crédito.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para transferir as informações de cada parcela
    /// calculada nos sistemas de amortização (SAC ou Price).
    /// </remarks>
    public record ParcelaDTO
    {
        /// <summary>
        /// O número sequencial da parcela no plano de pagamento (ex: 1, 2, 3...).
        /// </summary>
        [JsonPropertyName("numero")]
        public int Numero { get; init; }


        /// <summary>
        ///   O valor da parcela que é destinado a abater o saldo devedor principal.
        /// </summary>
        [JsonPropertyName("valorAmortizacao")]
        public decimal ValorAmortizacao { get; init; }

        /// <summary>
        ///   O valor da parcela que corresponde aos juros calculados sobre o saldo devedor do período.
        /// </summary>
        [JsonPropertyName("valorJuros")]
        public decimal ValorJuros { get; init; }

        /// <summary>
        ///  O valor total da parcela a ser paga, correspondendo à soma da amortização e dos juros(Amortização + Juros).
        /// </summary>
        [JsonPropertyName("valorPrestacao")]
        public decimal ValorPrestacao { get; init; }

    }
}
