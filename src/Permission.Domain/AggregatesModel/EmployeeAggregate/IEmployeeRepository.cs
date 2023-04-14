using Permissions.Domain.AggregatesModel.PermissionAggregate;
using Permissions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Domain.AggregatesModel.EmployeeAggregate
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee Add(Employee employee);

        void Update(Employee employee);

        Task<Employee> GetAsync(int employeeId);
    }
}
