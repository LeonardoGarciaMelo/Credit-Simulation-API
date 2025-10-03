using System.ComponentModel.DataAnnotations;

namespace Simulador_de_Credito.Model
{
    /// <summary>
    /// Representa a entidade de um produto de crédito, mapeada a partir do banco de dados.
    /// </summary>
    /// <remarks>
    /// Esta classe define a estrutura de um produto de crédito, incluindo suas regras de negócio
    /// como limites de valor, prazo e a taxa de juros associada.
    /// </remarks>
    public class Product
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
