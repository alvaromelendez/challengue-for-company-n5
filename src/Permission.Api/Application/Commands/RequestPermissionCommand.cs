using Azure;
using MediatR;
using Permissions.Domain.AggregatesModel.EmployeeAggregate;
using Permissions.Domain.AggregatesModel.PermissionAggregate;
using Permissions.Domain.SeedWork;
using Permissions.Infrastructure.Repositories;
using Permissions.Infrastructure.Services;
using Permissions.Infrastructure.Services.Blocks;

namespace Permissions.Api.Application.Commands
{
    public class RequestPermissionCommand : IRequest<bool>
    {
        public int EmployeeId { get; private set; }
        public DateTime DatePermission { get; private set; }
        public string Comment { get; private set; }
        public int PermissionType { get; set; }
        public RequestPermissionCommand(int employeeId, DateTime datePermission, string comment, int permissionType)
        {
            EmployeeId = employeeId;
            DatePermission = datePermission;
            Comment = comment;
            PermissionType = permissionType;
        }

        public class RequestPermissionCommandHandler : IRequestHandler<RequestPermissionCommand, bool>
        {
            private readonly ILogger<RequestPermissionCommandHandler> _logger;
            private readonly IEmployeeRepository _employeeRepository;
            private readonly IPermissionRepository _permissionRepository;
            private readonly IPermissionElasticService _permissionElasticService;
            public RequestPermissionCommandHandler(ILogger<RequestPermissionCommandHandler> logger, IEmployeeRepository employeeRepository, IPermissionRepository permissionRepository, IPermissionElasticService permissionElasticService)
            {
                _logger = logger;
                _employeeRepository = employeeRepository;
                _permissionRepository = permissionRepository;
                _permissionElasticService = permissionElasticService;
            }
            public async Task<bool> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
            {
                var employee = await _employeeRepository.GetAsync(request.EmployeeId);

                var newPermission = new Permission(request.DatePermission, request.Comment, employee, Enumeration.FromValue<PermissionType>(request.PermissionType));
                var permissionCurrent = _permissionRepository.Add(newPermission);

                await _permissionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                await _permissionElasticService.CreatePermission(new PermissionBlock { 
                    Id = permissionCurrent.Id.ToString(),
                    Comment = permissionCurrent.Comment,
                    DatePermission = permissionCurrent.DatePermission,
                    Employee = new PermissionBlock.EmployeeBlock 
                    {
                        FirstName = permissionCurrent.Employee.FirstName,
                        LastName = permissionCurrent.Employee.LastName,
                    },
                    PermissionType = new PermissionBlock.PermissionTypeBlock
                    {
                        Id = Enumeration.FromValue<PermissionType>(request.PermissionType).Id,
                        Name = Enumeration.FromValue<PermissionType>(request.PermissionType).Name,
                    },
                });

                return true;
            }
        }
    }
}
