using Microsoft.EntityFrameworkCore;
using Simulador_de_Credito.Data;
using Simulador_de_Credito.DTO;

namespace Simulador_de_Credito.Service
{
    /// <summary>
    /// Fornece serviços para consultar e encontrar produtos de crédito.
    /// </summary>
    /// <remarks>
    /// Este serviço encapsula a lógica de acesso ao banco de dados Oracle para
    /// obter as informações sobre as linhas de crédito disponíveis.
    /// </remarks>
    public class ProdutoService
    {
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
            var produtoEncontrado = await _context.Produto
                .Where(p =>
                    simulacaoRequest.ValorDesejado >= p.VrMinimo &&
                    (p.VrMaximo == null || simulacaoRequest.ValorDesejado <= p.VrMaximo) &&
                    simulacaoRequest.Prazo >= p.NuMinimoMeses &&
                    (p.NuMaximoMeses == null || simulacaoRequest.Prazo <= p.NuMaximoMeses)
                )
                .Select(p => new ProdutoDTO
                {
                    CoProduto = p.Id,
                    NoProduto = p.Nome,
                    PcTaxaJuros = p.PcTaxaJuros,
                    NuMinimoMeses = (short)p.NuMinimoMeses,
                    NuMaximoMeses = (short?)p.NuMaximoMeses,
                    VrMinimo = p.VrMinimo,
                    VrMaximo = p.VrMaximo
                })
                .FirstOrDefaultAsync();
            if (produtoEncontrado == null)
            {
                throw new KeyNotFoundException("Nenhum produto foi encontrado para os parâmetros informados.");
            }

            return produtoEncontrado;
        }

        public async Task<List<ProdutoDTO>> GetProdutosPorIdsAsync(List<int> ids)
        {
            return await _context.Produto
                .AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .Select(p => new ProdutoDTO
                {
                    CoProduto = p.Id,
                    NoProduto = p.Nome,
                    PcTaxaJuros = p.PcTaxaJuros
                })
                .ToListAsync();
        }
    }
}
