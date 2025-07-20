using System.Text.Json;
using Confluent.Kafka;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Domain.Brokers;

/// <summary>
/// Abstraction for publishing domain events to Kafka. Having a small interface
/// here keeps the rest of the domain decoupled from the Kafka client library.
/// </summary>
public interface IKafkaBroker
{
    /// <summary>
    /// Sends the specified message to the given Kafka topic.
    /// </summary>
    Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default);
}

/// <summary>
/// Default implementation of <see cref="IKafkaBroker"/> that serializes the
/// message payload as JSON and publishes it using the Confluent client.
/// </summary>
public sealed class KafkaBroker : IKafkaBroker, IDisposable
{
    private readonly IProducer<Null, string> _producer;

    /// <summary>
    /// Creates a new broker using the given bootstrap servers. We keep the
    /// connection setup here so consumers of the interface remain simple.
    /// </summary>
    public KafkaBroker(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    /// <summary>
    /// Serializes the provided message as JSON and produces it to the topic.
    /// </summary>
    public async Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = payload }, cancellationToken);
    }

    /// <summary>
    /// Disposes the underlying Kafka producer.
    /// </summary>
    public void Dispose() => _producer.Dispose();
}
