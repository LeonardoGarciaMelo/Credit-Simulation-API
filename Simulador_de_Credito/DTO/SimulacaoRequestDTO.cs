using System.ComponentModel.DataAnnotations;

namespace Simulador_de_Credito.DTO
{
    /// <summary>
    /// Representa os dados de entrada necessários para iniciar uma nova simulação de crédito.
    /// </summary>
    /// <remarks>
    /// Este objeto deve ser enviado no corpo (body) de uma requisição POST para o endpoint de simulação.
    /// </remarks>
    public record SimulacaoRequestDTO
    {
        /// <summary>
        /// O valor principal do empréstimo que o cliente deseja simular.
        /// </summary>
        /// <example>10000.50</example>
       [Required(ErrorMessage = "O valor desejado não pode ser nulo.")]
       public decimal ValorDesejado { get; init; }

        /// <summary>
        /// O número de meses (parcelas) em que o cliente deseja pagar o empréstimo.
        /// </summary>
        /// <example>24</example>
        public short Prazo { get; init; }
    }
}
