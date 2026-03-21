using Microsoft.EntityFrameworkCore;
using Dominio;

namespace Repositorio
{
    public class Context : DbContext
    {
        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Vendas> Vendas { get; set; }
        public DbSet<ProdutoVendas> ProdutoVendas { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=TQR216777\SQLEXPRESS;Database=DreamLand;User Id=tds;Password=tds123;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProdutoVendas>(entity =>
            {
                entity.HasKey(vp => vp.IdProdutoVenda);

                entity.HasOne(vp => vp.Produto)
                    .WithMany(p => p.ProdutoVendas)
                    .HasForeignKey(vp => vp.ProdutoId)
                    .IsRequired();

                entity.HasOne(vp => vp.Venda)
                    .WithMany(v => v.ProdutoVendas)
                    .HasForeignKey(vp => vp.VendaId)
                    .IsRequired();
            });

        }
    }
}
