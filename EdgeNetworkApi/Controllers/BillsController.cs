using System.Security.Claims;
using EdgeNetworkApplication.Common;
using EdgeNetworkApplication.Dtos.Bills;
using EdgeNetworkApplication.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EdgeNetworkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("bills")]
    public class BillsController : ControllerBase
    {
        private readonly IBillsServices _billsService;

        public BillsController(IBillsServices billsService)
        {
            _billsService = billsService;
        }

        [HttpGet("data-plans")]
        public IActionResult GetDataPlans()
        {
            var plans = _billsService.GetDataPlans();
            return Ok(ApiResponse<object>.Success(plans));
        }

        [HttpPost("airtime")]
        public async Task<IActionResult> PurchaseAirtime([FromBody] PurchaseAirtimeDto dto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(ApiResponse<object>.Failure("Unauthorized."));
            }

            await _billsService.PurchaseAirtimeAsync(dto, userId);
            return Ok(ApiResponse<object?>.Success(null, "Airtime purchased successfully."));
        }

        [HttpPost("data")]
        public async Task<IActionResult> PurchaseData([FromBody] PurchaseDataDto dto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(ApiResponse<object>.Failure("Unauthorized."));
            }

            await _billsService.PurchaseDataAsync(dto, userId);
            return Ok(ApiResponse<object?>.Success(null, "Data purchased successfully"));
        }
    }
}
