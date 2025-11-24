using Microsoft.EntityFrameworkCore;
using Sistema_Web_de_Nominas.Models;

namespace Sistema_Web_de_Nominas.Data
{
    public class NominaDbContext(DbContextOptions<NominaDbContext> options) : DbContext(options) //agregar sobre Empleado y nomina
    {
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Rol> Rol { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Admin" },
                new Rol { Id = 2, Nombre = "Usuario" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }

}