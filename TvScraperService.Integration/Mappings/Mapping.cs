using AutoMapper;
using TvScraperService.Core.Models;
using TvScraperService.Integration.Dtos;

namespace TvScraperService.Integration.Mappings
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<PersonDto, Actor>()
                .ForMember(dest => dest.TVMazeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ShowDto, Show>()
                .ForMember(dest => dest.TVMazeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
