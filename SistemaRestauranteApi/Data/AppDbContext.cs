using Microsoft.EntityFrameworkCore;


namespace SistemaRestauranteApi.Data;

public class AppDbContext : DbContext
{
     public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}
    
    

}