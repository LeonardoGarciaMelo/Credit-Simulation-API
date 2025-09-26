using Microsoft.AspNetCore.Mvc;
using Simulador_de_crédito.Data;

namespace Simulador_de_crédito.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly OracleDbContext _oracleContext;
        private readonly SqliteDbContext _sqliteContext;

        public TestController(OracleDbContext oracleContext, SqliteDbContext sqliteContext)
        {
            _oracleContext = oracleContext;
            _sqliteContext = sqliteContext;
        }
        [HttpGet("produtos")]
        public IActionResult GetProdutos()
        {
            var produtos = _oracleContext.Produto.ToList();
            return Ok(produtos);
        }
        [HttpGet("simulations")]
        public IActionResult GetSimulations()
        {
            var simulations = _sqliteContext.Simulacoes.ToList();
            return Ok(simulations);
        }
    }
}
