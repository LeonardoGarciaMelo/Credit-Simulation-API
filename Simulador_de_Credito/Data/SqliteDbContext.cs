using Microsoft.EntityFrameworkCore;
using Simulador_de_Credito.Model;

namespace Simulador_de_Credito.Data
{
    /// <summary>
    /// Representa o contexto do banco de dados para a conexão com o banco de dados local SQLite.
    /// </summary>
    /// <remarks>
    /// Esta classe é responsável por gerenciar a sessão com o banco de dados SQLite,
    /// que é utilizado para persistir o histórico de todas as simulações de crédito realizadas.
    /// </remarks>
    public class SqliteDbContext : DbContext
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="SqliteDbContext"/>.
        /// </summary>
        /// <param name="options">As opções para este contexto, configuradas em Program.cs e injetadas pela aplicação.</param>
        public SqliteDbContext(DbContextOptions options) : base(options){ }

        /// <summary>
        /// Obtém ou define o <see cref="DbSet{TEntity}"/> para as entidades de <see cref="Simulacao"/>.
        /// </summary>
        /// <remarks>
        /// Esta propriedade representa a coleção de todas as entidades 'Simulacao' no contexto,
        /// que é mapeada para a tabela 'Simulacoes' no banco de dados SQLite.
        /// </remarks>
        public DbSet<Simulacao> Simulacoes { get; set; }

        /// <summary>
        /// Configura o modelo de dados e o mapeamento das entidades para o banco de dados SQLite.
        /// </summary>
        /// <remarks>
        /// Este método é sobreposto para fornecer configurações explícitas para a entidade <see cref="Simulacao"/>,
        /// definindo o nome da tabela, chave primária, tipos de coluna e se os campos são obrigatórios.
        /// </remarks>
        /// <param name="modelBuilder">O construtor que está sendo usado para construir o modelo para este contexto.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeia a entidade 'Simulacao' para a tabela chamada 'Simulacoes'.
            modelBuilder.Entity<Simulacao>().ToTable("Simulacoes");
            modelBuilder.Entity<Simulacao>()
            .HasKey(s => s.Id);
            modelBuilder.Entity<Simulacao>()
                .Property(s => s.Id)
                .HasColumnType("TEXT")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Simulacao>()
                .Property(s => s.Data)
                .IsRequired();
            modelBuilder.Entity<Simulacao>()
                .Property(s => s.ValorDesejado)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Simulacao>()
                .Property(s => s.Prazo)
                .IsRequired();
            modelBuilder.Entity<Simulacao>()
                .Property(s => s.ValorTotalParcelas)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Simulacao>()
                .Property(s => s.CoProduto)
                .IsRequired();
        }
    }
}
