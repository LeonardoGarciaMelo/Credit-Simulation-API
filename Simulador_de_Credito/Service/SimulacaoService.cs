using Simulador_de_Credito.Data;
using Simulador_de_Credito.DTO;
using Simulador_de_Credito.Model;
using System.Threading.Tasks;

namespace Simulador_de_Credito.Service
{
    /// <summary>
    /// Responsável pela lógica de negócio principal para realizar as simulações de crédito.
    /// </summary>
    /// <remarks>
    /// Este serviço busca os produtos, invoca os cálculos,
    /// persiste os resultados e prepara a resposta final para o cliente.
    /// </remarks>
    public class SimulacaoService
    {
        private readonly CalculoService _calculoService;
        private readonly ProdutoService _produtoService;
        private readonly SqliteDbContext _sqliteDbContext;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="SimulacaoService"/>.
        /// </summary>
        /// <param name="calculoService">O serviço responsável pelos cálculos de amortização.</param>
        /// <param name="produtoService">O serviço responsável por encontrar produtos de crédito.</param>
        /// <param name="sqliteDbContext">O contexto do banco de dados para persistir a simulação.</param>
        public SimulacaoService(CalculoService calculoService, ProdutoService produtoService, SqliteDbContext sqliteDbContext)
        {
            _calculoService = calculoService;
            _produtoService = produtoService;
            _sqliteDbContext = sqliteDbContext;
        }

        /// <summary>
        /// Executa o fluxo completo de uma simulação de crédito.
        /// </summary>
        /// <param name="simulacaoRequestDTO">Os dados de entrada da simulação, contendo valor e prazo.</param>
        /// <returns>
        /// Um <see cref="Task"/> que resolve para um <see cref="SimulacaoResponseDTO"/> contendo o resultado
        /// detalhado da simulação para os sistemas SAC e Price.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Lançada se nenhum produto de crédito compatível for encontrado pelos critérios da requisição.
        /// Esta exceção é tipicamente capturada pelo Controller para retornar um status HTTP 404.
        /// </exception>
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
