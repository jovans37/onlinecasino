using Microsoft.EntityFrameworkCore;
using OnlineCasino.Application.DTOs;
using OnlineCasino.Application.Interfaces;
using OnlineCasino.Domain.Entities;
using OnlineCasino.Infrastructure.Data;

namespace OnlineCasino.Infrastructure.Repositories
{
    public class BonusRepository :  Repository<Bonus>, IBonusRepository
    {
        public BonusRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResponse<BonusDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Bonuses.AsQueryable();
            var totalCount = await query.CountAsync();

            var bonuses = await query
                .OrderByDescending(b => b.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BonusDto
                {
                    Id = b.Id,
                    PlayerId = b.PlayerId,
                    Amount = b.Amount,
                    IsActive = b.IsActive,
                    CreatedAt = b.CreatedAt,
                    ExpiresAt = b.ExpiresAt
                })
                .ToListAsync();

            return new PagedResponse<BonusDto>
            {
                Items = bonuses,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Bonus?> GetActiveBonusByPlayerAndTypeAsync(int playerId, string bonusType)
        {
            return await _context.Bonuses
                .FirstOrDefaultAsync(b =>
                    b.PlayerId == playerId &&
                    b.Type == bonusType &&
                    b.IsActive);
        }
    }
}