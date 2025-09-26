using System.Diagnostics.CodeAnalysis;

namespace Simulador_de_crédito.DTO
{
    public class SimulacaoRequest
    {
        [NotNull]
       public decimal ValorDesejado { get; set; }
       public short prazo { get; set; }


        public SimulacaoRequest(decimal ValorDesejado, short prazo) 
        {
            this.ValorDesejado = ValorDesejado;
            this.prazo = prazo;
        }
    }
}
