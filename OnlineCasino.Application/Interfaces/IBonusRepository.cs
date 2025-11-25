using OnlineCasino.Application.DTOs;
using OnlineCasino.Domain.Entities;
using OnlineCasino.Domain.Enums;
using OnlineCasino.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.Interfaces
{
    public interface IBonusRepository : IRepository<Bonus>
    {
        Task<PagedResponse<BonusDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<Bonus?> GetActiveBonusByPlayerAndTypeAsync(int playerId, BonusType bonusType);
    }
}
