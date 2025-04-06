using Microsoft.EntityFrameworkCore;
using Yeshuapp.Entities;

namespace Yeshuapp.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProdutosEntity> Produtos { get; set; }
        public DbSet<ClientesEntity> Clientes { get; set; }
        public DbSet<PedidosEntity> Pedidos { get; set; }
        public DbSet<PedidoProdutoEntity> PedidoProdutos { get; set; }
        public DbSet<EventosEntity> Eventos { get; set; }
        public DbSet<FrasesEntity> Frases { get; set; }
        public DbSet<FluxoCaixaEntity> FluxoCaixa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidoProdutoEntity>()
                .HasKey(pp => new { pp.PedidoId, pp.ProdutoId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
