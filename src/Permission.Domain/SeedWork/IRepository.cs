using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Domain.SeedWork
{
    public interface IRepository<T> where T : Entity
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
