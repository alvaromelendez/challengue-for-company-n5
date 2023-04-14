using MediatR;
using Permissions.Api.Application.Commands;
using Permissions.Domain.AggregatesModel.EmployeeAggregate;
using Permissions.Domain.AggregatesModel.PermissionAggregate;
using System.Linq;

namespace Permissions.Api.Application.Queries
{
    public class GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>>
    {
        public int EmployeeId { get; private set; }
        public GetPermissionsQuery(int employeeId)
        {
            EmployeeId = employeeId;
        }

        public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
        {
            private readonly ILogger<GetPermissionsQueryHandler> _logger;
            private readonly IEmployeeRepository _employeeRepository;
            public GetPermissionsQueryHandler(ILogger<GetPermissionsQueryHandler> logger, IEmployeeRepository employeeRepository)
            {
                _logger = logger;
                _employeeRepository = employeeRepository;
            }
            public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
            {
                var permissions = await _employeeRepository.GetAsync(request.EmployeeId);
                return permissions.Permissions.Select(p => new PermissionDto() { PermissionId = p.Id, DatePermission = p.DatePermission, Comment = p.Comment, PermissionType = p.PermissionType });
            }
        }
    }
}
