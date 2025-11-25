using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCasino.Application.DTOs;
using OnlineCasino.Application.Interfaces;

namespace OnlineCasino.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BonusController : ControllerBase
    {
        private readonly IBonusService _bonusService;
        private readonly ILogger<BonusController> _logger;

        public BonusController(IBonusService bonusService, ILogger<BonusController> logger)
        {
            _bonusService = bonusService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<ActionResult<PagedResponse<BonusDto>>> GetAllBonuses(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _bonusService.GetAllBonusesAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<BonusDto>> CreateBonus(CreateBonusRequest request)
        {
            var result = await _bonusService.CreateBonusAsync(request);
            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetAllBonuses), result.Data);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<BonusDto>> UpdateBonus(int id, UpdateBonusRequest request)
        {
            var result = await _bonusService.UpdateBonusAsync(id, request);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBonus(int id)
        {
            var result = await _bonusService.DeleteBonusAsync(id);

            if (!result.Success)
                return BadRequest(new { error = result.Message });

            return NoContent();
        }
    }
}