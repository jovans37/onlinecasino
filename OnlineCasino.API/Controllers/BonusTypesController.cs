// Controllers/EnumsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using OnlineCasino.Application.DTOs;
using OnlineCasino.Application.Extensions;
using OnlineCasino.Domain.Enums;

namespace OnlineCasino.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BonusTypesController : ControllerBase
    {
        [HttpGet("all")]
        public ActionResult<List<EnumTypeDto>> GetBonusTypes()
        {
            var bonusTypes = Enum.GetValues(typeof(BonusType))
                .Cast<BonusType>()
                .Select(bt => new EnumTypeDto
                {
                    Id = (int)bt,
                    Name = EnumHelper.GetDisplayName(bt)
                })
                .ToList();

            return Ok(bonusTypes);
        }

      
    }
}