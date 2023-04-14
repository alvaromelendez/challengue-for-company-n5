using Microsoft.EntityFrameworkCore;
using Permissions.Domain.AggregatesModel.EmployeeAggregate;
using Permissions.Domain.AggregatesModel.PermissionAggregate;
using Permissions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly PermissionDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public PermissionRepository(PermissionDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Permission Add(Permission permission)
        {
            return _context.Permissions.Add(permission).Entity;
        }

        public async Task<Permission> GetAsync(int employeeId)
        {
            var permission = await _context
                            .Permissions
                            .FirstOrDefaultAsync(o => o.Id == employeeId);

            return permission;
        }

        public void Update(Permission employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
        }
    }
}
