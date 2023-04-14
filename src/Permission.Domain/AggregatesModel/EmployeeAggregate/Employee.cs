using Permissions.Domain.AggregatesModel.PermissionAggregate;
using Permissions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Domain.AggregatesModel.EmployeeAggregate
{
    public class Employee : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private readonly List<Permission> _permissions;
        public IReadOnlyCollection<Permission> Permissions => _permissions;
        protected Employee() 
        {
            _permissions = new List<Permission>();
        }

        public void AddPermission(int permissionId, DateTime datePermission, string description, PermissionType permissionType)
        {
            var existingPermission = _permissions.Where(o => o.Id == permissionId)
                .SingleOrDefault();

            if (existingPermission != null)
            {
                _permissions.Add(existingPermission);
            }
            else
            {
                var permission = new Permission(datePermission, description, this, permissionType);
                _permissions.Add(permission);
            }
        }
    }
}
