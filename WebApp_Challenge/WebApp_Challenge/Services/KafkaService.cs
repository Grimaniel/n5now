using Confluent.Kafka;
using Newtonsoft.Json;
namespace WebApp_Challenge.Services
{
    public class KafkaService
    {
        private readonly string _bootstrapServers;

        public KafkaService(string bootstrapServers)
        {
            _bootstrapServers = bootstrapServers;
        }

        public async Task PublishPermissionOperation(string operation)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var message = new
            {
                Id = Guid.NewGuid(),
                Operation = operation,
                Timestamp = DateTime.UtcNow
            };

            await producer.ProduceAsync("permission-operations", new Message<Null, string>
            {
                Value = JsonConvert.SerializeObject(message)
            });
        }
    }
}
