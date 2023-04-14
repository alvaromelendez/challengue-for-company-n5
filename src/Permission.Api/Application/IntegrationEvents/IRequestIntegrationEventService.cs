using Permissions.Api.Dtos;

namespace Permissions.Api.Application.IntegrationEvents
{
    public interface IRequestIntegrationEventService
    {
        public Task<bool> SendRequest(RequestProcessingRequest message);
    }
}
