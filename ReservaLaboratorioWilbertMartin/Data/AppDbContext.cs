using Microsoft.EntityFrameworkCore;
using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        // DbSets existentes
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        // Nuevos DbSets
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<MonitorLab> MonitorLab { get; set; }
        public DbSet<Laboratorio> Laboratorios { get; set; }
        public DbSet<ReservaLaboratorio> ReservasLaboratorio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>().HasMany(r => r.Users).WithOne(u => u.Role).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "Admin" }, new Role { Id = 2, Name = "Usuario" });

            //un docente tiene un user y un user tiene un docente, la columna que los une es la UserId en la tabla docente
            modelBuilder.Entity<Docente>().HasOne(d => d.User).WithOne().HasForeignKey<Docente>(d => d.UserId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<MonitorLab>().HasOne(m => m.User).WithOne().HasForeignKey<MonitorLab>(m => m.UserId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ReservaLaboratorio>().HasOne(r => r.Docente).WithMany(d => d.Reservas).HasForeignKey(r => r.DocenteId).OnDelete(DeleteBehavior.Cascade);
        
            modelBuilder.Entity<ReservaLaboratorio>().HasOne(r => r.MonitorLab).WithMany(m => m.ReservasAprobadas).HasForeignKey(r => r.MonitorId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ReservaLaboratorio>() .HasOne(r => r.Laboratorio) .WithMany(l => l.Reservas).HasForeignKey(r => r.LaboratorioId).OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }


}
