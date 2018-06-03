using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace HRSystem.ActiveDirectory.Services.Requests.Interfaces
{
    public interface IActiveDirectoryRequestService
    {
        AddResponse MakeAddRequest(string id, DirectoryAttribute[] attributes);

        SearchResponse MakeSearchRequest(string path, string filter, SearchScope scope, params string[] attributes);

        ModifyResponse MakeModifyRequest(
            string distinguishedName,
            DirectoryAttributeOperation operation,
            string attributeName,
            params object[] attributeValues);

        ModifyResponse MakeModifyRequest(string distinguishedName, IEnumerable<DirectoryAttributeModification> modifications);

        ModifyDNResponse MakeModifyDistinguishedName(string currentDistinguishedName, string parentDistinguishedName, string newName);

        DeleteResponse MakeDeleteRequest(string distinguishedName);
    }
}
