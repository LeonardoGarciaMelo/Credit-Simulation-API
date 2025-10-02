using Microsoft.AspNetCore.Mvc;
using Simulador_de_Credito.DTO;
using Simulador_de_Credito.Service;

namespace Simulador_de_Credito.Controller
{
    [ApiController]
    [Route("api/simulacoes")]
    public class SimulacaoController : ControllerBase
    {
        private readonly SimulacaoService _simulacaoService;

        public SimulacaoController(SimulacaoService simulacaoService)
        {
            this._simulacaoService = simulacaoService;
        }
        [HttpPost("simular")]
       /* [ProducesResponseType(typeof(SimulacaoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]*/
        public async Task<IActionResult> Simular([FromBody] SimulacaoRequestDTO simulacaoRequest)
        {
            var response = await _simulacaoService.simular(simulacaoRequest);
            return Ok(response);
        }
    }
}
