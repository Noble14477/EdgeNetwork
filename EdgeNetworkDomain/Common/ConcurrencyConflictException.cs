using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkDomain.Common
{
    public class ConcurrencyConflictException : Exception
    {
        public ConcurrencyConflictException(string message) : base(message) { }
    }
}
