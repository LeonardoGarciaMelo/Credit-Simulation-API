using Microsoft.AspNetCore.Mvc;
using Simulador_de_Credito.DTO;
using Simulador_de_Credito.Service;

namespace Simulador_de_Credito.Controller
{
    /// <summary>
    /// Controlador responsável por gerenciar as simulações de crédito.
    /// </summary>
    [ApiController]
    [Route("api/simulacoes")]
    public class SimulacaoController : ControllerBase
    {
        private readonly SimulacaoService _simulacaoService;

        public SimulacaoController(SimulacaoService simulacaoService)
        {
            this._simulacaoService = simulacaoService;
        }
        /// <summary>
        /// Realiza uma nova simulação de crédito com base nos dados fornecidos.
        /// </summary>
        /// <remarks>
        /// Este endpoint consulta os produtos de crédito disponíveis, encontra um compatível com o valor e prazo desejados,
        /// calcula as parcelas pelos sistemas SAC e Price, e persiste o resultado da simulação.
        /// </remarks>
        /// <param name="simulacaoRequest">Objeto JSON contendo o valor desejado e o prazo em meses para a simulação.</param>
        /// <response code="200">Retorna o resultado completo da simulação, incluindo os detalhes das parcelas.</response>
        /// <response code="404">Retorna se nenhum produto de crédito compatível for encontrado para os parâmetros informados.</response>
        /// <response code="400">Retorna se os dados de entrada forem inválidos (ex: valores negativos).</response>
        [HttpPost("simular")]
        [ProducesResponseType(typeof(SimulacaoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Simular([FromBody] SimulacaoRequestDTO simulacaoRequest)
        {
            try
            {
                var response = await _simulacaoService.simular(simulacaoRequest);
                return Ok(response);
            }
            catch (KeyNotFoundException ex) 
            { 
                return NotFound(new {Erro = ex.Message});
            }
        }

        /// <summary>
        /// Lista todas as simulações de crédito já realizadas, de forma paginada.
        /// </summary>
        /// <remarks>
        /// Retorna um objeto com os detalhes da paginação e a lista de simulações salvas no banco de dados.
        /// Este endpoint delega a lógica de validação dos parâmetros e a consulta
        /// diretamente para o Serviço de Simulação.
        /// </remarks>
        /// <param name="pagina">O número da página desejada. Se não for fornecido ou for inválido, o padrão será 1.</param>
        /// <param name="limite">O número de registros por página. Se não for fornecido ou for inválido, o padrão será 20.</param>
        /// <returns>Uma lista paginada de simulações.</returns>
        /// <response code="200">Retorna o objeto ResultadoListAllSimulacoesDTO com a lista paginada de simulações.</response>
        /// <response code="500">Retorna caso ocorra um erro de conexão com o banco de dados ou falha interna.</response>

        [HttpGet("listar")]
        [ProducesResponseType(typeof(ResultadoListAllSimulacoesDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarSimulacoes([FromQuery] int pagina = 1, [FromQuery] int limite = 20)
        {
            try
            {
                var resultado = await _simulacaoService.GetAllSimulacoes(pagina, limite);
                return Ok(resultado);
            }
            catch (KeyNotFoundException ex) 
            {
                return StatusCode(500, new { Erro = "Erro ao buscar histórico de simulações.", Detalhes = ex.Message });
            }
            
        }

        /// <summary>
        /// Retorna o volume simulado por produto em um dia específico.
        /// </summary>
        /// <remarks>
        /// Este endpoint processa um relatório consolidado de todas as simulações realizadas na data informada.
        /// Ele agrupa os dados locais (SQLite) e enriquece com as informações cadastrais do produto (Oracle).
        /// </remarks>
        /// <param name="data">A data de referência para o relatório no formato AAAA-MM-DD (Ex: 2025-07-30).</param>
        /// <returns>Um objeto contendo a data e a lista de volumes por produto.</returns>
        /// <response code="200">Retorna o relatório gerado com sucesso.</response>
        /// <response code="400">Retorna quando a data fornecida não está no formato válido (AAAA-MM-DD).</response>
        /// <response code="404">Retorna quando não são encontrados dados para processar (caso o serviço lance KeyNotFound).</response>
        /// <response code="500">Retorna em caso de erro interno no servidor (ex: falha no banco de dados).</response>
        [HttpGet("volume-diario/{data}")]
        [ProducesResponseType(typeof(RelatorioDiario), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVolumePorDia([FromRoute] string data)
        {
            try
            {
                if (!DateTime.TryParse(data, out var dataReferencia))
                {
                    return BadRequest(new { Mensagem = "Data inválida. Use o formato YYYY-MM-DD." });
                }

                var relatorio = await _simulacaoService.GetVolumePorDiaAsync(dataReferencia);

                return Ok(relatorio);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Erro = ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Erro = "Ocorreu um erro interno ao gerar o relatório.", Detalhes = ex.Message });
            }
        }
    }
}
