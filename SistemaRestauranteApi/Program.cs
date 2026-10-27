
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

builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<MesaService>();

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
