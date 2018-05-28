using System.Linq;
using OneInc.ADEditor.ActiveDirectory.Services.Requests.Interfaces;

namespace OneInc.ADEditor.ActiveDirectory.Services.Requests
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

        public string GetNamePrefixFromDistinguishedName(string distinguishedName)
        {
            return distinguishedName.Split(',').First();
        }

        public string GetNameFromDistinguishedName(string distinguishedName)
        {
            var prefix = GetNamePrefixFromDistinguishedName(distinguishedName);

            return prefix.Split('=').Last();
        }

        public string GetParentDirectoryFromDistinguishedName(string distinguishedName)
        {
            return string.Join(",", distinguishedName.Split(',').Skip(1));
        }
    }
}
