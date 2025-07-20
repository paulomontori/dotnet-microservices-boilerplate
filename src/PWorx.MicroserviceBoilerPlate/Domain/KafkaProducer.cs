using Confluent.Kafka;

namespace PWorx.MicroserviceBoilerPlate.Domain;

/// <summary>
/// Abstraction over a Kafka producer. The domain only depends on this
/// interface so that message publishing can be replaced or mocked without
/// leaking the Confluent.Kafka implementation into the core logic.
/// </summary>
public interface IKafkaProducer
{
    /// <summary>
    /// Publishes a raw string message to the configured topic.
    /// </summary>
    Task PublishAsync(string message);
}

/// <summary>
/// Lightweight implementation of <see cref="IKafkaProducer"/> using the
/// Confluent Kafka client. Creating this wrapper keeps the domain service
/// free from third‑party dependencies.
/// </summary>
public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    /// <summary>
    /// Initializes a new Kafka producer for the given bootstrap servers and topic.
    /// </summary>
    public KafkaProducer(string bootstrapServers, string topic)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _topic = topic;
    }

    /// <summary>
    /// Sends the message to Kafka. The method is asynchronous to avoid blocking
    /// the calling thread and to integrate nicely with async workflows.
    /// </summary>
    public async Task PublishAsync(string message)
    {
        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
    }
}
