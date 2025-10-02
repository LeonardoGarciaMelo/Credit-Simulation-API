using Simulador_de_Credito.DTO;
using System.Diagnostics.Metrics;

namespace Simulador_de_Credito.Service
{
    public class CalculoService
    {

        public List<ParcelaDTO> CalculaSac(decimal SaldoDevedor, short Meses, decimal TaxaFinanciamento)
        {
            List<ParcelaDTO> ListaRetorno = new List<ParcelaDTO>();
            decimal Prestacao;
            decimal saldo = SaldoDevedor;
            decimal amortizacao = SaldoDevedor / Meses;

            for (int i = 0; i < Meses; i++)
            {
                decimal juros = saldo * TaxaFinanciamento;
                Prestacao = amortizacao + juros;
                var Parcela = new ParcelaDTO 
                {
                    Numero = (i + 1),
                    ValorAmortizacao = Math.Round(amortizacao, 2),
                    ValorJuros = Math.Round(juros, 2),
                    ValorPrestacao = Math.Round(Prestacao, 2)
                };

                ListaRetorno.Add(Parcela);
                saldo = saldo - amortizacao;
            }

            return ListaRetorno;
        }

        public List<ParcelaDTO> CalculaPrice(decimal ValorAtualDebito, short Meses, decimal TaxaJuros)
        {
            List<ParcelaDTO> ListaRetorno = new List<ParcelaDTO>();
            decimal one = 1.0m;

            // Calcular (1 + taxaJuros)^meses
            decimal baseValue = one + TaxaJuros;
            decimal powered = (decimal)Math.Pow((double)baseValue, Meses); // Aproximação, ajuste para precisão

            // Calcular denominador: 1 - 1 / (1 + taxaJuros)^meses
            decimal denominador = one - (one / powered);

            // Calcular prestação: valorAtualDebito * taxaJuros / denominador
            decimal prestacao = (ValorAtualDebito * TaxaJuros) / denominador;
            prestacao = Math.Round(prestacao, 2);

            decimal saldo = ValorAtualDebito;
            decimal juros;
            decimal amortizacao;

            for (int i = 0; i < Meses; i++)
            {
                juros = saldo * TaxaJuros;
                amortizacao = prestacao - juros;
                var parcela = new ParcelaDTO
                {
                    Numero = (i + 1),
                    ValorAmortizacao = Math.Round(amortizacao, 2),
                    ValorJuros = Math.Round(juros, 2),
                    ValorPrestacao = Math.Round(prestacao, 2)
                };
                ListaRetorno.Add(parcela);
                saldo = saldo - amortizacao;
            }

            return ListaRetorno;
        }
    }
}
