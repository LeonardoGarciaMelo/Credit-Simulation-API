using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulador_de_crédito.Model
{
    [Table("Simulacoes")]
    public class Simulacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("DATE")]
        [Required]
        public DateTimeOffset Data {  get; set; } = DateTimeOffset.Now;

        [Column("valor_desejado")]
        public decimal ValorDesejado { get; set; }

        [Column("prazo")]
        public short Prazo { get; set; }

        [Column("valorTotalParcelas")]
        public decimal ValorTotalParcelas { get; set; }

        [Column("co_produto")]
        public int CoProduto { get; set; }
    }
}
