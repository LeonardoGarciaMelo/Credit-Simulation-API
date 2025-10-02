using Simulador_de_Credito.Data;
using Simulador_de_Credito.DTO;
using Simulador_de_Credito.Model;
using System.Threading.Tasks;

namespace Simulador_de_Credito.Service
{
    public class SimulacaoService
    {
        private readonly CalculoService _calculoService;
        private readonly ProdutoService _produtoService;
        private readonly SqliteDbContext _sqliteDbContext;

        public SimulacaoService(CalculoService calculoService, ProdutoService produtoService, SqliteDbContext sqliteDbContext)
        {
            _calculoService = calculoService;
            _produtoService = produtoService;
            _sqliteDbContext = sqliteDbContext;
        }

        public async Task<SimulacaoResponseDTO> simular(SimulacaoRequestDTO simulacaoRequestDTO)
        {
            var produto = await _produtoService.FindProduto(simulacaoRequestDTO);
            var parcelasSac = _calculoService.CalculaSac(simulacaoRequestDTO.ValorDesejado, simulacaoRequestDTO.Prazo, produto.PcTaxaJuros);
            var parcelasPrice = _calculoService.CalculaPrice(simulacaoRequestDTO.ValorDesejado, simulacaoRequestDTO.Prazo, produto.PcTaxaJuros);
            var resultados = new List<ResultadoSimulacaoDTO>
            {
                new ResultadoSimulacaoDTO("SAC", parcelasSac),
                new ResultadoSimulacaoDTO("PRICE", parcelasPrice)
            };

            decimal valorTotal = parcelasPrice.Sum(p => p.ValorPrestacao);

            var simulacao = new Simulacao
            {
                ValorDesejado = simulacaoRequestDTO.ValorDesejado,
                Prazo = simulacaoRequestDTO.Prazo,
                ValorTotalParcelas = valorTotal,
                CoProduto = produto.CoProduto,
            };

            // Salva no banco de dados SQLite
            _sqliteDbContext.Add(simulacao);
            await _sqliteDbContext.SaveChangesAsync();

            var response = new SimulacaoResponseDTO(
                simulacao.Id,
                produto.CoProduto,
                produto.NoProduto,
                produto.PcTaxaJuros,
                resultados
                );

            return response;
        }
    }
}
