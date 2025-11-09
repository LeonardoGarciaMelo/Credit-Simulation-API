using Microsoft.EntityFrameworkCore;
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

        private const int DEFAULT_LIMITE_PAGINA = 20;
        private const int MAX_LIMITE_PAGINA = 100;

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

        /// <summary>
        /// Busca uma lista paginada de todas as simulações realizadas, aplicando as regras de negócio de paginação.
        /// </summary>
        /// <remarks>
        /// Este método é o responsável por "sanitizar" (validar) os parâmetros de paginação recebidos.
        /// 1. Garante que a 'pagina' seja sempre ao menos 1.
        /// 2. Garante que o 'limite' esteja dentro de um intervalo válido (entre 1 e MAX_LIMITE_PAGINA).
        /// 3. Executa uma consulta otimizada ao banco de dados usando AsNoTracking(), Skip() e Take().
        /// </remarks>
        /// <param name="pagina">O número da página solicitado (não validado).</param>
        /// <param name="limite">O número de registros por página solicitado (não validado).</param>
        /// <returns>
        /// Um <see cref="Task"/> que resolve para o <see cref="ResultadoListAllSimulacoesDTO"/> 
        /// contendo os dados da paginação e a lista de registros da página solicitada.
        /// </returns>
        public async Task<ResultadoListAllSimulacoesDTO> GetAllSimulacoes(int pagina, int limite)
        {
            if (pagina < 1)
            {
                pagina = 1;
            }

            if (limite < 1)
            {
                limite = DEFAULT_LIMITE_PAGINA;
            }

            if (limite > MAX_LIMITE_PAGINA)
            {
                limite = MAX_LIMITE_PAGINA;
            }

            var totalRegistros = await _sqliteDbContext.Simulacoes.CountAsync();

            var registros = await _sqliteDbContext.Simulacoes
                .AsNoTracking()
                .OrderByDescending(s => s.Data)
                .Skip((pagina - 1) * limite)
                .Take(limite)
                .Select(S => new RegistrosDTO
                {
                    Id = S.Id,
                    ValorDesejado = S.ValorDesejado,
                    Prazo = S.Prazo,
                    ValorTotalParcelas = S.ValorTotalParcelas,
                })
                .ToListAsync();

            return new ResultadoListAllSimulacoesDTO(
                Pagina: pagina,
                QtdRegistros: totalRegistros,
                QtdRegistrosPagina: registros.Count,
                Registros: registros
             );
        }
    }
}
