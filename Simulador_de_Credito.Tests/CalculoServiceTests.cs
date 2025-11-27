using Simulador_de_Credito.Service;

namespace Simulador_de_Credito.Tests
{
    public class CalculoServiceTests
    {
        private readonly CalculoService _calculoService;

        public CalculoServiceTests()
        {
            _calculoService = new CalculoService();
        }

        [Theory]
        [InlineData(1000, 10, 0.01, 100)] 
        [InlineData(2400, 24, 0.02, 100)]
        public void CalculaSac_DeveRetornarAmortizacaoConstante(decimal valor, short prazo, decimal taxa, decimal amortizacaoEsperada)
        {
            var resultado = _calculoService.CalculaSac(valor, prazo, taxa);
            Assert.Equal(prazo, resultado.Count); // Tem o número certo de parcelas?
            foreach(var parcela in resultado)
            {
                Assert.Equal(amortizacaoEsperada, parcela.ValorAmortizacao);
            }

            Assert.True(resultado[0].ValorPrestacao > resultado[prazo - 1].ValorPrestacao);
        }

        [Fact]
        public void CalculaPrice_DeveRetornarPrestacaoConstante()
        {
            decimal valor = 1000;
            short prazo = 12;
            decimal taxa = 0.01m; 

            var resultado = _calculoService.CalculaPrice(valor, prazo, taxa);

            Assert.Equal(prazo, resultado.Count);

            decimal valorPrimeiraPrestacao = resultado[0].ValorPrestacao;

            foreach (var parcela in resultado)
            {
                Assert.Equal(valorPrimeiraPrestacao, parcela.ValorPrestacao);
            }
        }
    }
}