using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Domain.Exceptions
{
    public class PermissionDomainException : Exception
    {
        public PermissionDomainException()
        { }

        public PermissionDomainException(string message)
            : base(message)
        { }

        public PermissionDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}