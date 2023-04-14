using Nest;
using Permissions.Domain.AggregatesModel.EmployeeAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Infrastructure.Services.Blocks
{
    public class PermissionBlock
    {
        [Keyword]
        public string Id { get; set; }
        [Text]
        public string Comment { get; set; }
        [Date(Format = "date_optional_time")]
        public DateTime DatePermission { get; set; }
        [Nested]
        public EmployeeBlock Employee { get; set; }
        [Nested]
        public PermissionTypeBlock PermissionType { get; set; }

        public class EmployeeBlock
        {
            [Keyword]
            public string FirstName { get; set; }
            [Text]
            public string LastName { get; set; }
        }
        public class PermissionTypeBlock
        {
            [Keyword]
            public int Id { get; set; }
            [Text]
            public string Name { get; set; }
        }
    }
    
}
