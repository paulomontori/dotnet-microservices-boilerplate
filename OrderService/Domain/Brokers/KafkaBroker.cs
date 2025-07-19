using System.Text.Json;
using Confluent.Kafka;

namespace dotnet_microservices_boilerplate.OrderService.Domain.Brokers;

/// <summary>
/// Simple Kafka producer used by the domain to publish events.
/// </summary>
public interface IKafkaBroker
{
    /// <summary>
    /// Sends the specified message to the given Kafka topic.
    /// </summary>
    Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default);
}

public sealed class KafkaBroker : IKafkaBroker, IDisposable
{
    private readonly IProducer<Null, string> _producer;

    public KafkaBroker(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = payload }, cancellationToken);
    }

    public void Dispose() => _producer.Dispose();
}
