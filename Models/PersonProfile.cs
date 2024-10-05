using AutoMapper;
using Trainee_Test.Resources;

namespace Trainee_Test.Models;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PersonEntity, PersonDTO>()
            .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(
                src => src.Married ? MaritalStatus.Married : MaritalStatus.Single));
        CreateMap<PersonDTO, PersonEntity>()
            .ForMember(dest => dest.Married, opt => opt.MapFrom(
                src => src.MaritalStatus == MaritalStatus.Married));

        CreateMap<PersonCsvRow, PersonEntity>().ReverseMap();
    }
}
