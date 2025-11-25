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
    }
}