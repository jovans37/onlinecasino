using OnlineCasino.SharedLibrary.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Domain.Entities
{
    public class Bonus : BaseEntity
    {
        public int PlayerId { get; private set; }
        public string Type { get; private set; }
        public decimal Amount { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string? UpdatedBy { get; private set; }

        public Bonus(int playerId, string type, decimal amount, string createdBy, DateTime? expiresAt = null)
        {
            PlayerId = playerId;
            Type = type;
            Amount = amount;
            CreatedBy = createdBy;
            ExpiresAt = expiresAt;
        }

        public void Update(decimal amount, bool isActive, string updatedBy)
        {
            Amount = amount;
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

        //on delete
        public void Deactivate(string updatedBy)
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

    }
}
