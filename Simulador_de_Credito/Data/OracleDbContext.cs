using Microsoft.EntityFrameworkCore;
using Simulador_de_Credito.Model;

namespace Simulador_de_Credito.Data
{
    /// <summary>
    /// Representa o contexto do banco de dados para a conexão com o Oracle.
    /// </summary>
    /// <remarks>
    /// Esta classe é a ponte (bridge) entre as entidades do C# e o banco de dados Oracle,
    /// utilizando o Entity Framework Core. Ela é responsável por gerenciar a sessão com o banco
    /// e permitir a consulta e manipulação de dados.
    /// </remarks>
    public class OracleDbContext : DbContext
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="OracleDbContext"/>.
        /// </summary>
        /// <param name="options">As opções para este contexto, tipicamente configuradas em Program.cs e passadas via injeção de dependência.</param>
        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options) { }

        /// <summary>
        /// Obtém ou define o <see cref="DbSet{TEntity}"/> para as entidades de <see cref="Produto"/>.
        /// </summary>
        /// <remarks>
        /// Esta propriedade representa a coleção de todas as entidades 'Produto' no contexto,
        /// que é mapeada para a tabela 'PRODUTO' no banco de dados Oracle.
        /// </remarks>
        public DbSet<Product> Produto { get; set; }

        /// <summary>
        /// Configura o modelo de dados que é descoberto por convenção a partir dos tipos de entidade.
        /// </summary>
        /// <remarks>
        /// Este método é sobreposto para fornecer uma configuração explícita e detalhada do mapeamento
        /// entre a entidade C# <see cref="Produto"/> e a tabela 'PRODUTO' do banco de dados Oracle,
        /// incluindo nomes de colunas, chaves primárias e precisão dos dados.
        /// </remarks>
        /// <param name="modelBuilder">O construtor que está sendo usado para construir o modelo para este contexto.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeia a entidade 'Produto' para a tabela chamada 'PRODUTO' no Oracle.
            modelBuilder.Entity<Product>().ToTable("PRODUTO");
            modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);
            modelBuilder.Entity<Product>()
                .Property(p => p.Id).HasColumnName("CO_PRODUTO");
            modelBuilder.Entity<Product>()
                .Property(p => p.Nome).HasColumnName("NO_PRODUTO");
            modelBuilder.Entity<Product>()
                .Property(p => p.PcTaxaJuros).HasColumnName("PC_TAXA_JUROS")
                .HasPrecision(10, 9);
            modelBuilder.Entity<Product>()
                .Property(p => p.NuMinimoMeses).HasColumnName("NU_MINIMO_MESES");
            modelBuilder.Entity<Product>()
                .Property(p => p.NuMaximoMeses).HasColumnName("NU_MAXIMO_MESES");
            modelBuilder.Entity<Product>()
                .Property(p => p.VrMinimo).HasColumnName("VR_MINIMO")
                .HasPrecision(18, 2);
            modelBuilder.Entity<Product>()
                .Property(p => p.VrMaximo).HasColumnName("VR_MAXIMO")
                .HasPrecision(18, 2);
        }
    }
}
