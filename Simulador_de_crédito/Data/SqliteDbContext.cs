using Microsoft.EntityFrameworkCore;
using Simulador_de_crédito.Model;

namespace Simulador_de_crédito.Data
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions options) : base(options){ }
        
        public DbSet<Simulacao> Simulacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
