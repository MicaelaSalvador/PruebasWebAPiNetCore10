using Microsoft.EntityFrameworkCore;
using SistemaVentasAPI.Data;
using Microsoft.Extensions.Options;
using SistemaVentasAPI.Services;
using SistemaVentasAPI.Services.Interfaces;
using SistemaVentasAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // ← Swagger
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPedidoService, PedidoService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();  // ← genera /swagger/v1/swagger.json
    app.UseSwaggerUI();  // ← genera /swagger (UI)
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () =>
{
    return "Sistema de Ventas API funcionando";
});

app.Run();

