using Microsoft.EntityFrameworkCore;
using Monitor_App.Web.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Monitor_App.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LocationData> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración del modelo LocationData
            modelBuilder.Entity<LocationData>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Latitude)
                    .IsRequired()
                    .HasColumnType("decimal(9,6)");

                entity.Property(e => e.Longitude)
                    .IsRequired()
                    .HasColumnType("decimal(9,6)");

                entity.Property(e => e.Altitude)
                    .HasColumnType("decimal(9,6)");

                entity.Property(e => e.Accuracy)
                    .HasColumnType("decimal(9,6)");

                entity.Property(e => e.Speed)
                    .HasColumnType("decimal(9,6)");

                entity.Property(e => e.Timestamp)
                    .IsRequired();

                // Índice para mejorar consultas por dispositivo y fecha
                entity.HasIndex(e => new { e.DeviceId, e.Timestamp })
                    .IsDescending(false, true);
            });
        }
    }
}
