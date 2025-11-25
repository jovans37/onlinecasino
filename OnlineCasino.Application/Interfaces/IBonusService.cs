using OnlineCasino.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.Interfaces
{
    public interface IBonusService
    {
        Task<PagedResponse<BonusDto>> GetAllBonusesAsync(int pageNumber, int pageSize);
        Task<Response<BonusDto>> CreateBonusAsync(CreateBonusRequest request);
        Task<Response<BonusDto>> UpdateBonusAsync(int id, UpdateBonusRequest request);
        Task<Response<BonusDto>> DeleteBonusAsync(int id);
    }
}
