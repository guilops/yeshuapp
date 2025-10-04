using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yeshuapp.Entities;

namespace Yeshuapp.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProdutosEntity> Produtos { get; set; }
        public DbSet<ClientesEntity> Clientes { get; set; }
        public DbSet<PedidosEntity> Pedidos { get; set; }
        public DbSet<PedidoProdutoEntity> PedidoProdutos { get; set; }
        public DbSet<EventosEntity> Eventos { get; set; }
        public DbSet<FrasesEntity> Frases { get; set; }
        public DbSet<FluxoCaixaEntity> FluxoCaixa { get; set; }
        public DbSet<DespesasEntity> Despesas { get; set; }
        public DbSet<PedidoOracaoEntity> PedidoOracao { get; set; }
        public DbSet<VisitanteEntity> Visitante { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.Property(r => r.NormalizedName).HasMaxLength(128);
            });

            modelBuilder.Entity<IdentityUser>(entity =>
            {
                entity.Property(u => u.NormalizedUserName).HasMaxLength(128);
                entity.Property(u => u.NormalizedEmail).HasMaxLength(128);
            });


            modelBuilder.Entity<PedidoOracaoEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Message).HasMaxLength(2000).IsRequired();
                e.Property(x => x.CreatedAt).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<VisitanteEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(120).IsRequired();
                e.Property(x => x.Email).HasMaxLength(200);
                e.Property(x => x.Phone).HasMaxLength(40);
                e.Property(x => x.Notes).HasMaxLength(1000);
            });

            modelBuilder.Entity<PedidoProdutoEntity>()
                .HasKey(pp => new { pp.PedidoId, pp.ProdutoId });

            modelBuilder.Entity<FluxoCaixaEntity>()
                .Property(e => e.Data)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<DespesasEntity>()
                .Property(e => e.Data)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<PedidoOracaoEntity>()
                .Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<PedidosEntity>()
                .Property(e => e.Data)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<VisitanteEntity>()
                .Property(e => e.VisitDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

        }
    }
}
