using CuidaTuAguaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CuidaTuAguaBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Hogar> Hogares => Set<Hogar>();

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
    }
}
