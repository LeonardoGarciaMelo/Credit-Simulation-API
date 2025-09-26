using System.ComponentModel.DataAnnotations;

namespace Simulador_de_crédito.DTO
{
    public record SimulacaoRequestDTO
    {
       [Required(ErrorMessage = "O valor desejado não pode ser nulo.")]
       public decimal ValorDesejado { get; init; }
       public short Prazo { get; init; }
    }
}
