using FINALGARETTOJUAN.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace FINALGARETTOJUAN.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación de Producto con Tipo
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Tipo)
                .WithMany(t => t.Productos)
                .HasForeignKey(p => p.IdTipo)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación de Producto con Modelo
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Modelo)
                .WithMany(m => m.Productos)
                .HasForeignKey(p => p.IdModelo)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación de Stock con Producto
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.producto) // Cada Stock tiene un Producto asociado
                .WithMany()  // Un Producto puede tener múltiples registros de Stock
                .HasForeignKey(s => s.IdProducto) // Clave foránea que referencia al Producto
                .OnDelete(DeleteBehavior.Restrict); // Aseguramos que no se elimine el Producto cuando se elimine el Stock
                                                    // Relación de Stock con Producto
                                                    // Relación de Stock con Producto: Uno a uno
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.stock) // Un Producto tiene un único Stock
                .WithOne(s => s.producto) // Un Stock tiene un solo Producto asociado
                .HasForeignKey<Stock>(s => s.IdProducto) // Clave foránea en Stock
                .OnDelete(DeleteBehavior.Restrict); // No eliminar Producto cuando se elimine el Stock



        }
    }
}


