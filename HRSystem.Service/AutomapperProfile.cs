using AutoMapper;
using HRSystem.Domain;
using HRSystem.Domain.Attributes.Base;
using HRSystem.Web.Dtos;

namespace HRSystem.Service
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            AllowNullCollections = true;

            ConfigureDtos();
        }

        private void ConfigureDtos()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<AttributeBase, EmployeeAttributeDto>()
                .ForMember(members => members.Value, expression => expression.MapFrom(a => a.GetValueAsString()));
            CreateMap<AttributeInfo, AttributeInfoDto>();
            CreateMap<ActiveDirectoryAttributeInfo, ActiveDirectoryAttributeInfoDto>();
        }
    }
}