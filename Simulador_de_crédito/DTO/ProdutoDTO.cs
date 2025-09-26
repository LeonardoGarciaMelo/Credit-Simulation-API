namespace Simulador_de_crédito.DTO
{
    public record ProdutoDTO
    {
        public String NoProduto {  get; init; }
        public decimal PcTaxaJuros { get; init; }
        public short NuMinimoMeses { get; init; }
        public short? NuMaximoMeses { get; init; }
        public decimal VrMinimo {  get; init; }
        public decimal? VrMaximo { get; init; }
    }
}
