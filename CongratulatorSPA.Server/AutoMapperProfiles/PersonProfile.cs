using AutoMapper;
using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Models;

namespace CongratulatorSPA.Server.AutoMapperProfiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonModel>();
            CreateMap<PersonModel, Person>();
        }
    }
}
