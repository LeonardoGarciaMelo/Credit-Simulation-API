using Microsoft.EntityFrameworkCore;
using Simulador_de_crédito.Data;
using Simulador_de_crédito.DTO;

namespace Simulador_de_crédito.Service
{
    public class ProdutoService
    {
        // Seu DbContext é injetado aqui pelo construtor
        private readonly OracleDbContext _context;

        public ProdutoService(OracleDbContext context)
        {
            _context = context;
        }

        /* /// <summary>
         /// Encontra o primeiro produto que atende aos critérios da simulação.
         /// A busca é feita de forma assíncrona e otimizada no banco de dados.
         /// </summary>
         /// <param name="simulacaoRequest">Os dados da simulação desejada.</param>
         /// <returns>A entidade do Produto encontrada.</returns>
         /// <exception cref="KeyNotFoundException">Lançada se nenhum produto for compatível.</exception>*/
        public async Task<ProdutoDTO> FindProduto(SimulacaoRequestDTO simulacaoRequest)
        {
            // O EF Core traduzirá o .Where() e o .Select() para uma única consulta SQL.
            var produtoEncontrado = await _context.Produto
                // 1. Filtra os produtos no banco de dados (cláusula WHERE em SQL)
                .Where(p =>
                    simulacaoRequest.ValorDesejado >= p.VrMinimo &&
                    (p.VrMaximo == null || simulacaoRequest.ValorDesejado <= p.VrMaximo) &&
                    simulacaoRequest.Prazo >= p.NuMinimoMeses &&
                    (p.NuMaximoMeses == null || simulacaoRequest.Prazo <= p.NuMaximoMeses)
                )
                // 2. Projeta o resultado diretamente em um ProdutoDTO (cláusula SELECT em SQL)
                .Select(p => new ProdutoDTO
                {
                    CoProduto = p.Id,
                    NoProduto = p.Nome,
                    PcTaxaJuros = p.PcTaxaJuros,
                    NuMinimoMeses = (short)p.NuMinimoMeses, // Cast explícito se os tipos forem diferentes
                    NuMaximoMeses = (short?)p.NuMaximoMeses,
                    VrMinimo = p.VrMinimo,
                    VrMaximo = p.VrMaximo
                })
                // 3. Pega o primeiro resultado ou nulo
                .FirstOrDefaultAsync();
            // Se FirstOrDefaultAsync não encontrar nada, ele retorna null.
            // Nós então lançamos uma exceção, como no código Java.
            if (produtoEncontrado == null)
            {
                throw new KeyNotFoundException("Nenhum produto foi encontrado para os parâmetros informados.");
            }

            return produtoEncontrado;
        }
    }
}
