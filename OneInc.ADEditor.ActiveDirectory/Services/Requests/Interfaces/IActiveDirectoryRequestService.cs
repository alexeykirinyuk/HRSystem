using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace OneInc.ADEditor.ActiveDirectory.Services.Requests.Interfaces
{
    public interface IActiveDirectoryRequestService
    {
        AddResponse MakeAddRequest(string id, DirectoryAttribute[] attributes);

        SearchResponse MakeSearchRequest(string path, string filter, SearchScope scope, params string[] attributes);

        ModifyResponse MakeModifyRequest(
            string id,
            DirectoryAttributeOperation operation,
            string attributeName,
            params object[] attributeValues);

        ModifyResponse MakeModifyRequest(string id, IEnumerable<DirectoryAttributeModification> modifications);

        ModifyDNResponse MakeModifyDistinguishedName(string currentDistinguishedName, string parentDistinguishedName, string newName);

        DeleteResponse MakeDeleteRequest(string distinguishedName);
    }
}
