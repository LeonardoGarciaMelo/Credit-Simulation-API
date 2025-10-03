namespace Simulador_de_Credito.DTO
{
    /// <summary>
    /// Representa os dados de um produto de crédito disponível para simulação.
    /// </summary>
    /// <remarks>
    /// Este DTO carrega as informações e regras de negócio de uma linha de crédito específica,
    /// localizada na tabela de produtos
    /// </remarks>
    public record ProdutoDTO
    {
        /// <summary>
        /// O código de identificação único do produto de crédito.
        /// </summary>
        public int CoProduto { get; init; }

        /// <summary>
        /// O nome comercial do produto de crédito.
        /// </summary>
        public String NoProduto {  get; init; }

        /// <summary>
        /// A taxa de juros mensal aplicável para este produto, em formato decimal.
        /// </summary>
        /// <example>Uma taxa de 1.79% ao mês seria representada como 0.0179.</example>
        public decimal PcTaxaJuros { get; init; }

        /// <summary>
        /// O número mínimo de meses permitido para este produto.
        /// </summary>
        public short NuMinimoMeses { get; init; }

        /// <summary>
        /// O número máximo de meses permitido para este produto.
        /// Um valor nulo indica que não há um limite máximo de prazo.
        /// </summary>
        public short? NuMaximoMeses { get; init; }

        /// <summary>
        /// O valor mínimo de empréstimo permitido para este produto.
        /// </summary>
        public decimal VrMinimo {  get; init; }

        /// <summary>
        /// O valor máximo de empréstimo permitido para este produto.
        /// Um valor nulo indica que não há um limite máximo de valor.
        /// </summary>
        public decimal? VrMaximo { get; init; }
    }
}
