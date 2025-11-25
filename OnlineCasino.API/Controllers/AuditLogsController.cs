using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCasino.Application.DTOs;
using OnlineCasino.Application.Interfaces;

namespace OnlineCasino.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuditLogsController : ControllerBase
    {
        private readonly IBonusAuditLogService _bonusAuditLogService;

        public AuditLogsController(IBonusAuditLogService bonusAuditLogService)
        {
            _bonusAuditLogService = bonusAuditLogService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<PagedResponse<BonusDto>>> GetAllBonusAuditLogs(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _bonusAuditLogService.GetAllAuditLogsAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}