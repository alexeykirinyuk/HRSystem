using System.Linq;
using HRSystem.ActiveDirectory.Services.Requests.Interfaces;

namespace HRSystem.ActiveDirectory.Services.Requests
{
    public class ActiveDirectoryDistinguishedNameBuilderService : IActiveDirectoryDistinguishedNameBuilderService
    {
        public string BuildNamePrefixByName(string name)
        {
            return $"CN={name}";
        }

        public string BuildDistinguishedName(string name, string parentDistinguishedName)
        {
            var prefix = BuildNamePrefixByName(name);

            return $"{prefix},{parentDistinguishedName}";
        }

        public string GetParentDirectoryFromDistinguishedName(string distinguishedName)
        {
            return string.Join(",", distinguishedName.Split(',').Skip(1));
        }
    }
}