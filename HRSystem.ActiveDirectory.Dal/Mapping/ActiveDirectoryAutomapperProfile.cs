using System.DirectoryServices.Protocols;
using AutoMapper;
using HRSystem.ActiveDirectory.Dal.Mapping.ActiveDirectory;
using HRSystem.Domain;

namespace HRSystem.ActiveDirectory.Dal.Mapping
{
    public class ActiveDirectoryAutomapperProfile : Profile
    {
        public ActiveDirectoryAutomapperProfile()
        {
            AllowNullCollections = true;

            ConfigureMapping();
        }

        private void ConfigureMapping()
        {
            CreateMap<SearchResultEntry, User>().ConvertUsing<ActiveDirectoryUserConverter>();
        }
    }
}
