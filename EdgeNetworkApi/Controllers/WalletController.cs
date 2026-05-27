using System.Security.Claims;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkApplication.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdgeNetworkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService; 

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var wallet = await _walletService.CreateWalletAsync(userId, dto.Currency);
            return Ok(new { wallet.Id, wallet.AccountNumber });
        }

        [HttpPost("fund")]
        public async Task<IActionResult> FundWallet([FromBody] FundWalletDto dto)
        {
            await _walletService.FundWalletAsync(dto);
            return Ok(new { message = "Wallet funded successfully." });
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferFundsDto dto)
        {
            await _walletService.TransferFundsAsync(dto);
            return Ok(new { message = "Transfer successful." });
        }

        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetWallet(Guid walletId)
        {
            var wallet = await _walletService.GetWalletAsync(walletId);
            if (wallet is null) return NotFound();
            return Ok(new { wallet.Id, wallet.AccountNumber, wallet.Balance, wallet.Status });
        }
    }
}
