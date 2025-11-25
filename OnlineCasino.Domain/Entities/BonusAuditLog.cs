using OnlineCasino.SharedLibrary.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Domain.Entities
{
    public class BonusAuditLog : BaseEntity
    {
        public int BonusId { get; private set; }
        public Bonus Bonus { get; private set; }
        public string Action { get; private set; }
        public string Operator { get; private set; }
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
        public string? OldValues { get; private set; }
        public string? NewValues { get; private set; }

        public BonusAuditLog(int bonusId, string action, string @operator, string? oldValues = null, string? newValues = null)
        {
            BonusId = bonusId;
            Action = action;
            Operator = @operator;
            OldValues = oldValues;
            NewValues = newValues;
        }
    }
}
