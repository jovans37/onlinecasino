using OnlineCasino.Application.DTOs;
using OnlineCasino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.Extensions
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.Name ?? enumValue.ToString();
        }

        
        public static bool IsValidBonusType(BonusType bonusType)
        {
            return Enum.IsDefined(typeof(BonusType), bonusType);
        }
    }
}
