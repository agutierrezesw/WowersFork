using MediatR;
using ConsumerBuilder =
    Confluent.Kafka.ConsumerBuilder<Confluent.Kafka.Null,
        RestaurantReservation.Core.Events.Contracts.IIntegrationEvent>;

namespace ReservationManagement.Consumer;

/// <summary>
/// Event consumer
/// </summary>
public class Worker(
    ILogger<Worker> logger, 
    ConsumerBuilder builder,
    IMediator mediator,
    string topic) : BackgroundService
{
 
    /// <summary>
    /// Read events from kafka cluster and dispatch it as Mediator notifications
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = builder.Build();
        consumer.Subscribe(topic);
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Waiting for event messages at topic {topic}", topic);

            var consumeResult = consumer.Consume(stoppingToken);
            if (consumeResult is null) continue;

            logger.LogInformation(
                "Event received with timestamp {timestamp}",
                consumeResult.Message.Timestamp
            );

            await mediator.Publish(consumeResult.Message.Value, stoppingToken);
        }

        consumer.Close();
    }
}