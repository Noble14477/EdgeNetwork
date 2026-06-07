using System.Security.Claims;
using EdgeNetworkApplication.Common;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkApplication.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdgeNetworkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService; 
        private readonly IValidator<FundWalletDto> _fundValidator;
        private readonly IValidator<TransferFundsDto> _transferValidator;

        public WalletController(IWalletService walletService, IValidator<FundWalletDto> fundValidator, IValidator<TransferFundsDto> transferValidator)
        {
            _walletService = walletService;
            _fundValidator = fundValidator;
            _transferValidator = transferValidator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletDto dto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();

            var wallet = await _walletService.CreateWalletAsync(userId, dto.Currency);

            return Ok(ApiResponse<object>.Success(
                new { wallet.Id, AccountNumber = wallet.AccountNumber.Value },
                "Wallet created successfully."));
        }

        [HttpPost("fund")]
        public async Task<IActionResult> FundWallet([FromBody] FundWalletDto dto)
        {
            var validationResult = await _fundValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.Failure(string.Join(", ", errors)));
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(ApiResponse<object>.Failure("Unauthorized."));
            }

            await _walletService.FundWalletAsync(dto, userId);
            return Ok(ApiResponse<object?>.Success(null, "Wallet funded successfully."));
        
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferFundsDto dto)
        {
            var validationResult = await _transferValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.Failure(string.Join(", ", errors)));
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
                return Unauthorized(ApiResponse<object>.Failure("Unauthorized."));

            await _walletService.TransferFundsAsync(dto, userId);
            return Ok(ApiResponse<object?>.Success(null, "Transfer successful."));
        }

        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetWallet(Guid walletId)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
                return Unauthorized(ApiResponse<object>.Failure("Unauthorized."));

            var wallet = await _walletService.GetWalletAsync(walletId, userId);
            if (wallet is null) return NotFound();

            return Ok(ApiResponse<object>.Success(new
            {
                wallet.Id,
                AccountNumber = wallet.AccountNumber.Value,
                Balance = wallet.Balance.Amount,
                Currency = wallet.Balance.Currency,
                wallet.Status
            }));
        }

        [HttpGet("{walletId}/transactions")]
        public async Task<IActionResult> GetTransactionHistory(Guid walletId, [FromQuery] TransactionFilterDto filter)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
                return Unauthorized(ApiResponse<object>.Failure("Unauthorized."));

            var transactions = await _walletService.GetTransactionHistoryAsync(walletId, userId, filter);
            return Ok(ApiResponse<object>.Success(transactions));
        }
    }
}
