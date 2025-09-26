namespace Simulador_de_crédito.DTO
{
    public class Parcela
    {
        public int Numero {  get; set; }
        public decimal ValorAmortizacao { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorPrestacao { get; set; }

        public Parcela(int Numero, decimal ValorAmortizacao, decimal ValorJuros, decimal ValorPrestacao) 
        {
            this.Numero = Numero;
            this.ValorAmortizacao = ValorAmortizacao;
            this.ValorJuros = ValorJuros;
            this.ValorPrestacao = ValorPrestacao;
        }

    }
}
