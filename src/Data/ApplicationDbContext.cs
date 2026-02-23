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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // CAMBIO CLAVE: Le decimos a Entity Framework que use el esquema correcto
            modelBuilder.HasDefaultSchema("UrnaElectronica");
            
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}