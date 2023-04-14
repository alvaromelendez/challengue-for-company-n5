using Microsoft.EntityFrameworkCore;
using Permissions.Domain.AggregatesModel.EmployeeAggregate;
using Permissions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PermissionDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public EmployeeRepository(PermissionDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Employee Add(Employee employee)
        {
            return _context.Employees.Add(employee).Entity;
        }

        public async Task<Employee> GetAsync(int employeeId)
        {
            var employee = await _context
                            .Employees
                            .Include(x => x.Permissions)
                            .FirstOrDefaultAsync(o => o.Id == employeeId);

            return employee;
        }

        public void Update(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
        }
    }
}
