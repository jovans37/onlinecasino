using AutoMapper;
using OnlineCasino.Application.DTOs;
using OnlineCasino.Application.Extensions;
using OnlineCasino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to DTO
            CreateMap<Bonus, BonusDto>()
                .ForMember(dest => dest.BonusType, opt => opt.MapFrom(src => src.Type.GetDisplayName()))
                .ForMember(dest => dest.BonusTypeId, opt => opt.MapFrom(src => src.Type));

            CreateMap<CreateBonusRequest, Bonus>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

            CreateMap(typeof(PagedResponse<>), typeof(PagedResponse<>));
        }
    }
}
