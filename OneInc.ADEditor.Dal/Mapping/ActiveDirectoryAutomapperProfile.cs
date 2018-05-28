using System.DirectoryServices.Protocols;
using AutoMapper;
using OneInc.ADEditor.Dal.Mapping.ActiveDirectory;
using OneInc.ADEditor.Models;

namespace OneInc.ADEditor.Dal.Mapping
{
    public class ActiveDirectoryAutomapperProfile : Profile
    {
        public ActiveDirectoryAutomapperProfile()
            : base($"{nameof(OneInc)}.{nameof(ADEditor)}.{nameof(ActiveDirectory)}")
        {
            AllowNullCollections = true;

            ConfigureMapping();
        }

        private void ConfigureMapping()
        {
            CreateMap<SearchResultEntry, User>().ConvertUsing<ActiveDirectoryUserConverter>();
            CreateMap<SearchResultEntry, Office>().ConvertUsing<ActiveDirectoryOfficeConverter>();
        }
    }
}
