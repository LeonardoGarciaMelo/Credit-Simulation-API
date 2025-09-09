using System.ComponentModel.DataAnnotations;

namespace Simulador_de_crédito.Model
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        [Required]
        public decimal PcTaxaJuros { get; set; }
        [Required]
        public short NuMinimoMeses { get; set; }
        public short? NuMaximoMeses { get; set; }
        [Required]
        public decimal VrMinimo { get; set; }
        public decimal? VrMaximo { get; set; }
    }
}
