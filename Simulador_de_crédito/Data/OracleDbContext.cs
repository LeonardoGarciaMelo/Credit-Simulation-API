using Microsoft.EntityFrameworkCore;
using Simulador_de_crédito.Model;

namespace Simulador_de_crédito.Data
{
    public class OracleDbContext : DbContext
    {
        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options) { }

        public DbSet<Produto> Produto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().ToTable("PRODUTO");
            modelBuilder.Entity<Produto>()
            .HasKey(p => p.Id);
            modelBuilder.Entity<Produto>()
                .Property(p => p.Id).HasColumnName("CO_PRODUTO");
            modelBuilder.Entity<Produto>()
                .Property(p => p.Nome).HasColumnName("NO_PRODUTO");
            modelBuilder.Entity<Produto>()
                .Property(p => p.PcTaxaJuros).HasColumnName("PC_TAXA_JUROS")
                .HasPrecision(10, 9);
            modelBuilder.Entity<Produto>()
                .Property(p => p.NuMinimoMeses).HasColumnName("NU_MINIMO_MESES");
            modelBuilder.Entity<Produto>()
                .Property(p => p.NuMaximoMeses).HasColumnName("NU_MAXIMO_MESES");
            modelBuilder.Entity<Produto>()
                .Property(p => p.VrMinimo).HasColumnName("VR_MINIMO")
                .HasPrecision(18, 2);
            modelBuilder.Entity<Produto>()
                .Property(p => p.VrMaximo).HasColumnName("VR_MAXIMO")
                .HasPrecision(18, 2);
        }
    }
}
