using AutoMapper;
using Microsoft.Extensions.Logging;
using OnlineCasino.Application.DTOs;
using OnlineCasino.Application.Extensions;
using OnlineCasino.Application.Interfaces;
using OnlineCasino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.Services
{
    public class BonusAuditLogService : IBonusAuditLogService
    {
        private readonly IBonusAuditLogRepository _auditLogRepository;
        private readonly ILogger<BonusAuditLogService> _logger;

        public BonusAuditLogService(
            IBonusAuditLogRepository auditLogRepository,
            ILogger<BonusAuditLogService> logger)
        {
            _auditLogRepository = auditLogRepository;
            _logger = logger;
        }

        public async Task<PagedResponse<BonusAuditLogDto>> GetAllAuditLogsAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _auditLogRepository.GetAllAsync(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit logs");
                return new PagedResponse<BonusAuditLogDto>
                {
                    Items = new List<BonusAuditLogDto>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }
    }
}
