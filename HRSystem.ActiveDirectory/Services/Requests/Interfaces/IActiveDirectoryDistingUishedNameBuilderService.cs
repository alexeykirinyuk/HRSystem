namespace HRSystem.ActiveDirectory.Services.Requests.Interfaces
{
    public interface IActiveDirectoryDistinguishedNameBuilderService
    {
        string BuildNamePrefixByName(string name);

        string BuildDistinguishedName(string name, string parentDistinguishedName);

        string GetNamePrefixFromDistinguishedName(string distinguishedName);

        string GetNameFromDistinguishedName(string distinguishedName);

        string GetParentDirectoryFromDistinguishedName(string distinguishedName);
    }
}
