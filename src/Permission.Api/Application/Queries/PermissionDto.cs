using Permissions.Domain.AggregatesModel.PermissionAggregate;

namespace Permissions.Api.Application.Queries
{
    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public DateTime DatePermission { get; set; }
        public string Comment { get; set; }
        public PermissionType PermissionType { get; set; }
    }
}
