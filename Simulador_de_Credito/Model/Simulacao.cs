using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulador_de_Credito.Model
{
    /// <summary>
    /// Representa a entidade de uma simulação de crédito que foi salva no banco de dados.
    /// </summary>
    /// <remarks>
    /// Esta classe define a estrutura dos dados que são persistidos no banco de dados local SQLite
    /// a cada simulação bem-sucedida.
    /// </remarks>
    [Table("Simulacoes")]
    public class Simulacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("DATE")]
        [Required]
        public DateTime Data {  get; set; } = DateTime.UtcNow;

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
