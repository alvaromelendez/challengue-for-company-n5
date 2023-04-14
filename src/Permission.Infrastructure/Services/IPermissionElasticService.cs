using Permissions.Infrastructure.Services.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Infrastructure.Services
{
    public interface IPermissionElasticService
    {
        public Task<bool> CreatePermission(PermissionBlock request);
        public Task<bool> UpdatePermission(PermissionBlock request);
    }
}
