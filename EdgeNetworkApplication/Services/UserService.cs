using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
