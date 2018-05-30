using System.DirectoryServices.Protocols;
using AutoMapper;
using HRSystem.Domain;
using OneInc.ADEditor.Dal.Mapping.ActiveDirectory;

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
        }
    }
}
