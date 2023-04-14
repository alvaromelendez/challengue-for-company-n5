using Permissions.Domain.AggregatesModel.EmployeeAggregate;
using Permissions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Domain.AggregatesModel.PermissionAggregate
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Permission Add(Permission employee);

        void Update(Permission employee);

        Task<Permission> GetAsync(int employeeId);
    }
}
