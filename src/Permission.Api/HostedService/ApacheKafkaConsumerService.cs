using Confluent.Kafka;
using Permissions.Api.Dtos;
using System.Diagnostics;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Permissions.Api.HostedService
{
    public class ApacheKafkaConsumerService : BackgroundService
    {
        private string topic;

        private readonly ILogger<ApacheKafkaConsumerService> _logger;
        private readonly IConsumer<Ignore, string> _kafkaConsumer;
        public ApacheKafkaConsumerService(ILogger<ApacheKafkaConsumerService> logger, IConfiguration config)
        {
            _logger = logger;
            this.topic = config.GetValue<string>("Kafka:RequestTopic");
            var consumerConfig = new ConsumerConfig();
            config.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
            consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            _kafkaConsumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }
        private void StartConsumerLoop(CancellationToken cancellationToken)
        {            
            try
            {
                _kafkaConsumer.Subscribe(topic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumer = _kafkaConsumer.Consume(cancellationToken);
                        var orderRequest = JsonSerializer.Deserialize<RequestProcessingRequest>(consumer.Message.Value);
                        _logger.LogInformation($"Kafka Processing Request Id: {orderRequest.Id} ");
                    }
                }
                catch (OperationCanceledException)
                {
                    _kafkaConsumer.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consumer.");
            }
        }
        public override void Dispose()
        {
            this._kafkaConsumer.Close();
            this._kafkaConsumer.Dispose();

            base.Dispose();
        }
    }
}
