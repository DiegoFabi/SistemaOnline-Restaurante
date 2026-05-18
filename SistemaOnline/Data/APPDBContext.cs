using Microsoft.EntityFrameworkCore;
using SistemaOnline.Models;

namespace SistemaOnline.Data
{
    public class APPDBContext : DbContext
    {
        public APPDBContext(DbContextOptions<APPDBContext> options) : base(options)
        {

        }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.ID_Rol);

            modelBuilder.Entity<Rol>().HasData(
                new Rol { ID_Rol = 1, Nombre_Rol = "Adminstrador" },
                new Rol { ID_Rol = 2, Nombre_Rol = "Mesero" },
                new Rol { ID_Rol = 3, Nombre_Rol = "Cajero" }
                );

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { ID_Usuario = 1, Nombre_Usuario = "Tigre Capo", Email = "TrigeCapo123@gmail.com", Password = "aQwr@2", ID_Rol = 1 }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
