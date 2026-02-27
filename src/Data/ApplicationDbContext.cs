using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Models;

namespace UrnaElectronica.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Partido> Partidos { get; set; }

        public DbSet<Impresora> Impresoras { get; set; }

        public DbSet<Urna> Urnas { get; set; }

        public DbSet<Acceso> Accesos { get; set; }

        public DbSet<ProcesoElectoral> Procesos { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Mantén el esquema si así está en SQL
            modelBuilder.HasDefaultSchema("UrnaElectronica");
            
            // MAPEO EXPLÍCITO: Asegúrate de que estos nombres sean idénticos a los de SQL
            modelBuilder.Entity<Urna>().ToTable("Urnas");
            modelBuilder.Entity<Partido>().ToTable("Partidos"); 
            modelBuilder.Entity<Impresora>().ToTable("Impresoras");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Acceso>().ToTable("Accesos");
            modelBuilder.Entity<ProcesoElectoral>().ToTable("Procesos");

            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Impresora>().HasIndex(i => i.Mac).IsUnique();
            modelBuilder.Entity<Urna>().HasIndex(u => u.Nombre).IsUnique();
            modelBuilder.Entity<Acceso>().HasIndex(a => a.Codigo).IsUnique();
        }
    }
}