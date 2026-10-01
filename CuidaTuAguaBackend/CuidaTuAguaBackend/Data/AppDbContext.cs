using CuidaTuAguaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CuidaTuAguaBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Hogar> Hogares => Set<Hogar>();

    public DbSet<ConsumoAgua> Consumos => Set<ConsumoAgua>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Hogar>(entity =>
        {
            entity.ToTable("hogares");

            entity.HasKey(h => h.Id);

            entity.Property(h => h.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(h => h.Direccion)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(h => h.Telefono)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(h => h.Pais)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(h => h.Estrato)
                .IsRequired()
                .HasMaxLength(5);

            entity.Property(h => h.Habitantes)
                .IsRequired();

            // Coinciden con las validaciones de "duplicateAddress" / "duplicatePhone"
            // que ya existen en el frontend (register.json).
            entity.HasIndex(h => h.Direccion).IsUnique();
            entity.HasIndex(h => h.Telefono).IsUnique();
        });

        modelBuilder.Entity<ConsumoAgua>(entity =>
        {
            entity.ToTable("consumos");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.ValorM3)
                .IsRequired()
                .HasColumnType("decimal(12,4)");

            entity.Property(c => c.FechaHora)
                .IsRequired();

            // Un hogar tiene muchas lecturas de consumo (HU-016).
            entity.HasOne(c => c.Hogar)
                .WithMany()
                .HasForeignKey(c => c.HogarId)
                .OnDelete(DeleteBehavior.Cascade);

            // Acelera las consultas por hogar + rango de fechas, que es
            // exactamente lo que hace el endpoint GET /api/consumo.
            entity.HasIndex(c => new { c.HogarId, c.FechaHora });
        });
    }
}
