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
    public class BonusService : IBonusService
    {
        private readonly IBonusRepository _bonusRepository;
        private readonly IBonusAuditLogRepository _auditLogRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<BonusService> _logger;

        public BonusService(
            IBonusRepository bonusRepository,
            IBonusAuditLogRepository auditLogRepository,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ILogger<BonusService> logger)
        {
            _bonusRepository = bonusRepository;
            _auditLogRepository = auditLogRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponse<BonusDto>> GetAllBonusesAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _bonusRepository.GetAllAsync(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bonuses");
                return new PagedResponse<BonusDto>
                {
                    Items = new List<BonusDto>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }

        public async Task<Response<BonusDto>> CreateBonusAsync(CreateBonusRequest request)
        {
            var response = new Response<BonusDto>();

            try
            {
                var operatorName = _currentUserService.GetUserName() ?? "Unknown";

                //check for existing active bonus for player
                var existingBonus = await _bonusRepository.GetActiveBonusByPlayerAndTypeAsync(
                    request.PlayerId, request.Type);

                if (existingBonus != null)
                {
                    response.Message = $"Player already has an active bonus with type: {request.Type}";
                    return response;
                }

                //check for typeid if exists in enum
                if (!EnumHelper.IsValidBonusType(request.Type))
                {
                    response.Message = $"Invalid bonus type: {request.Type}";
                    return response;
                }

                var bonus = new Bonus(request.PlayerId, request.Type, request.Amount, operatorName, request.ExpiresAt);

                await _bonusRepository.AddAsync(bonus);
                await _bonusRepository.SaveChangesAsync();

                //log to audit
                await _auditLogRepository.AddAsync(
                    new BonusAuditLog(bonus.Id, "CREATE", operatorName, null, $"Created bonus: {request.Type} for player {request.PlayerId}"));
                await _auditLogRepository.SaveChangesAsync();

                var bonusDto = _mapper.Map<BonusDto>(bonus);
                return new Response<BonusDto>(bonusDto);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error creating bonus for player {request.PlayerId}";
                _logger.LogError(ex, errorMessage);
                response.Message = errorMessage;
                return response;
            }
        }

        public async Task<Response<BonusDto>> UpdateBonusAsync(int id, UpdateBonusRequest request)
        {
            var response = new Response<BonusDto>();
            try
            {
                var operatorName = _currentUserService.GetUserName() ?? "Unknown";
                var bonus = await _bonusRepository.GetByIdAsync(id);

                if (bonus == null)
                {
                    response.Message = "Bonus not found";
                    return response;
                }

                var oldValues = $"Amount: {bonus.Amount}, Active: {bonus.IsActive}";

                bonus.Update(request.Amount, request.IsActive, operatorName);

                _bonusRepository.Update(bonus);
                await _bonusRepository.SaveChangesAsync();

                // Log audit
                var newValues = $"Amount: {request.Amount}, Active: {request.IsActive}";
                await _auditLogRepository.AddAsync(
                    new BonusAuditLog(bonus.Id, "UPDATE", operatorName, oldValues, newValues));
                await _auditLogRepository.SaveChangesAsync();

                var bonusDto = _mapper.Map<BonusDto>(bonus);
                return new Response<BonusDto>(bonusDto);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error updating bonus {id}";
                _logger.LogError(ex, errorMessage);
                response.Message = errorMessage;
                return response;
            }
        }

        public async Task<Response<BonusDto>> DeleteBonusAsync(int id)
        {
            var response = new Response<BonusDto>();
            try
            {
                var operatorName = _currentUserService.GetUserName() ?? "Unknown";
                var bonus = await _bonusRepository.GetByIdAsync(id);

                if (bonus == null)
                {
                    response.Message = "Bonus not found";
                    return response;
                }

                bonus.Deactivate(operatorName);

                _bonusRepository.Update(bonus);
                await _bonusRepository.SaveChangesAsync();

                // Log audit
                await _auditLogRepository.AddAsync(new BonusAuditLog(
                    bonus.Id, "DELETE", operatorName,
                    $"Bonus: {bonus.Type} for player {bonus.PlayerId}", null));
                await _auditLogRepository.SaveChangesAsync();

                return new Response<BonusDto>(true, "Bonus deleted");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error deleting bonus {id}";
                _logger.LogError(ex, errorMessage);
                response.Message = errorMessage;
                return response;
            }
        }
    }
}
