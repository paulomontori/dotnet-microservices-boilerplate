using Confluent.Kafka;

namespace PWorx.MicroserviceBoilerPlate.Domain;

public interface IKafkaProducer
{
    Task PublishAsync(string message);
}

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaProducer(string bootstrapServers, string topic)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _topic = topic;
    }

    public async Task PublishAsync(string message)
    {
        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
    }
}
