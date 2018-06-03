namespace HRSystem.ActiveDirectory.Services.Requests.Interfaces
{
    public interface IActiveDirectoryDistinguishedNameBuilderService
    {
        string BuildNamePrefixByName(string name);

        string BuildDistinguishedName(string name, string parentDistinguishedName);

        string GetParentDirectoryFromDistinguishedName(string distinguishedName);
    }
}
