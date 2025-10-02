namespace Simulador_de_Credito.DTO
{
    public record ProdutoDTO
    {
        public int CoProduto { get; init; }
        public String NoProduto {  get; init; }
        public decimal PcTaxaJuros { get; init; }
        public short NuMinimoMeses { get; init; }
        public short? NuMaximoMeses { get; init; }
        public decimal VrMinimo {  get; init; }
        public decimal? VrMaximo { get; init; }

      /*  public ProdutoDTO(String noProduto, decimal pcTaxaJuros, short nuMinimoMeses, short? nuMaximoMeses, decimal vrMinimo, decimal? vrMaximo)
        {
            NoProduto = noProduto;
            PcTaxaJuros = pcTaxaJuros;
            NuMinimoMeses = nuMinimoMeses;
            NuMaximoMeses = nuMaximoMeses;
            VrMinimo = vrMinimo;
            VrMaximo = vrMaximo;
        }*/
    }
}
