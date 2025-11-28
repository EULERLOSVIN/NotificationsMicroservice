using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationsMicroservice.Consumers;
using NotificationsMicroservice.Events;
using Xunit;

namespace NotificationsMicroservice.Test.Consumers;

public class TripCreatedConsumerTests
{
    [Fact]
    public async Task Should_Consume_TripCreatedEvent()
    {
        // Arrange
        var provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<TripCreatedConsumer>();
            })
            .AddSingleton(Mock.Of<ILogger<TripCreatedConsumer>>()) // Mock logger
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();

        await harness.Start();

        try
        {
            var tripId = 123;
            var passengerId = 456;

            // Act
            await harness.Bus.Publish(new TripCreatedEvent
            {
                TripId = tripId,
                PassengerId = passengerId,
                OriginAddress = "Calle Falsa 123",
                DestinationAddress = "Avenida Siempre Viva 742",
                EstimatedFare = 15.50m,
                CreatedAt = DateTime.UtcNow
            });

            // Assert
            
            // Verify that the message was consumed
            (await harness.Consumed.Any<TripCreatedEvent>()).Should().BeTrue();

            // Verify that the specific consumer consumed the message
            (await harness.GetConsumerHarness<TripCreatedConsumer>().Consumed.Any<TripCreatedEvent>()).Should().BeTrue();
        }
        finally
        {
            await harness.Stop();
        }
    }
}
