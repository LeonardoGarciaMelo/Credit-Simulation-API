using Microsoft.EntityFrameworkCore;
using Moq;
using Simulador_de_Credito.Data;
using Simulador_de_Credito.DTO;
using Simulador_de_Credito.Service;

namespace Simulador_de_Credito.Tests
{
    public class SimulacaoServiceTests
    {
        private readonly Mock<ProdutoService> _produtoServiceMock;
        private readonly Mock<CalculoService> _calculoServiceMock;
        private readonly SqliteDbContext _contextInMemory;
        private readonly SimulacaoService _simulacaoService;

        public SimulacaoServiceTests()
        {
            var optionsSqlite = new DbContextOptionsBuilder<SqliteDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _contextInMemory = new SqliteDbContext(optionsSqlite);

            var optionsOracle = new DbContextOptionsBuilder<OracleDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var oracleContextMock = new OracleDbContext(optionsOracle);

            oracleContextMock.Produto.Add(new Model.Product
            {
                Id = 1,
                Nome = "Produto Teste",
                PcTaxaJuros = 0.0179m,
                NuMinimoMeses = 0,
                VrMinimo = 200
            });

            oracleContextMock.SaveChanges();

            var calculoServiceReal = new CalculoService();
            var produtoServiceReal = new ProdutoService(oracleContextMock);

            _simulacaoService = new SimulacaoService(calculoServiceReal, produtoServiceReal, _contextInMemory);
        }

        [Fact]
        public async Task Simular_DeveFuncionar_QuandoProdutoExiste()
        {
            var request = new SimulacaoRequestDTO
            {
                ValorDesejado = 1000,
                Prazo = 12
            };

            var response = await _simulacaoService.simular(request);

            Assert.NotNull(response);
            Assert.Equal(1, response.CodigoProduto);
            Assert.Equal(0.0179m, response.TaxaJuros);

            var simulacaoSalva = await _contextInMemory.Simulacoes.FirstOrDefaultAsync();
            Assert.NotNull(simulacaoSalva);
            Assert.Equal(1000, simulacaoSalva.ValorDesejado);
        }

        [Fact]
        public async Task Simular_DeveFalhar_QuandoProdutoNaoExiste()
        {
            var request = new SimulacaoRequestDTO
            {
                ValorDesejado = 10,
                Prazo = 12
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _simulacaoService.simular(request);
            });
        }
    }
}
