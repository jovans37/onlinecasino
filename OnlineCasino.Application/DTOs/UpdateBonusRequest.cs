using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.DTOs
{
    public class UpdateBonusRequest
    {
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
