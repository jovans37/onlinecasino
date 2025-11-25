using OnlineCasino.Domain.Entities;
using OnlineCasino.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.Interfaces
{
    public interface IBonusAuditLogRepository : IRepository<BonusAuditLog>
    {
    }
}
