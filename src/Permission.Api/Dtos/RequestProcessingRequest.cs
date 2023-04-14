namespace Permissions.Api.Dtos
{
    public class RequestProcessingRequest
    {
        public Guid Id { get; set; }
        public string NameOperation { get; set; } //“modify”, “request” or “get”.
        public RequestProcessingRequest(string nameOperation)
        {
            Id = Guid.NewGuid();
            NameOperation = nameOperation;
        }
    }
}
