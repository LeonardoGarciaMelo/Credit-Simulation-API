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
    }
}
