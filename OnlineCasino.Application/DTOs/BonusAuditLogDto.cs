using OnlineCasino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.DTOs
{
    public class BonusAuditLogDto
    {
        public int BonusId { get; set; }
        public string Action { get; set; }
        public string Operator { get; set; }
        public DateTime Timestamp { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
}
