using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EdgeNetworkApplication.Common;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkApplication.Interface;
using EdgeNetworkInfrastructure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EdgeNetworkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IValidator<RegisterUserDto> _registerValidator;

        public AuthController(IUserService userService, UserManager<ApplicationUser> userManager, IConfiguration configuration, IValidator<RegisterUserDto> registerValidator)
        {
            _userService = userService;
            _userManager = userManager;
            _configuration = configuration;
            _registerValidator = registerValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var validationResult = await _registerValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.Failure(string.Join(", ", errors)));
            }

            var IdentityUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = dto.Email,
                Email = dto.Email,
            };

            var result = await _userManager.CreateAsync(IdentityUser, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var domainUser = await _userService.RegisterAsync(dto, IdentityUser.Id);

            return Ok(ApiResponse<object>.Success(
                        new { domainUser.Id },
                        "Registration successful."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var IdentityUser = await _userManager.FindByEmailAsync(dto.Email);
            if(IdentityUser is null)
                return Unauthorized("Invalid Credentials");

            var passwordValid = await _userManager.CheckPasswordAsync(IdentityUser, dto.Password);
            if(!passwordValid) return Unauthorized("Invalid credentials");

            var token = GenerateJwtToken(IdentityUser);
            return Ok(ApiResponse<object>.Success(new { token }, "Login successful."));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _userService.RefreshTokenAsync(refreshToken);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            var user = await _userManager.FindByIdAsync(result.Data.UserId.ToString());
            result.Data.AccessToken = GenerateJwtToken(user);
            result.Data.UserId = Guid.Empty;

            return Ok(result);
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}