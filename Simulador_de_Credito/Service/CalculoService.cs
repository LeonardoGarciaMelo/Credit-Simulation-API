using Simulador_de_Credito.DTO;

namespace Simulador_de_Credito.Service
{
    /// <summary>
    /// Fornece os métodos para calcular as parcelas de um financiamento pelos sistemas SAC e Price.
    /// </summary>
    /// <remarks>
    /// Este é um serviço sem estado (stateless) que contém a lógica de negócio financeira principal da aplicação.
    /// </remarks>
    public class CalculoService
    {
        /// <summary>
        /// Calcula o plano de pagamento de um financiamento pelo Sistema de Amortização Constante (SAC).
        /// </summary>
        /// <remarks>
        /// A principal característica do sistema SAC é que o valor da amortização é o mesmo em todas as parcelas,
        /// enquanto os juros diminuem a cada pagamento, resultando em prestações decrescentes.
        /// </remarks>
        /// <param name="saldoDevedor">O valor total do empréstimo a ser financiado.</param>
        /// <param name="meses">O número total de parcelas (prazo) do financiamento.</param>
        /// <param name="taxaFinanciamento">A taxa de juros mensal em formato decimal (ex: 0.0179 para 1.79%).</param>
        /// <returns>Uma lista de objetos `ParcelaDTO`, cada um representando uma parcela do financiamento.</returns>
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

        /// <summary>
        /// Calcula o plano de pagamento de um financiamento pelo Sistema Price (ou Tabela Price).
        /// </summary>
        /// <remarks>
        /// A principal característica do sistema Price é que o valor da prestação é o mesmo em todas as parcelas.
        /// No início, a maior parte da prestação é composta por juros, e no final, pela amortização.
        /// </remarks>
        /// <param name="valorAtualDebito">O valor total do empréstimo a ser financiado.</param>
        /// <param name="meses">O número total de parcelas (prazo) do financiamento.</param>
        /// <param name="taxaJuros">A taxa de juros mensal em formato decimal (ex: 0.0179 para 1.79%).</param>
        /// <returns>Uma lista de objetos `ParcelaDTO`, cada um representando uma parcela do financiamento.</returns>
        public List<ParcelaDTO> CalculaPrice(decimal ValorAtualDebito, short Meses, decimal TaxaJuros)
        {
            List<ParcelaDTO> ListaRetorno = new List<ParcelaDTO>();
            decimal one = 1.0m;

            decimal baseValue = one + TaxaJuros;
            decimal powered = (decimal)Math.Pow((double)baseValue, Meses); // Aproximação, ajuste para precisão

            decimal denominador = one - (one / powered);
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
