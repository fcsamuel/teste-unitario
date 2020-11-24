using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiProduct.Models
{
    public partial class produtodbContext : DbContext
    {
        public produtodbContext()
        {
        }

        public produtodbContext(DbContextOptions<produtodbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Marca> Marca { get; set; }
        public virtual DbSet<Produto> Produto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Server=localhost;Database=produtodb;Port=5432;User Id=postgres;Password=admin;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.CdMarca);

                entity.ToTable("marca");

                entity.Property(e => e.CdMarca)
                    .HasColumnName("cd_marca")
                    .ValueGeneratedNever();

                entity.Property(e => e.DsMarca)
                    .HasColumnName("ds_marca")
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.CdProduto);

                entity.ToTable("produto");

                entity.Property(e => e.CdProduto)
                    .HasColumnName("cd_produto")
                    .ValueGeneratedNever();

                entity.Property(e => e.CdMarca).HasColumnName("cd_marca");

                entity.Property(e => e.DsObs)
                    .IsRequired()
                    .HasColumnName("ds_obs")
                    .HasMaxLength(4000);

                entity.Property(e => e.DsProduto)
                    .HasColumnName("ds_produto")
                    .HasMaxLength(120);

                entity.Property(e => e.NrValor)
                    .HasColumnName("nr_valor")
                    .HasColumnType("numeric(15,3)");

                entity.HasOne(d => d.CdMarcaNavigation)
                    .WithMany(p => p.Produto)
                    .HasForeignKey(d => d.CdMarca)
                    .HasConstraintName("fk_produto_marca");
            });
        }
    }
}
