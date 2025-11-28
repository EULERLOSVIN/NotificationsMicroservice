using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationsMicroservice.Consumers;
using NotificationsMicroservice.Events;

namespace NotificationsMicroservice.Test.Consumers;

public class TripCreatedConsumerTests 
{
    [Fact]
    public async Task Should_Consume_TripCreatedEvent()
    {
        //Act : Configurar el entorno de prueba con MassTransit y un consumidor de prueba
        var provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<TripCreatedConsumer>();  // Registrar el consumidor que queremos probar
            })
            .AddSingleton(Mock.Of<ILogger<TripCreatedConsumer>>()) // Crear un logger simulado (mock) para el consumidor
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>(); // Obtener el test harness para simular el bus de mensajes

        await harness.Start();// Iniciar el bus de prueba

        try
        {
            var tripId = 123;
            var passengerId = 456;

            // Act : Publicar un evento de prueba
            await harness.Bus.Publish(new TripCreatedEvent
            {
                TripId = tripId,
                PassengerId = passengerId,
                OriginAddress = "Calle Falsa 123",
                DestinationAddress = "Avenida Siempre Viva 742",
                EstimatedFare = 15.50m,
                CreatedAt = DateTime.UtcNow
            });

            // Assert: Verificar que el mensaje fue consumido correctamente

            // Comprobar que el evento fue recibido por algún consumidor
            (await harness.Consumed.Any<TripCreatedEvent>()).Should().BeTrue();

            // Comprobar que nuestro consumidor específico procesó el evento
            (await harness.GetConsumerHarness<TripCreatedConsumer>().Consumed.Any<TripCreatedEvent>()).Should().BeTrue();
        }
        finally
        {
            await harness.Stop();// Detener el bus de prueba al final
        }
    }
}
