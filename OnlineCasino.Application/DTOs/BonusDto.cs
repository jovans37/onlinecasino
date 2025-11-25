using OnlineCasino.Domain.Entities;
using OnlineCasino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.DTOs
{
    public class BonusDto
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public BonusType BonusTypeId { get; set; }
        public string BonusType { get; set; }
        public int Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
