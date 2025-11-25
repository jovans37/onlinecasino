using OnlineCasino.Domain.Entities;
using OnlineCasino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.DTOs
{
    public class CreateBonusRequest
    {
        [Required]
        public int PlayerId { get; set; }
        [Required]
        public BonusType Type { get; set; }
        [Required]
        public int Amount { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
