using OnlineCasino.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.Interfaces
{
    public interface IBonusAuditLogService
    {
        Task<PagedResponse<BonusAuditLogDto>> GetAllAuditLogsAsync(int pageNumber, int pageSize);
    }
}
