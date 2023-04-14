using Permissions.Domain.AggregatesModel.EmployeeAggregate;
using Permissions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Domain.AggregatesModel.PermissionAggregate
{
    public class Permission : Entity
    {
        public DateTime DatePermission { get; private set; }
        public string Comment { get; private set; }

        public Employee Employee { get; private set; }
        private int _employeeId;

        public PermissionType PermissionType { get; private set; }
        private int _permissionTypeId;

        protected Permission()
        {
        }
        public Permission(DateTime datePermission, string comment, Employee employee, PermissionType permissionType)
        {
            DatePermission = datePermission;
            Comment = comment;
            Employee = employee;
            _employeeId = employee.Id;
            _permissionTypeId = permissionType.Id;
        }

        public void UpdatePermission(DateTime datePermission, string comment, Employee employee, PermissionType permissionType)
        {
            DatePermission = datePermission;
            Comment = comment;
            Employee = employee;
            _employeeId = employee.Id;
            _permissionTypeId = permissionType.Id;
        }
    }
}
