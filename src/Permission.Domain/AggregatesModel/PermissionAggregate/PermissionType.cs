using Permissions.Domain.Exceptions;
using Permissions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Domain.AggregatesModel.PermissionAggregate
{
    public class PermissionType : Enumeration
    {
        public static PermissionType Health = new PermissionType(1, nameof(Health).ToLowerInvariant());
        public static PermissionType Other = new PermissionType(2, nameof(Other).ToLowerInvariant());
        public PermissionType(int id, string name)
        : base(id, name)
        {
        }

        public static IEnumerable<PermissionType> List() => new[] { Health, Other };
        public static PermissionType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new PermissionDomainException($"Possible values for PermissionType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static PermissionType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new PermissionDomainException($"Possible values for PermissionType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
