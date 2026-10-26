using Microsoft.EntityFrameworkCore;
using SistemaRestauranteApi.Models;

namespace SistemaRestauranteApi.Data;

public class AppDbContext : DbContext
{
     public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}
    
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Mesa> Mesas => Set<Mesa>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallesPedidos => Set<DetallePedido>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Producto>()
            .Property(p => p.Precio)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Pedido>()
            .Property(p => p.Total)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DetallePedido>()
            .Property(d => d.PrecioUnitario)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DetallePedido>()
            .Property(d => d.Subtotal)
            .HasPrecision(10, 2);
    }

}