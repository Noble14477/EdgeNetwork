using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkDomain.Interface
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
