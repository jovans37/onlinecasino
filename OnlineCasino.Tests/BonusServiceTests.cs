using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineCasino.Application.DTOs;
using OnlineCasino.Application.Interfaces;
using OnlineCasino.Application.Services;
using OnlineCasino.Domain.Entities;
using OnlineCasino.Domain.Enums;

namespace OnlineCasino.Tests
{
    public class BonusServiceTests
    {
        private readonly Mock<IBonusRepository> _mockBonusRepository;
        private readonly Mock<IBonusAuditLogRepository> _mockAuditLogRepository;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<BonusService>> _mockLogger;
        private readonly BonusService _bonusService;

        public BonusServiceTests()
        {
            _mockBonusRepository = new Mock<IBonusRepository>();
            _mockAuditLogRepository = new Mock<IBonusAuditLogRepository>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<BonusService>>();

            _bonusService = new BonusService(
                _mockBonusRepository.Object,
                _mockAuditLogRepository.Object,
                _mockCurrentUserService.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task CreateBonusAsync_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new CreateBonusRequest
            {
                PlayerId = 123,
                Type = BonusType.Welcome,
                Amount = 10,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            var operatorName = "operator1";
            var createdBonus = new Bonus(request.PlayerId, request.Type, request.Amount, operatorName, request.ExpiresAt);
            var bonusDto = new BonusDto { Id = 1, PlayerId = request.PlayerId, BonusTypeId = request.Type, Amount = request.Amount };

            _mockCurrentUserService.Setup(x => x.GetUserName())
                .Returns(operatorName);
            _mockBonusRepository.Setup(x => x.GetActiveBonusByPlayerAndTypeAsync(request.PlayerId, request.Type))
                .ReturnsAsync((Bonus)null);
            _mockBonusRepository.Setup(x => x.AddAsync(It.IsAny<Bonus>()))
                .Callback<Bonus>(bonus => createdBonus = bonus);
            _mockMapper.Setup(x => x.Map<BonusDto>(It.IsAny<Bonus>()))
                .Returns(bonusDto);

            // Act
            var result = await _bonusService.CreateBonusAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(bonusDto.Id, result.Data.Id);
            Assert.Equal(request.PlayerId, result.Data.PlayerId);
            Assert.Equal(request.Type, result.Data.BonusTypeId);

            _mockBonusRepository.Verify(x => x.AddAsync(It.IsAny<Bonus>()), Times.Once);
            _mockBonusRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            _mockAuditLogRepository.Verify(x => x.AddAsync(It.IsAny<BonusAuditLog>()), Times.Once);
            _mockAuditLogRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateBonusAsync_PlayerHasExistingBonus_ReturnsErrorMessage()
        {
            // Arrange
            var request = new CreateBonusRequest
            {
                PlayerId = 1,
                Type = BonusType.Welcome,
                Amount = 10,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            var operatorName = "operator1";
            var existingBonus = new Bonus(request.PlayerId, request.Type, 10, operatorName, DateTime.UtcNow.AddDays(1));

            _mockCurrentUserService.Setup(x => x.GetUserName())
                .Returns(operatorName);
            _mockBonusRepository.Setup(x => x.GetActiveBonusByPlayerAndTypeAsync(request.PlayerId, request.Type))
                .ReturnsAsync(existingBonus);

            // Act
            var result = await _bonusService.CreateBonusAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal($"Player already has an active bonus with type: {request.Type}", result.Message);

            // Verify that AddAsync and SaveChangesAsync were NOT called since bonus creation should be blocked
            _mockBonusRepository.Verify(x => x.AddAsync(It.IsAny<Bonus>()), Times.Never);
            _mockBonusRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
            _mockAuditLogRepository.Verify(x => x.AddAsync(It.IsAny<BonusAuditLog>()), Times.Never);
            _mockAuditLogRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateBonusAsync_ExistingBonus_ReturnsBonusDto()
        {
            // Arrange
            var bonusId = 1;
            var updateRequest = new UpdateBonusRequest
            {
                Amount = 20,
                IsActive = false
            };

            var operatorName = "operator1";
            var existingBonus = new Bonus(1, BonusType.Welcome, 20, operatorName, DateTime.UtcNow.AddDays(1));

            var updatedBonusDto = new BonusDto
            {
                Id = bonusId,
                PlayerId = 1,
                BonusTypeId = BonusType.Welcome,
                Amount = updateRequest.Amount,
                IsActive = updateRequest.IsActive
            };

            _mockCurrentUserService.Setup(x => x.GetUserName())
                .Returns(operatorName);
            _mockBonusRepository.Setup(x => x.GetByIdAsync(bonusId))
                .ReturnsAsync(existingBonus);
            _mockMapper.Setup(x => x.Map<BonusDto>(It.IsAny<Bonus>()))
                .Returns(updatedBonusDto);

            // Act
            var result = await _bonusService.UpdateBonusAsync(bonusId, updateRequest);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(updateRequest.Amount, result.Data.Amount);
            Assert.Equal(updateRequest.IsActive, result.Data.IsActive);

            _mockBonusRepository.Verify(x => x.Update(It.IsAny<Bonus>()), Times.Once);
            _mockBonusRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            _mockAuditLogRepository.Verify(x => x.AddAsync(It.IsAny<BonusAuditLog>()), Times.Once);
            _mockAuditLogRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteBonusAsync_ExistingBonus_ReturnsSuccess()
        {
            // Arrange
            var bonusId = 1;
            var operatorName = "operator1";
            var existingBonus = new Bonus(1, BonusType.Welcome, 10, operatorName, DateTime.UtcNow.AddDays(1))
            {
                Id = bonusId
            };

            _mockCurrentUserService.Setup(x => x.GetUserName())
                .Returns(operatorName);
            _mockBonusRepository.Setup(x => x.GetByIdAsync(bonusId))
                .ReturnsAsync(existingBonus);

            // Act
            var result = await _bonusService.DeleteBonusAsync(bonusId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Bonus deleted", result.Message);
            Assert.Null(result.Data); 

            // verify the bonus was deactivated
            _mockBonusRepository.Verify(x => x.Update(It.Is<Bonus>(b => b.Id == bonusId)), Times.Once);
            _mockBonusRepository.Verify(x => x.SaveChangesAsync(), Times.Once);

            // verify audit log was created
            _mockAuditLogRepository.Verify(x => x.AddAsync(It.Is<BonusAuditLog>(log =>
                log.BonusId == bonusId &&
                log.Action == "DELETE" &&
                log.Operator == operatorName &&
                log.OldValues.Contains($"Bonus: {existingBonus.Type} for player {existingBonus.PlayerId}")
            )), Times.Once);
            _mockAuditLogRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteBonusAsync_BonusNotFound_ReturnsErrorMessage()
        {
            // Arrange
            var bonusId = 1;
            var operatorName = "operator1";

            _mockCurrentUserService.Setup(x => x.GetUserName())
                .Returns(operatorName);
            _mockBonusRepository.Setup(x => x.GetByIdAsync(bonusId))
                .ReturnsAsync((Bonus)null);

            // Act
            var result = await _bonusService.DeleteBonusAsync(bonusId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Bonus not found", result.Message);
            Assert.Null(result.Data);

            // verify no operations were performed if the bonus is not found
            _mockBonusRepository.Verify(x => x.Update(It.IsAny<Bonus>()), Times.Never);
            _mockBonusRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
            _mockAuditLogRepository.Verify(x => x.AddAsync(It.IsAny<BonusAuditLog>()), Times.Never);
            _mockAuditLogRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}