using MediatR;
using MediatR.Wrappers;
using Permissions.Domain.AggregatesModel.EmployeeAggregate;
using Permissions.Domain.AggregatesModel.PermissionAggregate;
using Permissions.Domain.SeedWork;
using Permissions.Infrastructure.Services;
using Permissions.Infrastructure.Services.Blocks;

namespace Permissions.Api.Application.Commands
{
    public class ModifyPermissionCommand : IRequest<bool>
    {
        public int EmployeeId { get; private set; }
        public int PermissionId { get; private set; }
        public DateTime DatePermission { get; private set; }
        public string Comment { get; private set; }
        public int PermissionType { get; set; }
        public ModifyPermissionCommand(int employeeId, int permissionId, DateTime datePermission, string comment)
        {
            PermissionId = permissionId;
            DatePermission = datePermission;
            Comment = comment;
            EmployeeId = employeeId;
        }

        public class ModifyPermissionCommandHandler : IRequestHandler<ModifyPermissionCommand, bool>
        {
            private readonly ILogger<ModifyPermissionCommandHandler> _logger;
            private readonly IEmployeeRepository _employeeRepository;
            private readonly IPermissionRepository _permissionRepository;
            private readonly IPermissionElasticService _permissionElasticService;
            public ModifyPermissionCommandHandler(ILogger<ModifyPermissionCommandHandler> logger, IPermissionRepository permissionRepository, IEmployeeRepository employeeRepository, IPermissionElasticService permissionElasticService)
            {
                _logger = logger;
                _permissionRepository = permissionRepository;
                _employeeRepository = employeeRepository;
                _permissionElasticService = permissionElasticService;
            }

            public async Task<bool> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
            {
                var employee = await _employeeRepository.GetAsync(request.EmployeeId);

                var permission = await _permissionRepository.GetAsync(request.PermissionId);
                permission.UpdatePermission(request.DatePermission, request.Comment, employee, Enumeration.FromValue<PermissionType>(request.PermissionType));
                _permissionRepository.Update(permission);

                await _permissionElasticService.UpdatePermission(new PermissionBlock
                {
                    Id = permission.Id.ToString(),
                    Comment = permission.Comment,
                    DatePermission = permission.DatePermission,
                    Employee = new PermissionBlock.EmployeeBlock
                    {
                        FirstName = permission.Employee.FirstName,
                        LastName = permission.Employee.LastName,
                    },
                    PermissionType = new PermissionBlock.PermissionTypeBlock
                    {
                        Id = Enumeration.FromValue<PermissionType>(request.PermissionType).Id,
                        Name = Enumeration.FromValue<PermissionType>(request.PermissionType).Name,
                    },
                });

                return await _permissionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
        }
    }
}
