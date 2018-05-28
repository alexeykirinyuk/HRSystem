using AutoMapper;
using Microsoft.SharePoint.Client;
using OneInc.ADEditor.Dal.Mapping.SharePoint;
using OneInc.ADEditor.Models;

namespace OneInc.ADEditor.Dal.Mapping
{
    public class SharePointAutomapperProfile : Profile
    {
        public SharePointAutomapperProfile()
            : base($"{nameof(OneInc)}.{nameof(ADEditor)}.{nameof(SharePoint)}")
        {
            AllowNullCollections = true;

            ConfigureMapping();
        }

        private void ConfigureMapping()
        {
            CreateMap<ListItem, Product>().ConvertUsing<SharePointProductConverter>();
            CreateMap<ListItem, Job>().ConvertUsing<SharePointJobConverter>();

            CreateMap<Employee, ListItem>().ConvertUsing<SharePointEmployeeConverter>();
            CreateMap<ListItem, Employee>().ConvertUsing<SharePointEmployeeConverter>();

            CreateMap<ListItem, EmployeeOnBoarding>().ConvertUsing<SharePointListEmployeeOnBoardingConverter>();
        }
    }
}
