using AutoMapper;
using EventOrganizerAPI.Entities;
using EventOrganizerAPI.Models.Dto;

namespace EventOrganizerAPI.Mapper
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile() 
        {
            CreateMap<CreateEventDto, Event>()
                .ForMember(r => r.Location,
                c => c.MapFrom(dto => new Location
                {
                    Street = dto.Street,
                    City = dto.City,
                }));

            CreateMap<Event, EventDto>()
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Location.Street))
                .ForMember(m => m.City, c => c.MapFrom(s => s.Location.City));


            CreateMap<UpdateEventDto, Event>()
                    .ForAllMembers(x => x.Condition(
                      (src, dest, sourceValue) => sourceValue != null));
        }
    }
}
