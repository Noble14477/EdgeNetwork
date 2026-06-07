using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;

namespace EdgeNetworkDomain.Interface
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task AddRefreshTokenAsync(RefreshToken token, CancellationToken ct = default);
        Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default);
    }
}
