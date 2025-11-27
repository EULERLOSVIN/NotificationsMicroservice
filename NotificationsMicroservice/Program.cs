using Microsoft.EntityFrameworkCore;
using NotificationsMicroservice.Data;
using NotificationsMicroservice.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// AGREGAR ESTO:
builder.Services.AddDbContext<NotificationsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// =================================================================
// 3. AQUÍ ESTÁ EL ARREGLO (Agrega estas dos líneas)
// =================================================================

// A. Registrar como Singleton para poder inyectarlo en el CommandHandler
builder.Services.AddSingleton<NotificationListener>();

// B. Registrar como HostedService para que corra en segundo plano (Background)
// Usamos el provider para decirle "Usa la misma instancia Singleton de arriba"
builder.Services.AddHostedService(provider => provider.GetRequiredService<NotificationListener>());

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // ESTA LÍNEA ES MÁGICA: Corta los bucles infinitos
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
