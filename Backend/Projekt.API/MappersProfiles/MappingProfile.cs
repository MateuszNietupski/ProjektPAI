using AutoMapper;
using Projekt.Entities.Models;
using Projekt.Entities.Models.DTOs;
using Projekt.Entities.Models.DTOs.Requests;

namespace Projekt.MappersProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ImageDto, Image>();
        
    }
}