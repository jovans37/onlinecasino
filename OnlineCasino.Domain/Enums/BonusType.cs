using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineCasino.Domain.Enums
{
    public enum BonusType
    {
        [Display(Name = "Welcome")]
        Welcome = 1,
        [Display(Name = "Deposit")]
        Deposit = 2,
        [Display(Name = "Free Spins")]
        FreeSpins = 3
    }
}
