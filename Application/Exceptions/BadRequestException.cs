using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tenant_service.Application.Exceptions
{
    public class BadRequestException: Exception
    {
        public BadRequestException() : base() { }
        public BadRequestException(string message) : base(message) { }
    }
}