using Microsoft.EntityFrameworkCore;
using OnlineCasino.Application.DTOs;
using OnlineCasino.Application.Extensions;
using OnlineCasino.Application.Interfaces;
using OnlineCasino.Domain.Entities;
using OnlineCasino.Infrastructure.Data;

namespace OnlineCasino.Infrastructure.Repositories
{
    public class BonusAuditLogRepository : Repository<BonusAuditLog>, IBonusAuditLogRepository
    {

        public BonusAuditLogRepository(ApplicationDbContext context) : base(context) 
        {
        }

        public async Task<PagedResponse<BonusAuditLogDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.BonusAuditLogs.AsQueryable();
            var totalCount = await query.CountAsync();

            var bonuses = await query
                .OrderByDescending(b => b.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BonusAuditLogDto
                {
                    BonusId = b.Id,
                    Action = b.Action,
                    Operator = b.Operator,
                    OldValues = b.OldValues,
                    NewValues = b.NewValues,
                    Timestamp = b.Timestamp,
                })
                .ToListAsync();

            return new PagedResponse<BonusAuditLogDto>
            {
                Items = bonuses,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}