using AutoMapper;
using Domain;
using System.Linq;

namespace bootcamp_api.Core
{
    public class PawssierMappingProfile : Profile
    {
        public PawssierMappingProfile()
        {
            CreateMap<Pet, Dto.Pet>();
            CreateMap<Condition, Dto.Condition>();
            CreateMap<Prescription, Dto.Prescription>();
            CreateMap<Vaccine, Dto.Vaccine>();
            CreateMap<FileLink, Dto.FileLink>();
            CreateMap<Bookmark, Dto.Bookmark>();
            CreateMap<User, Dto.User>();
            CreateMap<CalendarEvent, Dto.CalendarEvent>();
            CreateMap<Message, Dto.Message>();
        }
    }
}
