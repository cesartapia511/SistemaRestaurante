
using Microsoft.EntityFrameworkCore;
using SistemaRestauranteApi.Data;
using SistemaRestauranteApi.Interfaces;
using SistemaRestauranteApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var conexionLocal = builder.Configuration.GetConnectionString("RestauranteConection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(conexionLocal)
);

builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IMesaService, MesaService>();

builder.Services.AddScoped<PedidoService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
