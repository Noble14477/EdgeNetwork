using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Common;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkDomain.Entities;

namespace EdgeNetworkApplication.Interface
{
    public interface IUserService
    {
        Task<AppUser> RegisterAsync(RegisterUserDto user, Guid id);
        Task<AppUser?> GetByIdAsync(Guid id);
        Task<ApiResponse<RefreshTokenDto>> RefreshTokenAsync(string refreshToken);
    }
}
