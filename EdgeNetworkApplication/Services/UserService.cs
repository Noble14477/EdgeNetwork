using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using EdgeNetworkApplication.Common;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkApplication.Interface;
using EdgeNetworkDomain.Entities;
using EdgeNetworkDomain.Interface;

namespace EdgeNetworkApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppUser> RegisterAsync(RegisterUserDto dto, Guid id)
        {
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if(existing is not null)
            {
                throw new InvalidOperationException("A user with this email already exists");
            }

            var user = AppUser.Create(id, dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber);

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        public async Task<AppUser?> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<ApiResponse<RefreshTokenDto>> RefreshTokenAsync(string refreshToken)
        {
            var existing = await _userRepository.GetRefreshTokenAsync(refreshToken);

            if(existing is null || !existing.IsActive)
            {
                return ApiResponse<RefreshTokenDto>.Failure("Invalid or expired refresh token");
            }

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;

            var newRefreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = existing.UserId
            };

            await _userRepository.AddRefreshTokenAsync(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RefreshTokenDto>.Success(new RefreshTokenDto
            {
                RefreshToken = newRefreshToken.Token,
                UserId = existing.UserId 
            });
        }
    }
}
