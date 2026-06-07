using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;
using EdgeNetworkDomain.Interface;
using EdgeNetworkInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EdgeNetworkInfrastructure.Repositories
{
    public class UserRepository : BaseRepository<AppUser>, IUserRepository
    {
        public UserRepository(AppDBContext context) : base(context) { }

        public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.Value == email, ct);
        }
        public async Task AddRefreshTokenAsync(RefreshToken token, CancellationToken ct = default)
        {
            await _context.RefreshTokens.AddAsync(token, ct);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default)
        {
            return await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token, ct);
        }
    }
}
