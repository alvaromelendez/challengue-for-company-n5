using Confluent.Kafka;
using Permissions.Api.Dtos;
using Permissions.Api.HostedService;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Permissions.Api.Application.IntegrationEvents
{
    public class RequestIntegrationEventService : IRequestIntegrationEventService, IDisposable
    {
        private string topic;
        private readonly ILogger<RequestIntegrationEventService> _logger;
        private readonly IProducer<Null, string> kafkaProducer;
        public RequestIntegrationEventService(ILogger<RequestIntegrationEventService> logger, IConfiguration config)
        {
            _logger = logger;
            this.topic = config.GetValue<string>("Kafka:RequestTopic");
            var producerConfig = new ProducerConfig();
            config.GetSection("Kafka:ProducerSettings").Bind(producerConfig);
            producerConfig.ClientId = Dns.GetHostName();

            this.kafkaProducer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task<bool> SendRequest(RequestProcessingRequest message)
        {
            try
            {
                var result = await kafkaProducer.ProduceAsync(topic, new Message<Null, string> { Value = JsonSerializer.Serialize<RequestProcessingRequest>(message) });

                _logger.LogInformation($"Delivery Timestamp: {result.Timestamp.UtcDateTime} ");
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex ,$"Error occured: {ex.Message}");
            }

            return await Task.FromResult(false);
        }

        public void Dispose()
        {
            // Block until all outstanding produce requests have completed (with or
            // without error).
            kafkaProducer.Flush();
            kafkaProducer.Dispose();
        }
    }
}
